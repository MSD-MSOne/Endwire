using EndWire.API.Repositories;
using EndWire.Domain.ConfigurationOptions;
using EndWire.Domain.Models;
using EndWire.Domain.Models.API;
using EndWire.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Infrastructure.Repositories.Mobile
{
    public class LocationRepository : BaseRepository<EndWireContext>, ILocationRepository
    {
        private readonly StoredProcedures _storeProcedures;

        public LocationRepository(EndWireContext context, IConfiguration configuration, IOptions<StoredProcedures> storedProcedures) : base(context, configuration)
        {
            _storeProcedures = storedProcedures.Value;
        }
        public async Task<List<LocationResult>> GetLocationsAsync(string authToken)
        {
            var data = new
            {
                AuthToken = authToken
            };
            return await CallStoredProcedureWithResultsAsync<LocationResult>(_storeProcedures.GetLocationList, data);
        }

        public async Task<LocationselectResult> GetLocationselectAsync(string authToken, int locationid)
        {
            var data = new
            {
                AuthToken = authToken,
                Locationid = locationid
            };
            return await CallStoredProcedureWithResultAsync<LocationselectResult>("dbo.sp_SetLocation_API_UPD", data);
        }

        public async Task<LocationlistResponse> GetLocationlistAsync(string authToken)
        {
            var locations = await GetLocationsAsync(authToken);
            if (locations.IsNullOrEmpty())
            {
                return null;
            }
            LocationlistResponse response = new LocationlistResponse();           
            response.LocationCount = locations.Count();
            response.Locations = locations.Select(x => new Loc { Id = x.Id, Name = x.Name }).ToList();
            response.APIResponse = locations.FirstOrDefault().APIResponse;
            return response;
        }
    }

    public interface ILocationRepository
    {
        Task<List<LocationResult>> GetLocationsAsync(string authToken);
        Task<LocationlistResponse> GetLocationlistAsync(string authToken);
        Task<LocationselectResult> GetLocationselectAsync(string authToken, int locationid);
    }

}