using example.DataProvider;
using example.DataProvider.Entities;
using example.DataProvider.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace example.API.Controllers
{
    [Helpers.Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public RoleController(ILogger<RoleController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var all = await _unitOfWork.RoleReponsitory.GetAll();
            _unitOfWork.RoleReponsitory.Add(new Role()
            {
                Name = $"Group {all.Count() + 1}"
            });

            _unitOfWork.Complete();
            return Ok(_unitOfWork.RoleReponsitory.GetAll().Result);
        }
    }
}
