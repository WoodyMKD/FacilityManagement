using FacilityManagement.API.Models;
using FacilityManagement.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class TypesViewModel
    {
        public int InventoryObjectId { get; set; }
        public virtual ICollection<InventoryObjectTypeDTO> Types { get; set; }
    }
}
