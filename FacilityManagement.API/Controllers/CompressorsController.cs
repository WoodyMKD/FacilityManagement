using AutoMapper;
using FacilityManagement.API.Models;
using FacilityManagement.API.Models.Repositories;
using FacilityManagement.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.API.Controllers
{
    [Route("api/compressors")]
    [ApiController]
    public class CompressorsController : Controller
    {
        private readonly ICompressorRepository _compressorRepository;
        private readonly IMapper _mapper;

        public CompressorsController(
            ICompressorRepository compressorRepository,
            IMapper mapper)
        {
            _compressorRepository = compressorRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<CompressorModel[]>> GetAll()
        {
            try
            {
                var results = await _compressorRepository.GetAllCompressorsAsync();

                return _mapper.Map<CompressorModel[]>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompressorModel>> GetCompressorDataAsync(int id)
        {
            var compressor = await _compressorRepository.GetCompressorByIdAsync(id);

            if (compressor == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<CompressorModel>(compressor);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCompressorAsync([FromBody] CompressorModel model)
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

            var compressor = await _compressorRepository.GetCompressorByIdAsync(model.CompressorId);
            if (compressor == null)
            {
                return NotFound();
            }
            
            Mapper.Map(model, compressor);

            await _compressorRepository.UpdateCompressorAsync(compressor);

            return Json(new { success = true, message = "Add new data success." });



            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.CompressorId == 0)
            {
                //_compressorRepository.Add(model);

                //await _compressorRepository.SaveChangesAsync();

                return Json(new { success = true, message = "Add new data success." });
            }
            else
            {
                //_context.Update(todo);

                //await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Edit data success." });
            }
        }
    }
}
