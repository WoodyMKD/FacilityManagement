using FacilityManagement.API.Models;
using FacilityManagement.API.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class InventoryObjectsController : ControllerBase
    {
        private readonly IInventoryRepository _invObjectRepository;

        public InventoryObjectsController(
            IInventoryRepository invObjectRepository)
        {
            _invObjectRepository = invObjectRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<InventoryObject>> Get()
        {
            return Ok(_invObjectRepository.GetAllInventoryObjects().OrderBy(p => p.Name));
        }
    }
}
