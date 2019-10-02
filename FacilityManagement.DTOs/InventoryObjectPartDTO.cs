using System;
using System.Collections.Generic;
using System.Text;

namespace FacilityManagement.DTOs
{
    public class InventoryObjectPartDTO
    {
        public int InventoryObjectPartId { get; set; }
        public string Name { get; set; }
        public int InventoryObjectSystemId { get; set; }
        public bool Functional { get; set; }
    }
}
