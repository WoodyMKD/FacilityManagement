using AutoMapper;
using FacilityManagement.API.Models;
using FacilityManagement.API.Repositories;
using FacilityManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Controllers
{
    [Route("api/inventory/systems")]
    [ApiController]
    public class InventoryObjectSystemsController : ControllerBase
    {
        private readonly IInventoryObjectRepository _inventoryObjectRepository;
        private readonly IMapper _mapper;

        public InventoryObjectSystemsController(
            IInventoryObjectRepository inventoryObjectRepository,
            IMapper mapper)
        {
            _inventoryObjectRepository = inventoryObjectRepository;
            _mapper = mapper;
        }

        [HttpGet("byTypeId/{id}")]
        public async Task<ActionResult<InventoryObjectSystemDTO>> GetInventoryObjectSystemsByTypeIdAsync(int id)
        {
            var system = await _inventoryObjectRepository.GetInventoryObjectSystemsByTypeIdAsync(id);
            
            var result = _mapper.Map<InventoryObjectSystemDTO[]>(system);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddInventoryObjectSystemAsync([FromBody] InventoryObjectSystemDTO toAddModel)
        {

            if (toAddModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                // return 422 - Unprocessable Entity when validation fails
                return new UnprocessableEntityObjectResult(ModelState);
            }
            

            _inventoryObjectRepository.AddInventoryObjectSystem(_mapper.Map<InventoryObjectSystem>(toAddModel));

            return Ok(new { success = true, message = "Add new data success." });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInventoryObjectSystemAsync([FromBody] InventoryObjectSystemDTO model)
        {

            if (model == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                // return 422 - Unprocessable Entity when validation fails
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var system = await _inventoryObjectRepository.GetInventoryObjectSystemsByIdAsync(model.InventoryObjectSystemId);
            if (system == null)
            {
                return NotFound();
            }

            Mapper.Map(model, system);
            
            await _inventoryObjectRepository.UpdateInventoryObjectSystemAsync(system);

            return Ok(new { success = true, message = "Add new data success." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryObjectSystemAsync(int id)
        {
            var system = await _inventoryObjectRepository.GetInventoryObjectSystemsByIdAsync(id);
            if (system == null)
            {
                return NotFound();
            }

            _inventoryObjectRepository.DeleteInventoryObjectSystem(system);

            return Ok(new { success = true, message = "Succ deleted." });
        }
    }
}
