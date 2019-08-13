using AutoMapper;
using FacilityManagement.API.Models;
using FacilityManagement.API.Repositories;
using FacilityManagement.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.API.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    public class InventoryObjectController : ControllerBase
    {
        private readonly IInventoryObjectRepository _inventoryObjectRepository;
        private readonly IMapper _mapper;

        public InventoryObjectController(
            IInventoryObjectRepository inventoryObjectRepository,
            IMapper mapper)
        {
            _inventoryObjectRepository = inventoryObjectRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<InventoryObjectDTO[]>> GetAllInventoryObjectsAsync()
        {
            try
            {
                var results = await _inventoryObjectRepository.GetAllInventoryObjectsAsync();

                return _mapper.Map<InventoryObjectDTO[]>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryObjectDTO>> GetInventoryObjectByIdAsync(int id, 
            bool includeTypes = false, bool includeSystems = false, bool includeParts = false)
        {
            var invObjects = await _inventoryObjectRepository.GetInventoryObjectByIdAsync(id, includeTypes, includeSystems, includeParts);

            if (invObjects == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<InventoryObjectDTO>(invObjects);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInventoryObjectAsync([FromBody] InventoryObjectDTO model)
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

            var invObjects = await _inventoryObjectRepository.GetInventoryObjectByIdAsync(model.InventoryObjectId);
            if (invObjects == null)
            {
                return NotFound();
            }
            
            Mapper.Map(model, invObjects);

            await _inventoryObjectRepository.UpdateInventoryObjectAsync(invObjects);

            return Ok(new { success = true, message = "Add new data success." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryObjectAsync(int id)
        {
            var invObjects = await _inventoryObjectRepository.GetInventoryObjectByIdAsync(id, true, true, true);
            if (invObjects == null)
            {
                return NotFound();
            }

            _inventoryObjectRepository.DeleteInventoryObject(invObjects);

            return Ok(new { success = true, message = "Succ deleted." });
        }
    }
}
