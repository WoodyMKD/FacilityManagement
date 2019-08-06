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
            this.CreateMap<Compressor, CompressorModel>()
              .ReverseMap();

            this.CreateMap<CompressorSubType, CompressorSubTypeModel>()
              .ReverseMap();

            this.CreateMap<CompressorSystem, CompressorSystemModel>()
              .ReverseMap();
            
            this.CreateMap<Part, PartModel>()
              .ReverseMap();
        }
    }
}
