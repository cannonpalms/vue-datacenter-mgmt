﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.Api.Common;
using Web.Api.Core;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Resources;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ModelsController : ControllerBase
    {
        private IModelService _modelService;
        private readonly IApiErrorResources _errorResources;

        public ModelsController(IModelService modelService)
        {
            _modelService = modelService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<ModelDto>>> Get(string searchText, int page = 1, int pageSize = 10)
        {
            var users = await _modelService.GetModelsAsync(searchText, page, pageSize);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModelDto>> Get(Guid id)
        {
            var user = await _modelService.GetModelAsync(id);

            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ModelDto modelDto)
        {
            await _modelService.UpdateModelAsync(modelDto);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ModelDto modelDto)
        {
            if (!modelDto.Id.Equals(default))
            {
                return BadRequest(_errorResources.CannotSetId());
            }

            var modelId = await _modelService.CreateModelAsync(modelDto);
            modelDto.Id = modelId;

            return CreatedAtAction(nameof(Get), new {id = modelId}, modelDto);
        }
    }
}