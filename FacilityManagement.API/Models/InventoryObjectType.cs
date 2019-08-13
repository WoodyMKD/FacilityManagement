using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Models
{
    public class InventoryObjectType
    {
        [Key]
        public int InventoryObjectTypeId { get; set; }
        public string Name { get; set; }

        public int InventoryObjectId { get; set; }
        
        public virtual InventoryObject InventoryObject { get; set; }
        public virtual ICollection<InventoryObjectSystem> Systems { get; set; }
    }
}
