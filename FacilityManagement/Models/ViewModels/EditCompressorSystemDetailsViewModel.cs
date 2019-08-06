using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class EditCompressorSystemDetailsViewModel
    {
        public int CompressorSystemId { get; set; }
        [Required]
        public string Name { get; set; }
        public int? CompressorSubTypeId { get; set; }
        
        public ICollection<CompressorSystemDetailsViewModel> AllCompressorSystems { get; set; }
    }
}
