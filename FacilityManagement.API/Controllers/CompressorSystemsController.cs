using AutoMapper;
using FacilityManagement.API.Models;
using FacilityManagement.API.Models.Repositories;
using FacilityManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Controllers
{
    [Route("api/compressorSystems")]
    [ApiController]
    public class CompressorSystemsController : ControllerBase
    {
        private readonly ICompressorRepository _compressorRepository;
        private readonly IMapper _mapper;

        public CompressorSystemsController(
            ICompressorRepository compressorRepository,
            IMapper mapper)
        {
            _compressorRepository = compressorRepository;
            _mapper = mapper;
        }

        [HttpGet("bySubTypeId/{id}")]
        public async Task<ActionResult<CompressorSystemModel>> GetSystemsBySubTypeIdAsync(int id)
        {
            var compressorSystem = await _compressorRepository.GetCompressorSystemsBySubTypeIdAsync(id);
            
            var result = _mapper.Map<CompressorSystemModel[]>(compressorSystem);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompressorSystem([FromBody] CompressorSystemModel toAddModel)
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
            

            _compressorRepository.AddCompressorSystem(_mapper.Map<CompressorSystem>(toAddModel));

            return Ok(new { success = true, message = "Add new data success." });
        }

        [HttpPut]
        public async Task<IActionResult> EditCompressorSystemAsync([FromBody] CompressorSystemModel model)
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

            var compressorSystem = await _compressorRepository.GetCompressorSystemByIdAsync(model.CompressorSystemId);
            if (compressorSystem == null)
            {
                return NotFound();
            }

            Mapper.Map(model, compressorSystem);
            
            await _compressorRepository.UpdateCompressorSystemAsync(compressorSystem);

            return Ok(new { success = true, message = "Add new data success." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompressorSystemAsync(int id)
        {
            var compressorSystem = await _compressorRepository.GetCompressorSystemByIdAsync(id);
            if (compressorSystem == null)
            {
                return NotFound();
            }

            _compressorRepository.DeleteCompressorSystem(compressorSystem);

            return Ok(new { success = true, message = "Succ deleted." });
        }
    }
}
