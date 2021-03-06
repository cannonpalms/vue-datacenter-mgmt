﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Common.Mappers;
using Web.Api.Core.Dtos;
using Web.Api.Dtos;
using Web.Api.Dtos.Models;
using Web.Api.Dtos.Models.Create;
using Web.Api.Dtos.Models.Read;
using Web.Api.Dtos.Models.Update;

namespace Web.Api.Mappers
{
    public class ModelApiMapperProfile : PaginatedProfile
    {
        public ModelApiMapperProfile()
        {
            CreateMap<ModelDto, GetModelApiDto>()
                .ForMember(o => o.Assets, opts => opts.MapFrom(src => src.Assets))
                .ForMember(o => o.NetworkPorts, opts => opts.MapFrom(src => src.NetworkPorts))
                .ReverseMap();
            CreateMap<AssetDto, GetModelAssetApiDto>()
                .ReverseMap();
            CreateMap<ModelDto, GetModelsApiDto>()
                .ReverseMap();
            CreateMap<ModelDto, CreateModelApiDto>()
                .ForMember(o => o.NetworkPorts, opts => opts.MapFrom(src => src.NetworkPorts))
                .ReverseMap();
            CreateMap<ModelDto, UpdateModelApiDto>()
                .ForMember(o => o.NetworkPorts, opts => opts.MapFrom(src => src.NetworkPorts))
                .ReverseMap();
            CreateMap<ModelNetworkPortDto, CreateModelNetworkPortDto>()
                .ReverseMap();
            CreateMap<ModelNetworkPortDto, UpdateModelNetworkPortDto>()
                .ReverseMap();
            CreateMap<ModelNetworkPortDto, GetModelNetworkPort>()
                .ReverseMap();
        }
    }
}
