using System;
using System.Collections.Generic;
using System.Text;

namespace FacilityManagement.DTOs
{
    public class CompressorSystemModel
    {
        public int CompressorSystemId { get; set; }
        public string Name { get; set; }

        public ICollection<PartModel> Parts { get; set; }
    }
}
