using AutoMapper;
using RestAPI_RealtorAssistant.Entities;
using RestAPI_RealtorAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI_RealtorAssistant.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Realtors is the source, while RealtorWithoutAssetsDto is the destination
            CreateMap<Realtors, ReatorWithoutAssetsDto>();

            CreateMap<Realtors, RealtorDto>();

            CreateMap<RealtorForCreationDto, Realtors>(); // to add a new realtor

            CreateMap<Asset, AssetDto>();

            CreateMap<AssetForCreationDto, Asset>();// to add a new asset

            CreateMap<AssetForUpdateDto, Asset>(); // to update an existing asset
        }
    }
}
