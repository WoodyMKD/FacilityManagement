using FacilityManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<Compressor> GetCompressorByIdAsync(int id, bool includeSystemsAndParts = false)
        {
            IQueryable<Compressor> query = _appDbContext.Compressors;

            if (includeSystemsAndParts)
            {
                query = query.Include(c => c.CompressorSubTypes)
                    .ThenInclude(cst => cst.CompressorSystems)
                        .ThenInclude(cs => cs.Parts);
            } 
            else
            {
                query = query.Include(c => c.CompressorSubTypes);
            }

            query = query.Where(c => c.CompressorId == id);
            
            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<CompressorSystem[]> GetCompressorSystemsBySubTypeIdAsync(int id)
        {
            IQueryable<CompressorSystem> query = _appDbContext.CompressorSystems
                .Include(cs => cs.Parts)
                .Where(c => c.CompressorSubTypeId == id);

            var result = await query.ToArrayAsync();
            return result;
        }

        public async Task<CompressorSystem> GetCompressorSystemByIdAsync(int id)
        {
            IQueryable<CompressorSystem> query = _appDbContext.CompressorSystems
                .Include(cs => cs.Parts)
                .Where(cs => cs.CompressorSystemId == id);

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> UpdateCompressorAsync(Compressor newModel)
        {
            _appDbContext.Update(newModel);
            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<Part> GetPartByIdAsync(int id)
        {
            IQueryable<Part> query = _appDbContext.Parts
                .Where(p => p.PartId == id);

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> UpdatePartAsync(Part newModel)
        {
            _appDbContext.Update(newModel);
            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCompressorSystemAsync(CompressorSystem newModel)
        {
            _appDbContext.CompressorSystems.Update(newModel);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public void DeleteCompressor(Compressor toDelete)
        {
            _appDbContext.Remove(toDelete);
            _appDbContext.SaveChanges();
        }

        public void DeletePart(Part toDelete)
        {
            _appDbContext.Parts.Remove(toDelete);
            _appDbContext.SaveChanges();
        }

        public void AddCompressorSystem(CompressorSystem toAdd)
        {
            _appDbContext.CompressorSystems.Add(toAdd);
            _appDbContext.SaveChanges();
        }

        public void AddCompressorType(CompressorSubType toAdd)
        {
            _appDbContext.CompressorSubTypes.Add(toAdd);
            _appDbContext.SaveChanges();
        }

        public void AddPart(Part toAdd)
        {
            _appDbContext.Parts.Add(toAdd);
            _appDbContext.SaveChanges();
        }

        public void DeleteCompressorSystem(CompressorSystem toDelete)
        {
            _appDbContext.CompressorSystems.Remove(toDelete);
            _appDbContext.SaveChanges();
        }

        public void DeleteCompressorType(CompressorSubType toDelete)
        {
            _appDbContext.CompressorSubTypes.Remove(toDelete);
            _appDbContext.SaveChanges();
        }

        public async Task<bool> UpdateCompressorTypeAsync(CompressorSubType newModel)
        {
            _appDbContext.CompressorSubTypes.Update(newModel);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<CompressorSubType> GetCompressorTypeByIdAsync(int id)
        {
            IQueryable<CompressorSubType> query = _appDbContext.CompressorSubTypes
                .Include(cst => cst.CompressorSystems)
                .Where(cst => cst.CompressorSubTypeId == id);

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<CompressorSubType[]> GetCompressorTypesByCompressorIdAsync(int id)
        {
            IQueryable<CompressorSubType> query = _appDbContext.CompressorSubTypes
                .Where(cst => cst.CompressorId == id);

            var result = await query.ToArrayAsync();
            return result;
        }
    }
}
