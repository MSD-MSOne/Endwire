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
using EndWire.Core.Constants;

namespace EndWire.API.Controllers
{
    [Route("api/1.0")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IResourceProvider _resourceProvider;
        private readonly ILogger<TimerController> _logger;
        public ResourcesController(IMapper mapper, IResourceProvider resourceProvider, ILogger<TimerController> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _resourceProvider = resourceProvider;
            _logger = logger;
        }

        [HttpGet]
        [Route("resources/{nudgeId:int?}")]
        public async Task<IActionResult> GetResourcesAsync([FromHeader(Name = "authtoken")] string authToken, [FromRoute] int? nudgeId)
        {
            ResourceRequest request = new ResourceRequest { AuthToken = authToken, NudgeId = nudgeId };
            try
            {
                var response = await _resourceProvider.GetResourcesAsync(request);
                if (response.Resources.IsNullOrEmpty())
                {
                    //return StatusCode(StatusCodes.Status204NoContent);
                    return Ok(ApiResponseMessage.NoDataAvailable);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ResourceController.GetResourcesAsync AuthToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"AuthToken {request.AuthToken} GetResources failed with message {ex.Message}.");
            }
        }

        [HttpPost]
        [Route("addresource")]
        public async Task<IActionResult> AddResourceAsync([FromBody] AddResourceRequest request)
        {
            try
            {
                var response = await _resourceProvider.AddResourceAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in LoginController.LoginAsync message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Resource Name {request.ResourceName} AddResource failed {ex.Message}.");
            }
        }

    }

}
