﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class UpdateSystemDetailsViewModel
    {
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "System")]
        public int InventoryObjectSystemId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(64, ErrorMessage = "This field must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public int InventoryObjectTypeId { get; set; }
        
        public ICollection<SystemDetailsViewModel> AllSystems { get; set; }
    }
}
