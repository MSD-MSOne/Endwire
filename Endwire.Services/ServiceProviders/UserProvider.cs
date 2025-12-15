using Azure.Core;
using EndWire.Core;
using EndWire.Domain;
using EndWire.Domain.Models;
using EndWire.Domain.Models.API;
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
    public class UserProvider : IUserProvider
    {
        private readonly IUserRepository _userRepository;

        public UserProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserOnlineStatusResponse> GetUserOnlineStatusAsync(UserRequest request)
        {
            UserOnlineStatusResponse response = await _userRepository.GetUserOnlineStatusAsync(request);
            // response.APIResponse = response.UserId > 0 ? APIResponse.success.ToString() : APIResponse.failure.ToString();
            return response;
        }

        public async Task<UserProfileResult> GetUserProfileAsync(UserRequest request)
        {
            return await _userRepository.GetUserProfileAsync(request);
        }
        public async Task<List<RoleDto>> GetUserRolesAsync(int userId)
        {
            return await _userRepository.GetUserRolesAsync(userId);
        }

        public async Task<List<UserLocationResult>> GetUserLocationsAsync(int userId)
        {
            return await _userRepository.GetUserLocationsAsync(userId);
        }

        public async Task<List<ContactListResult>> GetContactListAsync(ContactsRequest request)
        {
            return await _userRepository.GetContactListAsync(request);
        }
        public async Task<GroupResultModel> GetGroupListAsync(string authToken)
        {
            var results = await _userRepository.GetGroupListAsync(authToken);
            if (results.IsNullOrEmpty())
            {
                return null;
            }
            GroupResultModel response = new GroupResultModel();
            response.APIResponse = results.FirstOrDefault().APIResponse;
            if(results.FirstOrDefault().GroupId > 0)
            {
                response.GroupResults = results.Select(x => new GroupResultDto { GroupId = x.GroupId, Group = x.Group }).ToList();
            }
            return response;
        }

        public async Task<ContactListResultModel> GetGroupUsersListAsync(GroupUsersList request)
        {
            var contacts = await _userRepository.GetGroupUsersListAsync(request.AuthToken, request.GroupId);
            if (contacts.IsNullOrEmpty())
            {
                return null;
            }
            ContactListResultModel response = new ContactListResultModel();
            response.APIResponse = contacts.FirstOrDefault().APIResponse;
            if (contacts.FirstOrDefault().UserId > 0)
            {
                response.contacts = contacts.Select(x => new ContactListResultDto { 
                    UserId = x.UserId, 
                    FirstName = x.FirstName, 
                    LastName = x.LastName, 
                    Favorite = x.Favorite, 
                    Online = x.Online, 
                    Role= x.Role,
                    Picture = x.Picture 
                }).ToList();
            }
            return response;

        }
        public async Task<FavResponse> MarkFavorite(string authToken, int userid)
        {
            return await _userRepository.MarkFavorite(authToken, userid);
        }
    }

    public interface IUserProvider
    {
        Task<UserOnlineStatusResponse> GetUserOnlineStatusAsync(UserRequest request);
        Task<UserProfileResult> GetUserProfileAsync(UserRequest request);
        Task<List<RoleDto>> GetUserRolesAsync(int userId);
        Task<List<UserLocationResult>> GetUserLocationsAsync(int userId);
        Task<List<ContactListResult>> GetContactListAsync(ContactsRequest request);
        Task<ContactListResultModel> GetGroupUsersListAsync(GroupUsersList request);
        Task<GroupResultModel> GetGroupListAsync(string authToken);
        Task<FavResponse> MarkFavorite(string authToken, int userid);
    }

}
