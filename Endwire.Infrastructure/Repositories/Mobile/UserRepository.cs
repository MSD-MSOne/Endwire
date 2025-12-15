using Azure.Core;
using EndWire.API.Repositories;
using EndWire.Domain;
using EndWire.Domain.ConfigurationOptions;
using EndWire.Domain.Models;
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
    public class UserRepository : BaseRepository<EndWireContext>, IUserRepository
    {
        private readonly StoredProcedures _storeProcedures;

        public UserRepository(EndWireContext context, IConfiguration configuration, IOptions<StoredProcedures> storedProcedures) : base(context, configuration)
        {
            _storeProcedures = storedProcedures.Value;
        }
        public async Task<UserOnlineStatusResponse> GetUserOnlineStatusAsync(UserRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken,
                UserId = request.UserId
            };
            return await CallStoredProcedureWithResultAsync<UserOnlineStatusResponse>(_storeProcedures.GetUserOnlineStatus.Trim(), data);
        }

        public async Task<UserProfileResult> GetUserProfileAsync(UserRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken,
                UserId = request.UserId
            };
            return await CallStoredProcedureWithResultAsync<UserProfileResult>(_storeProcedures.GetUserProfile.Trim(), data);
        }

        public async Task<List<RoleDto>> GetUserRolesAsync(int userId)
        {
            var data = new
            {
                UserId = userId
            };
            return await CallStoredProcedureWithResultsAsync<RoleDto>(_storeProcedures.GetUserRoles, data);
        }
        public async Task<List<UserLocationResult>> GetUserLocationsAsync(int userId)
        {
            var data = new
            {
                UserId = userId
            };
            return await CallStoredProcedureWithResultsAsync<UserLocationResult>(_storeProcedures.GetUserLocations, data);
        }

        public async Task<List<ContactListResult>> GetContactListAsync(ContactsRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken,
                NudgeId = request.NudgeId
            };
            return await CallStoredProcedureWithResultsAsync<ContactListResult>(_storeProcedures.GetUserListByNudgeId, data);

        }

        public async Task<List<GroupResult>> GetGroupListAsync(string authToken)
        {
            var data = new
            {
                AuthToken = authToken,
            };
            return await CallStoredProcedureWithResultsAsync<GroupResult>(_storeProcedures.GetGroups, data);
        }

        public async Task<List<ContactListResult>> GetGroupUsersListAsync(string authToken, int groupid)
        {
            var data = new
            {
                authtoken = authToken,
                groupid = groupid
            };
            return  await CallStoredProcedureWithResultsAsync<ContactListResult>(_storeProcedures.GetGroupUsersList, data);
           
        }
        public async Task<FavResponse> MarkFavorite(string authToken, int userid)
        {
            var data = new
            {
                AuthToken = authToken,
                UserID = userid
            };
            return await CallStoredProcedureWithResultAsync<FavResponse>("[dbo].[sp_MarkFavoriteContact_API_UPD]", data);
        }

    }

    public interface IUserRepository
    {
        Task<UserOnlineStatusResponse> GetUserOnlineStatusAsync(UserRequest request);
        Task<UserProfileResult> GetUserProfileAsync(UserRequest request);
        Task<List<RoleDto>> GetUserRolesAsync(int userId);
        Task<List<UserLocationResult>> GetUserLocationsAsync(int userId);
        Task<List<ContactListResult>> GetContactListAsync(ContactsRequest request);
        Task<List<GroupResult>> GetGroupListAsync(string authToken);
        Task<List<ContactListResult>> GetGroupUsersListAsync(string authToken, int groupid);
        Task<FavResponse> MarkFavorite(string authToken, int userid);
    }

}
