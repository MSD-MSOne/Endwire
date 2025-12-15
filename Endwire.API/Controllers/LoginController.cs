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

namespace EndWire.API.Controllers
{

    [Route("api/1.0")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoginProvider _loginProvider;
        private readonly ILogger<TimerController> _logger;
        public LoginController(IMapper mapper, ILoginProvider loginProvider, ILogger<TimerController> logger) { 
            _mapper = mapper??throw new ArgumentNullException(nameof(mapper));
            _loginProvider = loginProvider;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _loginProvider.LoginAsync(request);
                if (response == null)
                {
                    return NoContent();
                }
                if (!response.Error.IsNullOrEmpty())
                {
                    return Ok(new LoginErrorResponse { Error = response.Error });
                }

                var loginResponseModel = _mapper.Map<LoginResponseModel>(response);

                return Ok(loginResponseModel);
            }
            catch(Exception ex) 
            {
                _logger.LogError($"Error in LoginController.LoginAsync UserName = {request.UserName} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"UserName {request.UserName} login failed message {ex.Message}.");
            }
        }
 
    }
}
