﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class UpdateSystemDetailsViewModel
    {
        public int InventoryObjectSystemId { get; set; }
        [Required]
        public string Name { get; set; }
        public int InventoryObjectTypeId { get; set; }
        
        public ICollection<SystemDetailsViewModel> AllSystems { get; set; }
    }
}