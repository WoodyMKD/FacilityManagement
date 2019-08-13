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
    [Route("api/inventory/parts")]
    [ApiController]
    public class InventoryObjectPartsController : ControllerBase
    {
        private readonly IInventoryObjectRepository _inventoryObjectRepository;
        private readonly IMapper _mapper;

        public InventoryObjectPartsController(
            IInventoryObjectRepository inventoryObjectRepository,
            IMapper mapper)
        {
            _inventoryObjectRepository = inventoryObjectRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddInventoryObjectPartAsync([FromBody] InventoryObjectPartDTO toAddModel)
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


            _inventoryObjectRepository.AddInventoryObjectPart(_mapper.Map<InventoryObjectPart>(toAddModel));

            return Ok(new { success = true, message = "Add new data success." });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInventoryObjectPartAsync([FromBody] InventoryObjectPartDTO model)
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

            var part = await _inventoryObjectRepository.GetInventoryObjectPartByIdAsync(model.InventoryObjectPartId);
            if (part == null)
            {
                return NotFound();
            }

            Mapper.Map(model, part);

            await _inventoryObjectRepository.UpdateInventoryObjectPartAsync(part);

            return Ok(new { success = true, message = "Add new data success." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryObjectPartAsync(int id)
        {
            var part = await _inventoryObjectRepository.GetInventoryObjectPartByIdAsync(id);
            if (part == null)
            {
                return NotFound();
            }

            _inventoryObjectRepository.DeleteInventoryObjectPart(part);

            return Ok(new { success = true, message = "Succ deleted." });
        }
    }
}
