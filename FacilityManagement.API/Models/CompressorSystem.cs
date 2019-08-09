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

        public virtual CompressorSubType CompressorSubType { get; set; }
        public virtual ICollection<Part> Parts { get; set; }
    }
}
