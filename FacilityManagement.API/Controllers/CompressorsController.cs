using AutoMapper;
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
    public class CompressorsController : ControllerBase
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
    }
}
