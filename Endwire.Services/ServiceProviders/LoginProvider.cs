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
    public class LoginProvider : ILoginProvider
    {
        private readonly ILoginRepository _loginRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IRoleRepository _roleRepository;

        public LoginProvider(ILoginRepository loginRepository, ILocationRepository locationRepository, IRoleRepository roleRepository) {
            _loginRepository = loginRepository;
            _locationRepository = locationRepository;
            _roleRepository = roleRepository;
        }  
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
            var authTokenResult = await _loginRepository.GetAuthTokenAsync(request);
            response.APIResponse = authTokenResult.APIResponse;
            //if(!authTokenResult.Response.Equals("Success"))
            if (authTokenResult.AuthToken.IsNullOrEmpty())
            {
                return response;    
            }
            
            
            var locations = await _locationRepository.GetLocationsAsync(authTokenResult.AuthToken);
            var roles = await _roleRepository.GetRolesAsync(authTokenResult.AuthToken);

            response.AuthToken = authTokenResult.AuthToken;
            response.LocationCount = locations.Count;

            response.Locations = locations.Select(x => new Loc {Id = x.Id, Name = x.Name }).ToList();

            var roleResponse = new RoleResponse 
            {
                Name = roles.Select(x => x.Role).FirstOrDefault(),
                Authorities = MapToAuth(roles)
            };
            response.Roles = roleResponse;

            return response;    
        }

        public async Task<LogoutResponse> LogoutAsync(LogoutRequest request)
        {
            var result =  await _loginRepository.LogoutAsync(request);
            return new LogoutResponse { APIResponse = result.APIResponse, status = result.status };
        }

        private Auth MapToAuth(List<RoleResult> roles)
        {
            var auth = new Auth();
            auth.Broadcast = roles.Where(x => x.Authorities == "Broadcast").Select(x => x.AuthoritiesValue).FirstOrDefault();
            auth.Nudge = roles.Where(x => x.Authorities == "Nudge").Select(x => x.AuthoritiesValue).FirstOrDefault();
            auth.GroupCall = roles.Where(x => x.Authorities == "GroupCall").Select(x => x.AuthoritiesValue).FirstOrDefault();
            auth.VoiceCall = roles.Where(x => x.Authorities == "VoiceCall").Select(x => x.AuthoritiesValue).FirstOrDefault();
            auth.Reminders = roles.Where(x => x.Authorities == "Reminders").Select(x => x.AuthoritiesValue).FirstOrDefault();
            return auth;    
        }

    }

    public interface ILoginProvider
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<LogoutResponse> LogoutAsync(LogoutRequest request);
    }
}
