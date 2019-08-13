using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FacilityManagement.DTOs
{
    public class InventoryObjectTypeDTO
    {
        public int InventoryObjectTypeId { get; set; }
        public string Name { get; set; }

        public int InventoryObjectId { get; set; }
        
        public virtual ICollection<InventoryObjectSystemDTO> Systems { get; set; }
    }
}
