using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FacilityManagement.DTOs
{
    public class CompressorSubTypeModel
    {
        public int CompressorSubTypeId { get; set; }
        public string Name { get; set; }
        public int CompressorId { get; set; }
        public ICollection<CompressorSystemModel> CompressorSystems { get; set; }
    }
}
