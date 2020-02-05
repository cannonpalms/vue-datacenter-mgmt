﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class RackService : IRackService
    {
        private readonly IRackRepository _rackRepository;

        public RackService(IRackRepository rackRepository)
        {
            _rackRepository = rackRepository;
        }

        public async Task<List<RackDto>> GetRacksAsync(RackRangeQuery query)
        {
            query = query.ToUpper();
            var racks = await _rackRepository.GetRacksInRangeAsync(query.StartRow, query.StartCol, query.EndRow, query.EndCol);
            return racks.ToDto();
        }

        public async Task<PagedList<FlatRackDto>> GetRacksAsync(string search, int page = 1, int pageSize = 10)
        {
            var racks = await _rackRepository.GetRacksAsync(search, page, pageSize);
            return racks.ToFlatDto();
        }

        public async Task CreateRacksAsync(RackRangeQuery query)
        {
            query = query.ToUpper();
            await _rackRepository.CreateRacksInRangeAsync(query.StartRow, query.StartCol, query.EndRow, query.EndCol);
        }

        public async Task DeleteRacksAsync(RackRangeQuery query)
        {
            query = query.ToUpper();
            await _rackRepository.DeleteRacksInRangeAsync(query.StartRow, query.StartCol, query.EndRow, query.EndCol);
        }
    }
}