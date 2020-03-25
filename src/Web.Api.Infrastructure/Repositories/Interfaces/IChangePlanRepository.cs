﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IChangePlanRepository
    {
        Task<ChangePlan> GetChangePlanAsync(Guid changePlanId);
        Task<ChangePlanItem> GetChangePlanItemAsync(Guid changePlanItemId);
        Task<PagedList<ChangePlan>> GetChangePlansAsync(Guid? createdById, int page = 1, int pageSize = 10);
        Task<List<ChangePlanItem>> GetChangePlanItemsAsync(Guid changePlanId);
        Task<int> AddChangePlanAsync(ChangePlan changePlan);
        Task<int> AddChangePlanItemAsync(ChangePlanItem changePlanItem);
        Task<int> UpdateChangePlanItemAsync(ChangePlanItem changePlanItem);
        Task<int> DeleteChangePlanAsync(ChangePlan changePlan);
        Task<int> DeleteChangePlanItemAsync(ChangePlanItem changePlanItem);

    }
}
