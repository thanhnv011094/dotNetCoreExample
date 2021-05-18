using example.Admin.Services;
using example.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace example.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserApiClient _userApiClient;
        
        public UserController(ILogger<UserController> logger, IUserApiClient userApiClient)
        {
            _logger = logger;
            _userApiClient = userApiClient;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthenticateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userApiClient.Authenticate(request);
            if (!string.IsNullOrEmpty(result.Token))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(request);
        }
    }
}
