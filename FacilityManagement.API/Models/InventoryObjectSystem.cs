using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Models
{
    public class InventoryObjectSystem
    {
        [Key]
        public int InventoryObjectSystemId { get; set; }
        public string Name { get; set; }

        public int InventoryObjectTypeId { get; set; }

        public virtual InventoryObjectType Type { get; set; }
        public virtual ICollection<InventoryObjectPart> Parts { get; set; }
    }
}
