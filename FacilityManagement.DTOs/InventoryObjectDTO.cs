using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.DTOs
{
    public enum Category
    {
        Compressor,
        ClimateChamber,
        Chiller
    }

    public class InventoryObjectDTO
    {
        public int InventoryObjectId { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int WorkingHours { get; set; }

        public virtual ICollection<InventoryObjectTypeDTO> Types { get; set; }
    }
}
