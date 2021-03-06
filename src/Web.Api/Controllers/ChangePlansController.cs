﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos.Assets.Create;
using Web.Api.Dtos.Assets.Read;
using Web.Api.Dtos.Assets.Update;
using Web.Api.Dtos.ChangePlans;
using Web.Api.Mappers;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ChangePlansController : ControllerBase
    {
        private readonly IChangePlanService _changePlanService;
        private readonly IAssetService _assetService;
        private readonly IMapper _mapper;

        public ChangePlansController(IChangePlanService changePlanService, IAssetService assetService, IMapper mapper)
        {
            _changePlanService = changePlanService;
            _assetService = assetService;
            _mapper = mapper;
        }

        [HttpGet("{id}/changeplan")]
        public async Task<ActionResult<ChangePlanDto>> GetChangePlan(Guid id)
        {
            var response = await _changePlanService.GetChangePlanAsync(id);
            return Ok(response);
        }

        [HttpGet("{id}/changeplanitem")]
        public async Task<ActionResult<ChangePlanItemDto>> GetChangePlanItem(Guid id)
        {
            var response = await _changePlanService.GetChangePlanItemAsync(id);
            return Ok(response);
        }

        [HttpGet("{id}/changeplans")]
        public async Task<ActionResult<List<ChangePlanDto>>> GetChangePlans(Guid id)
        {
            var response = await _changePlanService.GetChangePlansAsync(id);
            return Ok(response);
        }

        [HttpGet("{id}/changeplanitems")]
        public async Task<ActionResult<List<ChangePlanItemDto>>> GetChangePlanItems(Guid id)
        {
            var response = await _changePlanService.GetChangePlanItemsAsync(id);
            foreach (var changePlanItem in response)
            {
                var assetDto = new AssetDto();
                if (changePlanItem.ExecutionType.Equals("create"))
                    assetDto = _mapper.Map<AssetDto>(
                        JsonConvert.DeserializeObject<CreateAssetApiDto>(changePlanItem.NewData));
                else if (changePlanItem.ExecutionType.Equals("update"))
                    assetDto = _mapper.Map<AssetDto>(
                        JsonConvert.DeserializeObject<UpdateAssetApiDto>(changePlanItem.NewData));
                else
                    continue;
                assetDto = await _changePlanService.FillFieldsInAssetApiForChangePlans(assetDto);
                var assetApiDto = _mapper.Map<GetAssetApiDto>(assetDto);
                changePlanItem.NewData = JsonConvert.SerializeObject(assetApiDto);
            }

            return Ok(response);
        }

        [HttpPost("changeplan")]
        public async Task<IActionResult> Post([FromBody] CreateChangePlanApiDto changePlan)
        {
            var changePlanDto = _mapper.Map<ChangePlanDto>(changePlan);
            changePlanDto.CreatedDate = DateTime.Now;
            await _changePlanService.CreateChangePlanAsync(changePlanDto);
            return Ok();
        }

        [HttpPost("changeplanitem")]
        public async Task<IActionResult> Post([FromBody] CreateChangePlanItemApiDto changePlanItem)
        {
            var changePlanItemDto = _mapper.Map<ChangePlanItemDto>(changePlanItem);
            changePlanItemDto.CreatedDate = DateTime.Now;
            await _changePlanService.CreateChangePlanItemAsync(changePlanItemDto);
            return Ok();
        }

        [HttpPut("changeplan")]
        public async Task<IActionResult> Put(ChangePlanDto changePlan)
        {
            await _changePlanService.UpdateChangePlanAsync(changePlan);
            return NoContent();
        }

        [HttpDelete("{id}/changeplan")]
        public async Task<IActionResult> DeleteChangePlan(Guid id)
        {
            await _changePlanService.DeleteChangePlanAsync(id);
            return Ok();
        }

        [HttpDelete("{id}/changeplanitem")]
        public async Task<IActionResult> DeleteChangePlanItem(Guid id)
        {
            await _changePlanService.DeleteChangePlanItemAsync(id);
            return Ok();
        }

        [HttpPut("{id}/execute")]
        public async Task<IActionResult> ExecuteChangePlan(Guid id)
        {
            var changePlan = await _changePlanService.GetChangePlanAsync(id);
            changePlan.ExecutedDate = DateTime.Now;
            await _changePlanService.UpdateChangePlanAsync(changePlan);
            var changePlanItems = await _changePlanService.GetChangePlanItemsAsync(id);
            for (var i = 0; i < changePlanItems.Count(); i++)
            {
                var changePlanItem = changePlanItems[i];
                //NOTE: THE NEWDATA HERE IS A CreateAssetApiDto (assetDto/asset entity cannot be serialized)
                if (changePlanItem.ExecutionType.Equals("create"))
                {
                    var assetApiDto = JsonConvert.DeserializeObject<CreateAssetApiDto>(changePlanItem.NewData);
                    //assetApiDto.LastUpdatedDate = DateTime.Now;
                    var assetDto = _mapper.Map<AssetDto>(assetApiDto);
                    if (assetDto.ChassisId != null && assetDto.ChassisId != Guid.Empty)
                    {
                        var item = await _changePlanService.GetChangePlanItemAsync(assetDto.ChassisId ?? Guid.Empty);
                        if(item != null)
                        {
                            assetDto.ChassisId = item.AssetId;
                        }
                    }
                    await _changePlanService.CreateAssetAsync(assetDto, changePlanItem);
                }
                //NOTE: THE NEWDATA HERE IS A UpdateAssetApiDto (assetDto/asset entity cannot be serialized)
                else if (changePlanItem.ExecutionType.Equals("update"))
                {
                    var assetApiDto = JsonConvert.DeserializeObject<UpdateAssetApiDto>(changePlanItem.NewData);
                    //assetApiDto.LastUpdatedDate = DateTime.Now;
                    var assetDto = _mapper.Map<AssetDto>(assetApiDto);
                    await _assetService.UpdateAssetAsync(assetDto);
                }
                //NOTE: THE NEWDATA HERE IS A DecommissionedAssetDto
                else if (changePlanItem.ExecutionType.Equals("decommission"))
                {
                    var changePlanItemForAsset = await _changePlanService.GetChangePlanItemAsync(changePlanItem.AssetId);
                    if (changePlanItemForAsset != null) changePlanItem.AssetId = changePlanItemForAsset.AssetId;

                    var decommisionedAsset = JsonConvert.DeserializeObject<DecommissionedAssetDto>(changePlanItem.NewData);
                    var createDecommissionedAsset = JsonConvert.DeserializeObject<CreateDecommissionedAsset>(decommisionedAsset.Data);
                    var assetDto = await _assetService.GetAssetForDecommissioning(changePlanItem.AssetId);
                    decommisionedAsset = CreateDecommissionedAsset(assetDto, createDecommissionedAsset);
                    
                    await _assetService.DeleteAssetAsync(decommisionedAsset.Id);
                    await _assetService.CreateDecommissionedAssetAsync(decommisionedAsset);
                }
            }

            return Ok();
        }
        private DecommissionedAssetDto CreateDecommissionedAsset(AssetDto assetDto, CreateDecommissionedAsset decommissionedAssetDto)
        {
            //creating the createDecommissionedAsset from the assetDto
            var createDecommissionedAsset = _mapper.Map<CreateDecommissionedAsset>(assetDto);
            //adding network graph, person who decommissioned, and date decommissioned to decommissioned asset info
            createDecommissionedAsset.NetworkPortGraph = decommissionedAssetDto.NetworkPortGraph;
            createDecommissionedAsset.Decommissioner = decommissionedAssetDto.Decommissioner;
            createDecommissionedAsset.DateDecommissioned = DateTime.Now;
            var jsonString = JsonConvert.SerializeObject(createDecommissionedAsset);
            //creating a new decommissionedAssetDto + filling in the data
            var decommissionedAsset = _mapper.Map<DecommissionedAssetDto>(createDecommissionedAsset);
            decommissionedAsset.Data = jsonString;
            return decommissionedAsset;
        }
    }
}