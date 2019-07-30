using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Models
{
    public enum InventoryObjectType
    {
        CHILLER,
        CLIMATIC_CHAMBER,
        COMPRESSOR,
        BOILER
    }

    public class InventoryObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public InventoryObjectType Type { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
    }
}
