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
                    new InventoryObject { Name = "Компресор #1", Category = Category.Compressor, Description = "Хала 1", Manufacturer = "Adidas", Model = "Air", WorkingHours = 10, Types = new List<InventoryObjectType>() },
                    new InventoryObject { Name = "Компресор #2", Category = Category.Compressor, Description = "Хала 2", Manufacturer = "Nike", Model = "Boo", WorkingHours = 15, Types = new List<InventoryObjectType>() },
                    new InventoryObject { Name = "Компресор #3", Category = Category.Compressor, Description = "Хала 3", Manufacturer = "Puma", Model = "Foo", WorkingHours = 20, Types = new List<InventoryObjectType>() }
                };
                invObjects.ForEach(c => context.InventoryObjects.Add(c));

                var types = new List<InventoryObjectType>
                {
                    new InventoryObjectType { Name = "LX1", InventoryObjectId = 1, Systems = new List<InventoryObjectSystem>() },
                    new InventoryObjectType { Name = "LX2", InventoryObjectId = 1, Systems = new List<InventoryObjectSystem>() },
                    new InventoryObjectType { Name = "FX1", InventoryObjectId = 1, Systems = new List<InventoryObjectSystem>() },
                    new InventoryObjectType { Name = "FX2", InventoryObjectId = 1, Systems = new List<InventoryObjectSystem>() }
                };
                types.ForEach(cst => context.InventoryObjectTypes.Add(cst));

                invObjects[0].Types.Add(types[0]);
                invObjects[0].Types.Add(types[1]);
                invObjects[0].Types.Add(types[2]);
                invObjects[0].Types.Add(types[3]);

                var systems = new List<InventoryObjectSystem>
                {
                    new InventoryObjectSystem { Name = "Електронски систем", Type = types.FirstOrDefault(cst => cst.InventoryObjectTypeId == 1), Parts = new List<InventoryObjectPart>() }
                };
                systems.ForEach(cs => context.InventoryObjectSystems.Add(cs));

                types[0].Systems.Add(systems[0]);

                var parts = new List<InventoryObjectPart>
                {
                    new InventoryObjectPart { Name = "Филтер", Systems = systems.FirstOrDefault(cs => cs.InventoryObjectSystemId == 1)}
                };
                parts.ForEach(p => context.InventoryObjectParts.Add(p));

                systems[0].Parts.Add(parts[0]);

                context.SaveChanges();
            }

        }
    }
}
