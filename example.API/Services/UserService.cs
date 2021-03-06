using example.API.Helpers;
using example.API.Models;
using example.DataProvider.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace example.API.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinManager;
        private readonly RoleManager<Role> _roleManager;

        public UserService(IOptions<AppSettings> appSettings,
            UserManager<User> userManager,
            SignInManager<User> signinManager,
            RoleManager<Role> roleManager)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
        }

        public async Task<AuthenticateUserResponse> Authenticate(AuthenticateUserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            // return null if user not found
            if (user == null) return null;

            var result = await _signinManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return null;
            }

            // authentication successful so generate jwt token
            var token = await generateJwtToken(user);

            return new AuthenticateUserResponse(user, token);
        }

        public async Task<User> GetByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<RegisterUserResponse> Register(RegisterUserRequest request)
        {
            var user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName
            };
            
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return null;
            }

            return new RegisterUserResponse()
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        // helper methods

        private async Task<string> generateJwtToken(User user)
        {
            // generate token that is valid for 3 mins
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var roles = await _userManager.GetRolesAsync(user);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[] {
                        new Claim("UserName", user.UserName.ToString()),
                        new Claim("Roles",string.Join(",", roles))
                    }
                ),
                Expires = DateTime.UtcNow.AddMinutes(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
