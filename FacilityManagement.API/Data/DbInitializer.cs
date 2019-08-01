using FacilityManagement.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Data
{
    public class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Compressors.Any())
            {
                var compressors = new List<Compressor>
                {
                    new Compressor { Name = "Компресор #1", Description = "Хала 1", Manufacturer = "Adidas", Model = "Air", WorkingHours = 10, CompressorSubTypes = new List<CompressorSubType>() },
                    new Compressor { Name = "Компресор #2", Description = "Хала 2", Manufacturer = "Nike", Model = "Boo", WorkingHours = 15, CompressorSubTypes = new List<CompressorSubType>() },
                    new Compressor { Name = "Компресор #3", Description = "Хала 3", Manufacturer = "Puma", Model = "Foo", WorkingHours = 20, CompressorSubTypes = new List<CompressorSubType>() }
                };
                compressors.ForEach(c => context.Compressors.Add(c));

                var compressorSubTypes = new List<CompressorSubType>
                {
                    new CompressorSubType { Name = "LX1", CompressorId = 1, CompressorSystems = new List<CompressorSystem>() },
                    new CompressorSubType { Name = "LX2", CompressorId = 1, CompressorSystems = new List<CompressorSystem>() },
                    new CompressorSubType { Name = "FX1", CompressorId = 1, CompressorSystems = new List<CompressorSystem>() },
                    new CompressorSubType { Name = "FX2", CompressorId = 1, CompressorSystems = new List<CompressorSystem>() }
                };
                compressorSubTypes.ForEach(cst => context.CompressorSubTypes.Add(cst));
                
                compressors[0].CompressorSubTypes.Add(compressorSubTypes[0]);
                compressors[0].CompressorSubTypes.Add(compressorSubTypes[1]);
                compressors[0].CompressorSubTypes.Add(compressorSubTypes[2]);
                compressors[0].CompressorSubTypes.Add(compressorSubTypes[3]);

                var compressorSystems = new List<CompressorSystem>
                {
                    new CompressorSystem { Name = "Електронски систем", CompressorSubType = compressorSubTypes.FirstOrDefault(cst => cst.CompressorSubTypeId == 1), Parts = new List<Part>() }
                };
                compressorSystems.ForEach(cs => context.CompressorSystems.Add(cs));

                compressorSubTypes[0].CompressorSystems.Add(compressorSystems[0]);

                var parts = new List<Part>
                {
                    new Part { Name = "Filter", CompressorSystem = compressorSystems.FirstOrDefault(cs => cs.CompressorSystemId == 1)}
                };
                parts.ForEach(p => context.Parts.Add(p));

                compressorSystems[0].Parts.Add(parts[0]);

                context.SaveChanges();
            }

        }
    }
}
