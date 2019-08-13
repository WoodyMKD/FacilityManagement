using System;
using System.Collections.Generic;
using System.Text;

namespace FacilityManagement.DTOs
{
    public class InventoryObjectSystemDTO
    {
        public int InventoryObjectSystemId { get; set; }
        public string Name { get; set; }

        public int InventoryObjectTypeId { get; set; }

        public virtual ICollection<InventoryObjectPartDTO> Parts { get; set; }
    }
}
