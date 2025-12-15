using EndWire.Domain;
using EndWire.Domain.Models;
using EndWire.Infrastructure.Repositories.Mobile;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Services.ServiceProviders
{
    public class ResourceProvider : IResourceProvider
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceProvider(IResourceRepository resourceRepository) {
            _resourceRepository = resourceRepository;
        }
        public async Task<ResourceResponse> GetResourcesAsync(ResourceRequest request)
        {
            var result = await _resourceRepository.GetResourcesAsync(request);

            if(result.IsNullOrEmpty())
            {
                return new ResourceResponse();
            }

            ResourceResponse response = new ResourceResponse
            {
                Resources = result.Select(x => new ResourceDto { ResourceId = x.ResourceId, Name = x.ResourceName }).ToList(),
                APIResponse = result.First().APIResponse
            };
            return response;    
        }

        public async Task<AddResourceResponse> AddResourceAsync(AddResourceRequest request)
        {
            return await _resourceRepository.AddResourceAsync(request);
          }

    }

    public interface IResourceProvider
    {
        Task<ResourceResponse> GetResourcesAsync(ResourceRequest request);
        Task<AddResourceResponse>  AddResourceAsync(AddResourceRequest request);
    }

}
