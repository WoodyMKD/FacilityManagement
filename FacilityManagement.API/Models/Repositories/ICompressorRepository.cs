using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Models.Repositories
{
    public interface ICompressorRepository
    {
        // Compressor
        Task<Compressor[]> GetAllCompressorsAsync();
        Task<Compressor> GetCompressorByIdAsync(int id, bool includeSystemsAndParts = false);
        Task<bool> UpdateCompressorAsync(Compressor newModel);
        void DeleteCompressor(Compressor toDelete);

        Task<CompressorSystem[]> GetCompressorSystemsBySubTypeIdAsync(int id);
        Task<CompressorSystem> GetCompressorSystemByIdAsync(int id);
        Task<bool> UpdateCompressorSystemAsync(CompressorSystem newModel);
        void DeleteCompressorSystem(CompressorSystem toDelete);

        void AddCompressorSystem(CompressorSystem toAddModel);
        void AddCompressorType(CompressorSubType toAddModel);
        Task<CompressorSubType[]> GetCompressorTypesByCompressorIdAsync(int id);
        Task<CompressorSubType> GetCompressorTypeByIdAsync(int id);
        Task<bool> UpdateCompressorTypeAsync(CompressorSubType newModel);
        void DeleteCompressorType(CompressorSubType toDelete);


        void AddPart(Part toAddModel);
        Task<Part> GetPartByIdAsync(int id);
        Task<bool> UpdatePartAsync(Part newModel);
        void DeletePart(Part toDelete);
    }
}
