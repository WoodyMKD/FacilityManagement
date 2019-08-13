using FacilityManagement.API.Models;
using FacilityManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class IndexViewModel
    {
        public List<InventoryObjectDTO> InventoryObjects { get; set; }
    }
}
