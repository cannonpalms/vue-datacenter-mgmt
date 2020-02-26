﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skoruba.AuditLogging.Services;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Events.Asset;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _repository;
        private readonly IAuditEventLogger _auditEventLogger;

        public AssetService(IAssetRepository repository, IAuditEventLogger auditEventLogger)
        {
            _repository = repository;
            _auditEventLogger = auditEventLogger;
        }

        public async Task<PagedList<AssetDto>> GetAssetsAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await _repository.GetAssetsAsync(search, page, pageSize);
            return pagedList.ToDto();
        }

        public async Task<AssetDto> GetAssetAsync(Guid assetId)
        {
            var asset = await _repository.GetAssetAsync(assetId);
            return asset.ToDto();
        }

        public async Task<List<AssetDto>> GetAssetExportAsync(AssetExportQuery query)
        {
            query = query.ReformatQuery();
            System.Diagnostics.Debug.WriteLine(query.StartRow);
            var assets = await _repository.GetAssetExportAsync(query.Search, query.Hostname, query.StartRow, query.StartCol, query.EndRow, query.EndCol);
            return assets.ToDto();
        }

        public async Task<List<AssetNetworkPortDto>> GetNetworkPortExportAsync(NetworkPortExportQuery query)
        {
            query = query.ReformatQuery();
            System.Diagnostics.Debug.WriteLine(query.StartRow);
            var assets = await _repository.GetNetworkPortExportAsync(query.Search, query.Hostname, query.StartRow, query.StartCol, query.EndRow, query.EndCol);
            return assets.ToDto();
        }

        public async Task<Guid> CreateAssetAsync(AssetDto asset)
        {
            var entity = asset.ToEntity();
            await _repository.AddAssetAsync(entity);

            await _auditEventLogger.LogEventAsync(new AssetCreatedEvent(asset));
            return entity.Id;
        }

        public async Task DeleteAssetAsync(AssetDto asset)
        {
            var entity = asset.ToEntity();
            await _repository.DeleteAssetAsync(entity);

            await _auditEventLogger.LogEventAsync(new AssetDeletedEvent(asset));
        }

        public async Task<int> UpdateAssetAsync(AssetDto asset)
        {
            var entity = asset.ToEntity();
            var updated = await _repository.UpdateAssetAsync(entity);

            await _auditEventLogger.LogEventAsync(new AssetUpdatedEvent(asset));
            return updated;
        }
    }
}