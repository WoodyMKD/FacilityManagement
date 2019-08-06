using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Models
{
    public class Part
    {
        public int PartId { get; set; }
        public string Name { get; set; }
        public int CompressorSystemId { get; set; }
        
        public CompressorSystem CompressorSystem { get; set; }
    }
}
