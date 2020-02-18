﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos;
using Web.Api.Mappers;
using Web.Api.Resources;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InstancesController : ControllerBase
    {
        private readonly IInstanceService _instanceService;
        private readonly IApiErrorResources _errorResources;

        public InstancesController(IInstanceService instanceService, IApiErrorResources errorResources)
        {
            _instanceService = instanceService;
            _errorResources = errorResources;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<GetAssetApiDto>>> GetMany(string searchText, int page = 1, int pageSize = 10)
        {
            var instances = await _instanceService.GetInstancesAsync(searchText, page, pageSize);

            var response = instances.MapTo<PagedList<GetAssetApiDto>>();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetAssetApiDto>> Get(Guid id)
        {
            var instance = await _instanceService.GetInstanceAsync(id);

            var response = instance.MapTo<GetAssetApiDto>();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAssetApiDto assetApiDto)
        {
            var assetDto = assetApiDto.MapTo<AssetDto>();
            await _instanceService.CreateInstanceAsync(assetDto);

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _instanceService.DeleteInstanceAsync(id);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Put(UpdateAssetApiDto assetApiDto)
        {
            var assetDto = assetApiDto.MapTo<AssetDto>();
            await _instanceService.UpdateInstanceAsync(assetDto);
            return NoContent();
        }

    }
}
