using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Models
{
    public class Inspection
    {
        [Key]
        public int InspectionId { get; set; }
        public virtual InventoryObject Object { get; set; }
        public virtual InventoryObjectType Type { get; set; }
        public virtual ICollection<InventoryObjectPart> Parts { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<bool> Functional { get; set; }
    }
}
