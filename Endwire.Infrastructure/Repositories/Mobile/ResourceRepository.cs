using EndWire.API.Repositories;
using EndWire.Domain.ConfigurationOptions;
using EndWire.Domain.Models;
using EndWire.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Infrastructure.Repositories.Mobile
{
    public class ResourceRepository : BaseRepository<EndWireContext>, IResourceRepository
    {
        private readonly StoredProcedures _storeProcedures;

        public ResourceRepository(EndWireContext context, IConfiguration configuration, IOptions<StoredProcedures> storedProcedures) : base (context, configuration)    
        {
            _storeProcedures = storedProcedures.Value;
        }
        public async Task<List<ResourceResult>> GetResourcesAsync(ResourceRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken,
                NudgeId = request.NudgeId
            };
            return await CallStoredProcedureWithResultsAsync<ResourceResult>(_storeProcedures.GetResourceByNudgeId, data);
        }

        public async Task<AddResourceResponse> AddResourceAsync(AddResourceRequest request)
        {
            var locationIdList = String.Join(",", request.LocationIdList.Select(x => x));
            var data = new
            {
                LocationIdList = locationIdList,
                ResourceName = request.ResourceName,
                Active = request.Active
            };
            return await CallStoredProcedureWithResultAsync<AddResourceResponse>(_storeProcedures.AddNewResource, data);
        }
    }

    public interface IResourceRepository
    {
        Task<List<ResourceResult>> GetResourcesAsync(ResourceRequest request);
        Task<AddResourceResponse> AddResourceAsync(AddResourceRequest request);
    }
        
}
