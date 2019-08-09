using FacilityManagement.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class EditCompressorTypeDetailsViewModel
    {
        public int CompressorSubTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public int CompressorId { get; set; }

        public ICollection<CompressorSubTypeModel> AllCompressorTypes { get; set; }
    }
}
