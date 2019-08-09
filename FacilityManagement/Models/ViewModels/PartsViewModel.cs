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
    public class PartsViewModel
    {
        public int CompressorSystemId { get; set; }
        public ICollection<PartModel> Parts { get; set; }
    }
}
