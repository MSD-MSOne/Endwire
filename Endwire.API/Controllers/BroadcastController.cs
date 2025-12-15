using EndWire.Domain.Models;
using EndWire.Domain.Models.Timer;
using EndWire.Infrastructure.Repositories.Mobile;
using EndWire.Services.ServiceProviders;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EndWire.API.Controllers
{
    [Route("api/1.0")]
    [ApiController]
    public class BroadcastController : ControllerBase
    {
        private readonly IBroadcastProvider _broadcastProvider;
        private readonly ILogger<TimerController> _logger;

        public BroadcastController(IBroadcastProvider broadcastProvider, ILogger<TimerController> logger)
        {
            _broadcastProvider = broadcastProvider;
            _logger = logger;
        }

        [HttpPost]
        [Route("broadcast")]
        public async Task<IActionResult> StartBroadcastAsync([FromHeader(Name = "authtoken")] string authToken)
        {
            BroadcastRequest request = new BroadcastRequest { AuthToken = authToken };
            try
            {
                var response = await _broadcastProvider.StartBroadcastAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in BroadcastController.StartBroadcastAsync AuthToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"AuthToken {request.AuthToken} StartBroadcastAsync failed with message {ex.Message}.");
            }

        }

        [HttpPost]
        [Route("endbroadcast")]
        public async Task<IActionResult> EndBroadcastAsync([FromHeader(Name = "authtoken")] string authToken, [FromBody] EndBroadcastRequest request)
        {
            request.AuthToken= authToken;
            try
            {
                var response = await _broadcastProvider.EndBroadcastAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in BroadcastController.EndBroadcastAsync AuthToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"AuthToken {request.AuthToken} EndBroadcastAsync failed with message {ex.Message}.");
            }

        }

    }
}
