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
        Task<Compressor> GetCompressorByIdAsync(int id);
        Task<bool> UpdateCompressorAsync(Compressor newModel);

        Task<CompressorSystem[]> GetCompressorSystemsBySubTypeIdAsync(int id);
    }
}
