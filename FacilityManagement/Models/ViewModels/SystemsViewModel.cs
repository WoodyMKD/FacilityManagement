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
    public class SystemsViewModel
    {
        public int CompressorTypeId { get; set; }
        public ICollection<CompressorSystemModel> Systems { get; set; }
    }
}
