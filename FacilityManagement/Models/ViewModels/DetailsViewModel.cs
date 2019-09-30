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
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(64, ErrorMessage = "This field must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(64, ErrorMessage = "This field must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(64, ErrorMessage = "This field must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(64, ErrorMessage = "This field must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Model")]
        public string Model { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid number.")]
        [Display(Name = "Working Hours")]
        public int WorkingHours { get; set; }

        public virtual ICollection<InventoryObjectTypeDTO> Types { get; set; }
    }
}
