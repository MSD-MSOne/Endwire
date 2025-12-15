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
    public class RoleRepository : BaseRepository<EndWireContext>, IRoleRepository
    {
        private readonly StoredProcedures _storeProcedures;

        public RoleRepository(EndWireContext context, IConfiguration configuration, IOptions<StoredProcedures> storedProcedures) : base(context, configuration)
        {
            _storeProcedures = storedProcedures.Value;
        }
        public async Task<List<RoleResult>> GetRolesAsync(string authToken)
        {
            var data = new
            {
                AuthToken = authToken

            };
            return await CallStoredProcedureWithResultsAsync<RoleResult>(_storeProcedures.GetRoleAuthorities, data);
        }
    }

    public interface IRoleRepository
    {
        Task<List<RoleResult>> GetRolesAsync(string authToken);
    }
}
