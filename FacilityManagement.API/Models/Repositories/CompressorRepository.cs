using FacilityManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Models.Repositories
{
    public class CompressorRepository: ICompressorRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public CompressorRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Compressor[]> GetAllCompressorsAsync()
        {
            IQueryable<Compressor> query = _appDbContext.Compressors;
            return await query.ToArrayAsync();
        }

        public async Task<Compressor> GetCompressorByIdAsync(int id)
        {
            IQueryable<Compressor> query = _appDbContext.Compressors
                .Include(c => c.CompressorSubTypes)
                    .ThenInclude(cst => cst.CompressorSystems)
                        .ThenInclude(cs => cs.Parts)
                .Where(c => c.CompressorId == id);
            
            var result = await query.FirstOrDefaultAsync();
            return result;
        }
    }
}
