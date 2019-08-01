using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.DTOs
{
    public class CompressorModel
    {
        public int CompressorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int WorkingHours { get; set; }

        public ICollection<CompressorSubTypeModel> CompressorSubTypes;
    }
}
