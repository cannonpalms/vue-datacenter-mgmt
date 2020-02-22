﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IRackRepository
    {
        Task<PagedList<Rack>> GetRacksAsync(string search, Guid datacenterId, int page = 1, int pageSize = 10);
        Task<List<Rack>> GetRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd, Guid datacenterId);
        Task<Rack> GetRackAsync(string row, int col);
        Task<Rack> GetRackAsync(Guid rackId);
        Task<int> AddRackAsync(Rack rack);
        Task<int> UpdateRackAsync(Rack rack);
        Task<int> DeleteRackAsync(Rack rack);
        Task CreateRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd, Guid datacenterId);
        Task DeleteRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd, Guid datacenterId);
        Task<bool> AddressExistsAsync(string rackRow, int rackColumn, Guid datacenterId);
    }
}