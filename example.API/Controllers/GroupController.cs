using example.DataProvider;
using example.DataProvider.Models;
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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GroupController(ILogger<GroupController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var all = await _unitOfWork.GroupReponsitory.GetAll();
            _unitOfWork.GroupReponsitory.Add(new Group()
            {
                Name = $"Group {all.Count() + 1}"
            });

            _unitOfWork.Complete();
            return Ok(_unitOfWork.GroupReponsitory.GetAll().Result);
        }
    }
}
