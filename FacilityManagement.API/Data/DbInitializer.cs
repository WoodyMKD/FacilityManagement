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
            if (!context.InventoryObjects.Any())
            {
                var invObjects = new List<InventoryObject>
                {
                    new InventoryObject { Name = "Compressor #1", Category = Category.Compressor, Description = "High pressure compressor", Manufacturer = "Hydro-Pac", Model = "C15-25FX-EXT", WorkingHours = 10, Types = new List<InventoryObjectType>() },
                    new InventoryObject { Name = "Compressor #2", Category = Category.Compressor, Description = "Low pressure compressor", Manufacturer = "Hydro-Pac", Model = "C03-60-120/140LX", WorkingHours = 15, Types = new List<InventoryObjectType>() },
                    new InventoryObject { Name = "Chiller #1", Category = Category.Chiller, Description = "Хала 3", Manufacturer = "Puma", Model = "Foo", WorkingHours = 20, Types = new List<InventoryObjectType>() },
                    new InventoryObject { Name = "Chiller #2", Category = Category.Chiller, Description = "Хала 4", Manufacturer = "New Balance", Model = "Koo", WorkingHours = 25, Types = new List<InventoryObjectType>() },
                    new InventoryObject { Name = "Climate chamber #1", Category = Category.ClimateChamber, Description = "Хала 5", Manufacturer = "Converse", Model = "Doo", WorkingHours = 30, Types = new List<InventoryObjectType>() }
                };
                invObjects.ForEach(c => context.InventoryObjects.Add(c));

                var types = new List<InventoryObjectType>
                {
                    new InventoryObjectType { Name = "FX1", InventoryObjectId = 1, Systems = new List<InventoryObjectSystem>() },
                    new InventoryObjectType { Name = "FX2", InventoryObjectId = 1, Systems = new List<InventoryObjectSystem>() },

                    new InventoryObjectType { Name = "LX1", InventoryObjectId = 2, Systems = new List<InventoryObjectSystem>() },
                    new InventoryObjectType { Name = "LX2", InventoryObjectId = 2, Systems = new List<InventoryObjectSystem>() }
                };
                types.ForEach(cst => context.InventoryObjectTypes.Add(cst));

                invObjects[0].Types.Add(types[0]);
                invObjects[0].Types.Add(types[1]);
                invObjects[1].Types.Add(types[2]);
                invObjects[1].Types.Add(types[3]);

                var systems = new List<InventoryObjectSystem>
                {
                    new InventoryObjectSystem { Name = "Detail D", Type = types.FirstOrDefault(cst => cst.InventoryObjectTypeId == 1), Parts = new List<InventoryObjectPart>() },
                    new InventoryObjectSystem { Name = "Detail E", Type = types.FirstOrDefault(cst => cst.InventoryObjectTypeId == 1), Parts = new List<InventoryObjectPart>() },

                    new InventoryObjectSystem { Name = "Detail A", Type = types.FirstOrDefault(cst => cst.InventoryObjectTypeId == 2), Parts = new List<InventoryObjectPart>() }
                };
                systems.ForEach(cs => context.InventoryObjectSystems.Add(cs));

                types[0].Systems.Add(systems[0]);
                types[0].Systems.Add(systems[1]);
                types[2].Systems.Add(systems[2]);

                var parts = new List<InventoryObjectPart>
                {
                    new InventoryObjectPart { Name = "A10-6163", Systems = systems.FirstOrDefault(cs => cs.InventoryObjectSystemId == 1)},
                    new InventoryObjectPart { Name = "A10-6288", Systems = systems.FirstOrDefault(cs => cs.InventoryObjectSystemId == 1)},
                    new InventoryObjectPart { Name = "A10-2374", Systems = systems.FirstOrDefault(cs => cs.InventoryObjectSystemId == 1)},
                    new InventoryObjectPart { Name = "A10-2375", Systems = systems.FirstOrDefault(cs => cs.InventoryObjectSystemId == 1)},
                    new InventoryObjectPart { Name = "A10-2026", Systems = systems.FirstOrDefault(cs => cs.InventoryObjectSystemId == 1)},

                    new InventoryObjectPart { Name = "R-1316", Systems = systems.FirstOrDefault(cs => cs.InventoryObjectSystemId == 2)},
                    new InventoryObjectPart { Name = "A10-2027", Systems = systems.FirstOrDefault(cs => cs.InventoryObjectSystemId == 2)}
                };
                parts.ForEach(p => context.InventoryObjectParts.Add(p));

                systems[0].Parts.Add(parts[0]);
                systems[0].Parts.Add(parts[1]);
                systems[0].Parts.Add(parts[2]);
                systems[0].Parts.Add(parts[3]);
                systems[0].Parts.Add(parts[4]);
                systems[1].Parts.Add(parts[5]);
                systems[1].Parts.Add(parts[6]);

                context.SaveChanges();
            }

        }
    }
}
