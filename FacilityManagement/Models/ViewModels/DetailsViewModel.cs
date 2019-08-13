using FacilityManagement.API.Models;
using FacilityManagement.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class DetailsViewModel
    {
        public int InventoryObjectId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int WorkingHours { get; set; }

        public virtual ICollection<InventoryObjectTypeDTO> Types { get; set; }
    }
}
