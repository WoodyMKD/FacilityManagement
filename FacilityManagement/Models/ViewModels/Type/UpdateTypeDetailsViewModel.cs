using FacilityManagement.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class UpdateTypeDetailsViewModel
    {
        public int InventoryObjectTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public int InventoryObjectId { get; set; }

        public ICollection<InventoryObjectTypeDTO> AllTypes { get; set; }
    }
}
