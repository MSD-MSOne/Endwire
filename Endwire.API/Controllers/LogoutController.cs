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

namespace EndWire.API.Controllers
{
    [Route("api/1.0")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoginProvider _loginProvider;
        private readonly ILogger<LogoutController> _logger;
        public LogoutController(IMapper mapper, ILoginProvider loginProvider, ILogger<LogoutController> logger) { 
            _mapper = mapper??throw new ArgumentNullException(nameof(mapper));
            _loginProvider = loginProvider;
            _logger = logger;

        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync([FromHeader(Name = "authtoken")] string authToken)
        {
            LogoutRequest request = new LogoutRequest { AuthToken = authToken };
            try
            {
                var response = await _loginProvider.LogoutAsync(request);
                //if(response == null || response.Status.Equals("No active session found"))
                //{
                //    return NotFound("No active session found");
                //}
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in LoginController.LoginAsync AuthToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"AuthToken {request.AuthToken} GetRtcTokenAsync failed with message {ex.Message}.");
            }
        }
    }
}
