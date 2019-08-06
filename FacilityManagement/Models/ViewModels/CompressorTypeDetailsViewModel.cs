using FacilityManagement.API.Models;
using FacilityManagement.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class CompressorTypeDetailsViewModel
    {
        public int CompressorSubTypeId { get; set; }
        public string Name { get; set; }
        public int? CompressorId { get; set; }
        
        public ICollection<CompressorSystem> CompressorSystems { get; set; }
    }
}
