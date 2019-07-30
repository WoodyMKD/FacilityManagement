using FacilityManagement.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<InventoryObject> InventoryObjects { get; set; }
    }
}
