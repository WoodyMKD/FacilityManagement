using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Models
{
    public class CompressorSubType
    {
        [Key]
        public int CompressorSubTypeId { get; set; }
        public string Name { get; set; }
        public int? CompressorId { get; set; }

        [ForeignKey("CompressorId")]
        public Compressor Compressor { get; set; }
        public ICollection<CompressorSystem> CompressorSystems { get; set; }
    }
}
