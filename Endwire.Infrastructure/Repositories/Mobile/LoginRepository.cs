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
    public class LoginRepository : BaseRepository<EndWireContext>, ILoginRepository
    {
        private readonly StoredProcedures _storeProcedures;

        public LoginRepository(EndWireContext context, IConfiguration configuration, IOptions<StoredProcedures> storedProcedures) : base (context, configuration)    
        {
            _storeProcedures = storedProcedures.Value;
        }
        public async Task<AuthTokenResponse> GetAuthTokenAsync(LoginRequest request)
        {
            var data = new
            {
                UserName = request.UserName,
                PassCode = request.PassCode,
                RegToken = request.RegToken 
            };
            return await CallStoredProcedureWithResultAsync<AuthTokenResponse>(_storeProcedures.GetAuthToken, data);
        }

        public async Task<LogoutResult> LogoutAsync(LogoutRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken
            };
            return await CallStoredProcedureWithResultAsync<LogoutResult>(_storeProcedures.EndSession, data);
        }
    }

    public interface ILoginRepository
    {
        Task<AuthTokenResponse> GetAuthTokenAsync(LoginRequest request);
        Task<LogoutResult> LogoutAsync(LogoutRequest request);
    }
        
}
