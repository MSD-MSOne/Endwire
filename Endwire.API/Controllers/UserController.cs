using AutoMapper;
using Azure.Core;
using EndWire.Core;
using EndWire.Core.Constants;
using EndWire.Domain.Models;
using EndWire.Domain.Models.Login;
using EndWire.Services.ServiceProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EndWire.API.Controllers
{

    [ApiController]
    [Route("api/1.0")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserProvider _userProvider;
        private readonly ILogger<UserController> _logger;

        public UserController(IMapper mapper, IUserProvider userProvider, ILogger<UserController> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userProvider = userProvider;
            _logger = logger;
        }

        [HttpGet]
        [Route("checkuserstatus/{userId}")]
        public async Task<IActionResult> GetUserOnlineStatusAsync([FromHeader(Name = "authtoken")] string authToken, [FromRoute] int userId)
        {
            UserRequest request = new UserRequest { AuthToken = authToken, UserId = userId };
            try
            {
                _logger.LogInformation("Executing {Action} {Parameters}", nameof(GetUserOnlineStatusAsync), request.UserId);
                _logger.LogDebug("Executing {Action} {Parameters}", nameof(GetUserOnlineStatusAsync), request.UserId);

                var response = await _userProvider.GetUserOnlineStatusAsync(request);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex) {
                _logger.LogError($"Error in LoginController.GetUserProfileAsync UserId = {request.UserId} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"userId {request.UserId} GetUserOnlineStatusAsync failed with message {ex.Message}.");
            }
        }

        [HttpGet]
        [Route("profile/{userId:int?}")]
        //public async Task<IActionResult> GetUserProfileAsync([FromHeader(Name = "authtoken")] string authToken, [FromQuery] int userId)
        public async Task<IActionResult> GetUserProfileAsync([FromHeader(Name = "authtoken")] string authToken, [FromRoute] int? userId)
        {
            UserRequest request = new UserRequest { AuthToken = authToken, UserId = userId };
            try
            {
                var userProfileResult = await _userProvider.GetUserProfileAsync(request);
                if (userProfileResult == null || !userProfileResult.UserId.HasValue || userProfileResult.UserId == 0 )
                {
                    return Ok(new UserProfileResponse { APIResponse = userProfileResult?.APIResponse });
                }
                var roles = await _userProvider.GetUserRolesAsync(userProfileResult.UserId.Value);
                var userLocations = await _userProvider.GetUserLocationsAsync(userProfileResult.UserId.Value);

                UserProfileResponse response = new UserProfileResponse
                {
                    UserId = userProfileResult.UserId.Value,
                    FirstName = userProfileResult.FirstName,
                    LastName = userProfileResult.LastName,
                    Picture = userProfileResult.Picture,
                    Online =  userProfileResult.Online,   
                    Roles = roles,
                    Locations = _mapper.Map<List<Loc>>(userLocations),
                    APIResponse = userProfileResult.APIResponse
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in LoginController.GetUserProfileAsync UserId = {request.UserId} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"userId {request.UserId} GetUserProfileAsync failed with message {ex.Message}.");
            }

        }
        [HttpGet]
        [Route("contacts/{nudgeId:int?}")]
        public async Task<IActionResult> GetContactListAsync([FromHeader(Name = "authtoken")] string authToken, [FromRoute] int? nudgeId)
        {
            ContactsRequest request = new ContactsRequest { AuthToken = authToken, NudgeId = nudgeId };
            var result = await _userProvider.GetContactListAsync(request);
            try
            {
                if (result.IsNullOrEmpty())
                {
                    //return NoContent();
                    //return StatusCode(StatusCodes.Status204NoContent);
                    return Ok(ApiResponseMessage.NoDataAvailable);
                }
                //var groupResult = await _userProvider.GetGroupsAsync(request.AuthToken);

                List<Domain.UserDto> users = result.Select(x =>
                new Domain.UserDto
                {
                    UserId = x.UserId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Picture = x.Picture,
                    Favorite = x.Favorite,
                    Online = x.Online,
                    Role = x.Role,  
                    //Groups = groupResult
                }).ToList();

                var response = new ContactsResponse
                {
                    Users = users.Count==1 && users.FirstOrDefault()?.UserId==0?new List<Domain.UserDto>():users,
                    APIResponse = result.FirstOrDefault().APIResponse
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in LoginController.LoginAsync NudgeId = {request.NudgeId} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"NudgeId {request.NudgeId} GetContactListAsync failed with message {ex.Message}.");
            }
        }

        [HttpPost]
        [Route("markfav")]
        public async Task<IActionResult> MarkfavAsync([FromHeader(Name = "authtoken")] string authToken, [FromBody] MarkfavRequest request)
        {
            request.AuthToken = authToken;  
            try
            {
                var response = await _userProvider.MarkFavorite(request.AuthToken, request.UserId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UserController.MarkfavAsync authToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"authToken {request.AuthToken} GetGroupListAsync failed with message {ex.Message}.");
            }
        }

        [HttpGet]
        [Route("grouplist")]
        public async Task<IActionResult> GetGroupListAsync([FromHeader(Name = "authtoken")] string authToken)
        {
            AuthToken request = new AuthToken { authtoken = authToken };
            try
            {
                var groupResult = await _userProvider.GetGroupListAsync(request.authtoken);
                if (groupResult == null)
                {
                    //return NoContent();
                    //return StatusCode(StatusCodes.Status204NoContent);
                    return Ok(ApiResponseMessage.NoDataAvailable);
                }
               
                return Ok(groupResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UserController.GetGroupListAsync authToken = {request.authtoken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"authToken {request.authtoken} GetGroupListAsync failed with message {ex.Message}.");
            }
        }

        [HttpGet]
        [Route("getgroup/{groupId}")]
        public async Task<IActionResult> GetGroupUserListAsync([FromHeader(Name = "authtoken")] string authToken, [FromRoute] int groupId)
        {
            GroupUsersList request = new GroupUsersList { AuthToken = authToken, GroupId = groupId };
            try
            {
                var result = await _userProvider.GetGroupUsersListAsync(request);
                if (result == null)
                {
                    //return NoContent();
                    //return StatusCode(StatusCodes.Status204NoContent);
                    return Ok(ApiResponseMessage.NoDataAvailable);
                }


                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UserController.GetTimerTaskListAsync authToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"authToken {request.AuthToken} GetGroupUserListAsync failed with message {ex.Message}.");
            }
        }
    }
}

