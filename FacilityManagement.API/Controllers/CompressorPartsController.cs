using AutoMapper;
using FacilityManagement.API.Models;
using FacilityManagement.API.Models.Repositories;
using FacilityManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Controllers
{
    [Route("api/compressorParts")]
    [ApiController]
    public class CompressorPartsController : ControllerBase
    {
        private readonly ICompressorRepository _compressorRepository;
        private readonly IMapper _mapper;

        public CompressorPartsController(
            ICompressorRepository compressorRepository,
            IMapper mapper)
        {
            _compressorRepository = compressorRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddPart([FromBody] PartModel toAddModel)
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


            _compressorRepository.AddPart(_mapper.Map<Part>(toAddModel));

            return Ok(new { success = true, message = "Add new data success." });
        }

        [HttpPut]
        public async Task<IActionResult> EditPartAsync([FromBody] PartModel model)
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

            var part = await _compressorRepository.GetPartByIdAsync(model.PartId);
            if (part == null)
            {
                return NotFound();
            }

            Mapper.Map(model, part);

            await _compressorRepository.UpdatePartAsync(part);

            return Ok(new { success = true, message = "Add new data success." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartAsync(int id)
        {
            var part = await _compressorRepository.GetPartByIdAsync(id);
            if (part == null)
            {
                return NotFound();
            }

            _compressorRepository.DeletePart(part);

            return Ok(new { success = true, message = "Succ deleted." });
        }
    }
}
