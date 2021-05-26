using example.Admin.Helpers;
using example.Admin.Services;
using example.ViewModel.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace example.Admin.Controllers
{
    //[Route("/user/")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserApiClient _userApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;


        public UserController(IHttpContextAccessor httpContextAccessor, ILogger<UserController> logger, IUserApiClient userApiClient, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("user/{id}/{title}", Name = "Index")]
        public IActionResult Index(int id, string title)
        {
            // Get the actual friendly version of the title.
            string friendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(User.Identity.Name);

            // Compare the title with the friendly title.
            if (!string.Equals(friendlyTitle, title, StringComparison.Ordinal))
            {
                // If the title is null, empty or does not match the friendly title, return a 301 Permanent
                // Redirect to the correct friendly URL.
                return this.RedirectToRoutePermanent("Index", new { id = id, title = friendlyTitle });
            }

            // The URL the client has browsed to is correct, show them the view containing the product.
            return View();
        }

        [Authorize]
        [HttpGet("user/{id}")]
        public IActionResult Index(int id)
        {
            return Index(id, "");
        }

        public async Task<IActionResult> Login()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _httpContextAccessor.HttpContext.Session.Remove("Token");
            return View("login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthenticateUserRequest request, string next = "")
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userApiClient.Authenticate(request);
            if (!string.IsNullOrEmpty(result.Token) && authenticationCookie(result.Token, request.RememberMe).Result)
            {
                _httpContextAccessor.HttpContext.Session.SetString("Token", result.Token);

                if (!string.IsNullOrEmpty(next) && Url.IsLocalUrl(next))
                {
                    return Redirect(next);
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
            return View(request);
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-5.0
        /// </summary>
        /// <param name="context"></param>
        /// <param name="token"></param>
        private async Task<bool> authenticationCookie(string token, bool isPersistent)
        {
            try
            {
                IdentityModelEventSource.ShowPII = true;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]);
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(3),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    IsPersistent = isPersistent,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    authProperties);
                return true;
            }
            catch (Exception ex)
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }

            return false;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _httpContextAccessor.HttpContext.Session.Remove("Token");
            return RedirectToAction("login", "user");
        }

        public async Task<IActionResult> Register()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _httpContextAccessor.HttpContext.Session.Remove("Token");



            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userApiClient.Register(request);
            if(!string.IsNullOrEmpty(result.FirstName) && !string.IsNullOrEmpty(result.FirstName))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                _httpContextAccessor.HttpContext.Session.Remove("Token");

                var userLogin = new AuthenticateUserRequest()
                {
                    UserName = request.UserName,
                    Password = request.Password,
                    RememberMe = true
                };

                return await Login(userLogin);
            }

            return View(request);
        }

        public async Task<IActionResult> ForgotPassword(string email)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _httpContextAccessor.HttpContext.Session.Remove("Token");
            return View();
        }
    }
}
