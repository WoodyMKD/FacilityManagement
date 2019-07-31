using System;
using System.Collections.Generic;
using System.Text;

namespace FacilityManagement.DTOs
{
    public class CompressorSubTypeModel
    {
        public int CompressorSubTypeId { get; set; }
        public string Name { get; set; }
        
        public ICollection<CompressorSystemModel> CompressorSystems { get; set; }
    }
}
