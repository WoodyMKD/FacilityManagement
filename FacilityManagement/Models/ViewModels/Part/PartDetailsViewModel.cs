using FacilityManagement.API.Models;
using FacilityManagement.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class PartDetailsViewModel
    {
        public int InventoryObjectPartId { get; set; }
        [Required]
        public string Name { get; set; }
        public int InventoryObjectSystemId { get; set; }
    }
}
