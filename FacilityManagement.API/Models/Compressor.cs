using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Models
{
    public class Compressor
    {
        [Key]
        public int CompressorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        
        public ICollection<CompressorSubType> CompressorSubTypes { get; set; } 
    }
}
