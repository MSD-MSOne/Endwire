using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using EndWire.API.Communications.Models;
using EndWire.Services.ServiceProviders;
using EndWire.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using EndWire.Domain;
using AgoraNET;
using EndWire.Core.Helpers;
using EndWire.Core;
using EndWire.Infrastructure.Repositories.Mobile;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
//using StreamChat.Clients;

namespace EndWire.API.Controllers
{

    [Route("api/1.0")]
    [ApiController]
    public class RtcCallController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoginProvider _loginProvider;
        private readonly ILogger<RtcCallController> _logger;
        private readonly IRtcServiceProvider _rtcServiceProvider;
        private readonly IFcmNotification _fcmNotification;

        //private string _appId = "c5ada4e280e249f185432cc3498ad906"; // af87b56cb96a4d539f0171d842e3e7e2
        //private string _appCertificate = "6400b96609fe4d8997a02d1f243f53b0"; // 5504df3cf59343aea29432aff29893d2
        private string _channelName = "endwire-chat"; // "7d72365eb983485397e3e3f9d460bdda";
        //private string _uid = "ff5cc111825d47768acd05ffe49d64d5"; //"2882341273";
        //private uint _ts = 1111111;
        //private uint _salt = 1;
        //private uint _expiredTs = 1446455471;

        public RtcCallController(IMapper mapper, ILoginProvider loginProvider, ILogger<RtcCallController> logger, IRtcServiceProvider rtcServiceProvider, IFcmNotification fcmNotification)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _loginProvider = loginProvider;
            _logger = logger;
            _rtcServiceProvider = rtcServiceProvider;
            _fcmNotification = fcmNotification; 
        }

        [HttpPost]
        [Route("callrequest")]
        public async Task<IActionResult> RtcCallRequestAsync([FromHeader(Name = "authtoken")] string authToken, [FromBody] RtcCallRequest request)
        {
            request.AuthToken = authToken;  
            //string currentRegToken = "";
            try
            {
                _channelName = RandomStringHelper.RandomString(20);
                if (string.IsNullOrEmpty(request.ChannelId) || request.ChannelId.Equals("string", StringComparison.Ordinal)) {
                    request.ChannelId = RandomStringHelper.RandomString(20);
                }

                var userResponse = await _rtcServiceProvider.GetStartCallAsync(request);
                var response = new RtcCallResponse { APIResponse = userResponse?.FirstOrDefault()?.APIResponse ?? "BadToken" };
               // var response = new RtcCallResponseModel { APIResponse = userResponse?.FirstOrDefault()?.APIResponse ?? "BadToken" };
                //string callerToken = string.Empty;
                //int initiatorId = 0;

                //initiatorId = userResponse.Where(u => u.Caller.Equals("1")).Select(u => u.UserId).FirstOrDefault();
        /*
                if (userResponse?.FirstOrDefault()?.Section is null)
                {
                    return BadRequest(response);
                }
                */ 
                
                List<RtcCallUser> users = new List<RtcCallUser>();
                
                foreach (var u in userResponse)
                {
                    if(u.UserId != 0  && !users.Any(ur => ur.UserId==u.UserId))
                    {
                        users.Add(new RtcCallUser { UserId = u.UserId, CallToken = u.RegToken, Status=u.Status });
                    }

                   //var currentRegToken = u.RegToken;
                   // var callToken = _rtcServiceProvider.GetRtcAccessToken(request.ChannelId);
                   // if(String.Equals(u., "1"))
                   // {
                   //     callerToken = callToken;    
                   // }
                   // //users.Add(new RtcCallUser { UserId = u.UserId, CallToken = callToken });

                   // if (!String.Equals(u.Caller, "1"))
                   // {
                   //     var notificationRequest = new NotificationRequest
                   //     {
                   //         Title = "Start Call Notification",
                   //         Body = u.FCMMessage,
                   //         RegToken = u.RegToken,
                   //     };
                   //     notificationRequest.Data.Add("userid", initiatorId.ToString());
                   //     notificationRequest.Data.Add("channelid", request.ChannelId);
                   //     notificationRequest.Data.Add("calltoken", callToken);
                   //     var notficationRepsone = await _fcmNotification.PushFcmNotificationAsync(notificationRequest);
                   // }
                }
                response.Users = users;
                response.ChannelId = request.ChannelId;
                //response.CallToken = String.IsNullOrEmpty(callerToken) ? _rtcServiceProvider.GetRtcAccessToken(request.ChannelId): callerToken;
                response.CallToken = _rtcServiceProvider.GetRtcAccessToken(request.ChannelId);
                response.APIResponse = "Success";

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RtcCallController.RtcCallRequestAsync AuthToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"RtcCallRequest failed with message: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("acceptcall")]
        public async Task<IActionResult> AcceptCallAsync([FromHeader(Name = "authtoken")] string authToken, [FromBody] AcceptCallRequest request)
        {
            request.AuthToken = authToken;  
            try
            {
                var response = await _rtcServiceProvider.AcceptCallAsync(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RtcCallController.LoginAsync AuthToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"AuthToken {request.AuthToken} AcceptCallAsync failed with message {ex.Message}");
            }

        }

        [HttpPost]
        [Route("exitcall")]
        public async Task<IActionResult> ExitCallAsync([FromHeader(Name = "authtoken")] string authToken, [FromBody] ExitCallRequest request)
        {
            request.AuthToken = authToken;
            try
            {
                var response = await _rtcServiceProvider.ExitCallAsync(request);

                if (response.APIResponse.Equals("Invalid or Expired Authorization Token"))
                {
                    return Ok(response);
                }

                var userResponse = await _rtcServiceProvider.FcmExitCallAsync(request);

                 if (userResponse?.FirstOrDefault() is not null)
                {
                    foreach (var u in userResponse)
                    {
                        var callToken = _rtcServiceProvider.GetRtcAccessToken(request.ChannelId);

                        if (u.RegToken is not null)
                        {
                            var notificationRequest = new NotificationRequest
                            {
                                Title = "Call Exited",
                                Body = $"{u.Name} ({u.UserId}) exited the call.",
                                RegToken = u.RegToken,
                            };
                            notificationRequest.Data.Add("userid", u.UserId.ToString());
                            notificationRequest.Data.Add("channelid", request.ChannelId);
                            notificationRequest.Data.Add("calltoken", callToken);

                            var notficationRepsone = await _fcmNotification.PushFcmNotificationAsync(notificationRequest);
                        }
                    }
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RtcCallController.ExitCallAsync AuthToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"AuthToken {request.AuthToken} ExitCallAsync failed with message {ex.Message}");
            }

        }

        [HttpPost]
        [Route("endcall")]
        public async Task<IActionResult> EndRtcCallAsync([FromHeader(Name = "authtoken")] string authToken, [FromBody] EndCallRequest request)
        {
            request.AuthToken = authToken;
            try
            {
                var response = await _rtcServiceProvider.EndRtcCallAsync(request);

                if (response.APIResponse.Equals("Invalid or Expired Authorization Token"))
                {
                    return Ok(response);
                }

                ExitCallRequest exitRequest = new ExitCallRequest 
                { 
                    AuthToken = request.AuthToken,
                    ChannelId = request.ChannelId
                };

                var userResponse = await _rtcServiceProvider.FcmExitCallAsync(exitRequest);

                if (userResponse?.FirstOrDefault() is not null)
                {
                    foreach (var u in userResponse)
                    {
                        var callToken = _rtcServiceProvider.GetRtcAccessToken(request.ChannelId);

                        if (u.RegToken is not null)
                        {
                            var notificationRequest = new NotificationRequest
                            {
                                Title = "Call Ended",
                                Body = $"{u.Name} ({u.UserId}) ended the call.",
                                RegToken = u.RegToken,
                            };
                            notificationRequest.Data.Add("userid", u.UserId.ToString());
                            notificationRequest.Data.Add("channelid", request.ChannelId);
                            notificationRequest.Data.Add("calltoken", callToken);

                            var notficationRepsone = await _fcmNotification.PushFcmNotificationAsync(notificationRequest);
                        }
                    }
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in LoginController.LoginAsync AuthToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"AuthToken {request.AuthToken} RtcCallRequestAsync failed. with message {ex.Message}");
            }

        }

    }
}
