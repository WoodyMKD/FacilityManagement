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
    [Route("api/compressorTypes")]
    [ApiController]
    public class CompressorTypesController : ControllerBase
    {
        private readonly ICompressorRepository _compressorRepository;
        private readonly IMapper _mapper;

        public CompressorTypesController(
            ICompressorRepository compressorRepository,
            IMapper mapper)
        {
            _compressorRepository = compressorRepository;
            _mapper = mapper;
        }

        [HttpGet("byCompressorId/{id}")]
        public async Task<ActionResult<CompressorSubTypeModel>> GetTypesByControllerIdAsync(int id)
        {
            var compressorType = await _compressorRepository.GetCompressorTypesByCompressorIdAsync(id);

            var result = _mapper.Map<CompressorSubTypeModel[]>(compressorType);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompressorType([FromBody] CompressorSubTypeModel toAddModel)
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
            

            _compressorRepository.AddCompressorType(_mapper.Map<CompressorSubType>(toAddModel));

            return Ok(new { success = true, message = "Add new data success." });
        }

        [HttpPut]
        public async Task<IActionResult> EditCompressorTypeAsync([FromBody] CompressorSubTypeModel model)
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

            var compressorType = await _compressorRepository.GetCompressorTypeByIdAsync(model.CompressorSubTypeId);
            if (compressorType == null)
            {
                return NotFound();
            }

            Mapper.Map(model, compressorType);

            await _compressorRepository.UpdateCompressorTypeAsync(compressorType);

            return Ok(new { success = true, message = "Add new data success." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompressorTypeAsync(int id)
        {
            var compressorType = await _compressorRepository.GetCompressorTypeByIdAsync(id);
            if (compressorType == null)
            {
                return NotFound();
            }

            _compressorRepository.DeleteCompressorType(compressorType);

            return Ok(new { success = true, message = "Succ deleted." });
        }
    }
}
