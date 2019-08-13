using AutoMapper;
using FacilityManagement.API.Models;
using FacilityManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.API.Data
{
    public class FacilityManagementProfile: Profile
    {
        public FacilityManagementProfile()
        {
            this.CreateMap<InventoryObject, InventoryObjectDTO>()
              .ReverseMap();

            this.CreateMap<InventoryObjectType, InventoryObjectTypeDTO>()
              .ReverseMap();

            this.CreateMap<InventoryObjectSystem, InventoryObjectSystemDTO>()
              .ReverseMap();
            
            this.CreateMap<InventoryObjectPart, InventoryObjectPartDTO>()
              .ReverseMap();
        }
    }
}
