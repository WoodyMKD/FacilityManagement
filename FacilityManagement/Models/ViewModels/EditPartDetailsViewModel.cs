using FacilityManagement.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class EditPartDetailsViewModel
    {
        public int PartId { get; set; }
        public string Name { get; set; }
        public int CompressorSystemId { get; set; }
    }
}
