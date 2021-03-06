﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Dtos;
using Web.Api.Dtos.Bulk.Export;

namespace Web.Api.Mappers
{
    public class ExportApiMapperProfile : Profile
    {
        public ExportApiMapperProfile()
        {
            // need to get from full -> export ready DTOs for models and assets 
            CreateMap<ModelDto, ExportModelDto>()
                .ForMember(o => o.mount_type, opts => opts.MapFrom(src => src.MountType))
                .ForMember(o => o.vendor, opts => opts.MapFrom(src => src.Vendor))
                .ForMember(o => o.model_number, opts => opts.MapFrom(src => src.ModelNumber))
                .ForMember(o => o.height, opts => opts.MapFrom(src => src.Height))
                .ForMember(o => o.display_color, opts => opts.MapFrom(src => src.DisplayColor))
                .ForMember(o => o.network_ports, opts => opts.MapFrom(src => src.EthernetPorts))
                .ForMember(o => o.power_ports, opts => opts.MapFrom(src => src.PowerPorts))
                .ForMember(o => o.cpu, opts => opts.MapFrom(src => src.Cpu))
                .ForMember(o => o.memory, opts => opts.MapFrom(src => src.Memory))
                .ForMember(o => o.storage, opts => opts.MapFrom(src => src.Storage))
                .ForMember(o => o.comment, opts => opts.MapFrom(src => src.Comment))
                .ForMember(o => o.network_port_names, opts => opts.MapFrom(src => src.NetworkPorts));
            CreateMap<AssetDto, ExportAssetDto>()
                .ForMember(o => o.asset_number, opts => opts.MapFrom(src => src.AssetNumber))
                .ForMember(o => o.hostname, opts => opts.MapFrom(src => src.Hostname))

                .ForMember(o => o.datacenter, opts => opts.MapFrom(src => src.Rack.Datacenter.Name))
                .ForMember(x => x.offline_site, opt => opt.Ignore())
                .ForMember(o => o.rack, opts => opts.MapFrom(src => src.Rack.RackAddress))
                .ForMember(o => o.rack_position, opts => opts.MapFrom(src => src.RackPosition))
                .ForMember(x => x.chassis_number, opt => opt.Ignore())
                .ForMember(o => o.chassis_slot, opts => opts.Ignore())
                .ForMember(o => o.vendor, opts => opts.MapFrom(src => src.Model.Vendor))
                .ForMember(o => o.model_number, opts => opts.MapFrom(src => src.Model.ModelNumber))
                .ForMember(o => o.owner, opts => opts.MapFrom(src => src.Owner.Username))
                .ForMember(o => o.comment, opts => opts.MapFrom(src => src.Comment))
                .ForMember(o => o.power_port, opts => opts.MapFrom(src => src.PowerPorts))
                .ForMember(o => o.custom_display_color, opts => opts.MapFrom(src => src.CustomDisplayColor))
                .ForMember(o => o.custom_cpu, opts => opts.MapFrom(src => src.CustomCpu))
                .ForMember(o => o.custom_memory, opts => opts.MapFrom(src => src.CustomMemory))
                .ForMember(o => o.custom_storage, opts => opts.MapFrom(src => src.CustomStorage)); 
               
            CreateMap<ModelNetworkPortDto, ExportModelNetworkPortDto>()
                .ForMember(o => o.network_port_name, opts => opts.MapFrom(src => src.Name));
            CreateMap<AssetPowerPortDto, ExportAssetPowerPortDto>()
                .ForMember(o => o.power_port_location, opts => opts.MapFrom(src => src.PduPort.Pdu.Location))
                .ForMember(o => o.power_port_number, opts => opts.MapFrom(src => src.PduPort.Number));
            CreateMap<AssetNetworkPortDto, ExportNetworkPortDto>()
                .ForMember(o => o.src_hostname, opts => opts.MapFrom(src => src.Asset.Hostname))
                .ForMember(o => o.src_port, opts => opts.MapFrom(src => src.ModelNetworkPort.Name))
                .ForMember(o => o.src_mac, opts => opts.MapFrom(src => src.MacAddress))
                .ForMember(o => o.dest_hostname, opts => opts.MapFrom(src => src.ConnectedPort.Asset.Hostname))
                .ForMember(o => o.dest_port, opts => opts.MapFrom(src => src.ConnectedPort.ModelNetworkPort.Name));
        }
    }
}
