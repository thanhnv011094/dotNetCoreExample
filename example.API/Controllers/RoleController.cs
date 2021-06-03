using example.DataProvider;
using example.DataProvider.Entities;
using example.DataProvider.Repositories;
using example.ViewModel.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace example.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinManager;
        private readonly RoleManager<Role> _roleManager;
        public RoleController(ILogger<RoleController> logger,
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            SignInManager<User> signinManager,
            RoleManager<Role> roleManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var all = await _unitOfWork.RoleReponsitory.GetAllAsync();
            //_unitOfWork.RoleReponsitory.Add(new Role()
            //{
            //    Name = $"Group {all.Count() + 1}"
            //});

            //_unitOfWork.Complete();
            return Ok(all);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin1, guest")]
        public async Task<IActionResult> Index(int id)
        {
            var all = await _unitOfWork.RoleReponsitory.FindAsync(x => x.Id == id);
            //_unitOfWork.RoleReponsitory.Add(new Role()
            //{
            //    Name = $"Group {all.Count() + 1}"
            //});

            //_unitOfWork.Complete();
            return Ok(all);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole(CreateRoleRequest request)
        {
            var newRole = new Role()
            {
                Name = request.Name,
                Description = request.Description ?? request.Name
            };

            var result = await _roleManager.CreateAsync(newRole);

            if (result.Succeeded)
            {
                return Ok(newRole);
            }

            return Ok();
        }
    }
}
