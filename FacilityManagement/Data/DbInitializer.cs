using FacilityManagement.Data;
using FacilityManagement.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Data
{
    public class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.InventoryObjects.Any())
            {
                context.AddRange
                (
                    new InventoryObject { Name = "Чилер #1", Type = "chiller", Manufacturer = "Cleveland", Model = "ABC123MK" },
                    new InventoryObject { Name = "Чилер #2", Type = "chiller", Manufacturer = "Nike", Model = "ABC123MK" },
                    new InventoryObject { Name = "Клима Комора #1", Type = "climate-chamber", Manufacturer = "Adidas", Model = "ABC123MK" },
                    new InventoryObject { Name = "Компресор #1", Type = "compressor", Manufacturer = "Puma", Model = "ABC123MK" },
                    new InventoryObject { Name = "Компресор #2", Type = "compressor", Manufacturer = "Gucci", Model = "ABC123MK" },
                    new InventoryObject { Name = "Бојлер #1", Type = "boiler", Manufacturer = "Louis Vitton", Model = "ABC123MK" }
                );

                context.SaveChanges();
            }

        }
    }
}
