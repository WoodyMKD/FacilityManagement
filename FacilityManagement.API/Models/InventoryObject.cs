using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Models
{
    public enum Category
    {
        Compressor,
        Mixer
    }

    public class InventoryObject
    {
        [Key]
        public int InventoryObjectId { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int WorkingHours { get; set; }

        public virtual ICollection<InventoryObjectType> Types { get; set; } 
    }
}
