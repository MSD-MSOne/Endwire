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

namespace EndWire.API.Controllers
{

    [Route("api/1.0")]
    [ApiController]
    public class RtcTokenController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoginProvider _loginProvider;
        private readonly ILogger <RtcTokenController> _logger;

        private string _appId = "c5ada4e280e249f185432cc3498ad906"; // af87b56cb96a4d539f0171d842e3e7e2
        private string _appCertificate = "6400b96609fe4d8997a02d1f243f53b0"; // 5504df3cf59343aea29432aff29893d2
        private string _channelName = "endwire-test"; // "7d72365eb983485397e3e3f9d460bdda";
        private string _uid = "ff5cc111825d47768acd05ffe49d64d5"; //"2882341273";
        private  uint _ts = 1111111;
        private  uint _salt = 1;
        private  uint _expiredTs = 1446455471;

        public RtcTokenController(IMapper mapper, ILoginProvider loginProvider, ILogger<RtcTokenController> logger) { 
            _mapper = mapper??throw new ArgumentNullException(nameof(mapper));
            _loginProvider = loginProvider;
            _logger = logger;   
        }

        [HttpPost]
        [Route("rtctoken")]
        //[ProceduresResponseType((int)HttpStatusCode.OK)]
        //[ProceduresResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetRtcTokenAsync([FromBody] RtcTokenRequest request)
        {
            try
            {
                // _channelName = RandomStringHelper.RandomString(20); // commented 07/02/2024
                _channelName = request.ChannelName ?? RandomStringHelper.RandomString(20); // 07/02/2024

                AccessToken token =
                    new AccessToken(_appId, _appCertificate, _channelName, _uid);
                token.message.ts = _ts;
                token.message.salt = _salt;
                token.AddPrivilege(Privileges.JoinChannel, _expiredTs);

                string result = token.Build();

                if (result == null)
                {
                    return NoContent();
                }

                var response = new RtcTokenResponse
                {
                    Token = result
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in LoginController.LoginAsync AppId = {request.AppId} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"AppId {request.AppId} GetRtcTokenAsync failed {ex.Message}.");
            }

        }
 
    }
}
