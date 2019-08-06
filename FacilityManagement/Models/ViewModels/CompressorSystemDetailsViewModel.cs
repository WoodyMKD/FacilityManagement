using FacilityManagement.API.Models;
using FacilityManagement.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class CompressorSystemDetailsViewModel
    {
        public int CompressorSystemId { get; set; }
        [Required]
        public string Name { get; set; }

        public int? CompressorSubTypeId { get; set; }
        public ICollection<PartModel> Parts { get; set; }

        public static ICollection<CompressorSystemDetailsViewModel> AllCompressorSystems { get; set; }
    }
}
