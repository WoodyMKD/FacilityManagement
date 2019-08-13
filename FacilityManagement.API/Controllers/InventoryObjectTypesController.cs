using AutoMapper;
using FacilityManagement.API.Models;
using FacilityManagement.API.Repositories;
using FacilityManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Controllers
{
    [Route("api/inventory/types")]
    [ApiController]
    public class InventoryObjectTypesController : ControllerBase
    {
        private readonly IInventoryObjectRepository _inventoryObjectRepository;
        private readonly IMapper _mapper;

        public InventoryObjectTypesController(
            IInventoryObjectRepository inventoryObjectRepository,
            IMapper mapper)
        {
            _inventoryObjectRepository = inventoryObjectRepository;
            _mapper = mapper;
        }

        [HttpGet("byInventoryObjectId/{id}")]
        public async Task<ActionResult<InventoryObjectTypeDTO>> GetInventoryObjectTypesByInventoryObjectIdAsync(int id)
        {
            var type = await _inventoryObjectRepository.GetInventoryObjectTypesByInventoryObjectIdAsync(id);

            var result = _mapper.Map<InventoryObjectTypeDTO[]>(type);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddInventoryObjectTypeAsync([FromBody] InventoryObjectTypeDTO toAddModel)
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
            

            _inventoryObjectRepository.AddInventoryObjectType(_mapper.Map<InventoryObjectType>(toAddModel));

            return Ok(new { success = true, message = "Add new data success." });
        }

        [HttpPut]
        public async Task<IActionResult> EditInventoryObjectTypeAsync([FromBody] InventoryObjectTypeDTO model)
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

            var type = await _inventoryObjectRepository.GetInventoryObjectTypeByIdAsync(model.InventoryObjectTypeId);
            if (type == null)
            {
                return NotFound();
            }

            Mapper.Map(model, type);

            await _inventoryObjectRepository.UpdateInventoryObjectTypeAsync(type);

            return Ok(new { success = true, message = "Add new data success." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryObjectTypeAsync(int id)
        {
            var type = await _inventoryObjectRepository.GetInventoryObjectTypeByIdAsync(id);
            if (type == null)
            {
                return NotFound();
            }

            _inventoryObjectRepository.DeleteInventoryObjectType(type);

            return Ok(new { success = true, message = "Succ deleted." });
        }
    }
}
