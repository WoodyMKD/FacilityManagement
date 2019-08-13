using FacilityManagement.API.Models;
using FacilityManagement.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class TypeDetailsViewModel
    {
        public int InventoryObjectTypeId { get; set; }
        public string Name { get; set; }
        public int InventoryObjectId { get; set; }
        
        public ICollection<InventoryObjectSystem> Systems { get; set; }
    }
}
