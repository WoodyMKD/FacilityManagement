using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Models
{
    public class InventoryObjectPart
    {
        [Key]
        public int InventoryObjectPartId { get; set; }
        public string Name { get; set; }

        public int InventoryObjectSystemId { get; set; }
        
        public virtual InventoryObjectSystem Systems { get; set; }
    }
}
