using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Models
{
    public class CompressorSystem
    {
        public int CompressorSystemId { get; set; }
        public string Name { get; set; }
        public int CompressorSubTypeId { get; set; }

        public CompressorSubType CompressorSubType { get; set; }
        public ICollection<Part> Parts { get; set; }
    }
}
