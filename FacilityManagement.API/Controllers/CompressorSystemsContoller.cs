using AutoMapper;
using FacilityManagement.API.Models.Repositories;
using FacilityManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Controllers
{
    [Route("api/compressorSystems")]
    [ApiController]
    public class CompressorSystemsContoller : ControllerBase
    {
        private readonly ICompressorRepository _compressorRepository;
        private readonly IMapper _mapper;

        public CompressorSystemsContoller(
            ICompressorRepository compressorRepository,
            IMapper mapper)
        {
            _compressorRepository = compressorRepository;
            _mapper = mapper;
        }

        [HttpGet("byCompressorId/{id}")]
        public async Task<ActionResult<CompressorSystemModel>> GetSystemsByCompressorIdAsync(int id)
        {
            var compressorSystem = await _compressorRepository.GetCompressorSystemsBySubTypeIdAsync(id);

            if (compressorSystem == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<CompressorSystemModel[]>(compressorSystem);

            return Ok(result);
        }
    }
}
