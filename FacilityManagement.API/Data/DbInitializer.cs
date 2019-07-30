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
                context.AddRange
                (
                    new InventoryObject { Name = "Чилер #1", Type = InventoryObjectType.CHILLER, Manufacturer = "Cleveland", Model = "ABC123MK" },
                    new InventoryObject { Name = "Чилер #2", Type = InventoryObjectType.CHILLER, Manufacturer = "Nike", Model = "ABC123MK" },
                    new InventoryObject { Name = "Клима Комора #1", Type = InventoryObjectType.CLIMATIC_CHAMBER, Manufacturer = "Adidas", Model = "ABC123MK" },
                    new InventoryObject { Name = "Компресор #1", Type = InventoryObjectType.COMPRESSOR, Manufacturer = "Puma", Model = "ABC123MK" },
                    new InventoryObject { Name = "Компресор #2", Type = InventoryObjectType.COMPRESSOR, Manufacturer = "Gucci", Model = "ABC123MK" },
                    new InventoryObject { Name = "Бојлер #1", Type = InventoryObjectType.BOILER, Manufacturer = "Louis Vitton", Model = "ABC123MK" }
                );

                context.SaveChanges();
            }

        }
    }
}
