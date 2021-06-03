using example.API.Services;
using example.DataProvider;
using example.DataProvider.Entities;
using example.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace example.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api

        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUserService _userService; 
        public UserController(ILogger<UserController> logger,
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            SignInManager<User> signinManager,
            RoleManager<Role> roleManager,
            IUserService userService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateUserRequest request)
        {
            var response = await _userService.Authenticate(request);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var response = await _userService.Register(request);

            if (response == null)
                return BadRequest(new { message = "Can't create user, try again" });

            return Ok(response);
        }

        [HttpPost("set-role")]
        public async Task<IActionResult> SetRole(SetRoleUserRequest request)
        {
            var response = await _userService.SetRoleUser(request);

            if (response == null)
                return BadRequest(new { message = $"Can't set role '{request.RoleName}' for user '{request.UserName}', try again" });

            return Ok(response);
        }
    }
}
