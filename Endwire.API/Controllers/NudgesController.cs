using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using EndWire.API.Communications.Models;
using EndWire.Services.ServiceProviders;
using EndWire.Domain.Models;
using EndWire.Domain;
using EndWire.Domain.DTO;
using EndWire.Core;
using EndWire.Core.Constants;

namespace EndWire.API.Controllers
{
    [Route("api/1.0")]
    [ApiController]
    public class NudgesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INudgeProvider _nudgeProvider;
        private readonly ILogger<TimerController> _logger;
        public NudgesController(IMapper mapper, INudgeProvider nudgeProvider, ILogger<TimerController> logger) { 
            _mapper = mapper??throw new ArgumentNullException(nameof(mapper));
            _nudgeProvider = nudgeProvider;
            _logger = logger;   
        }

        [HttpGet]
        [Route("nudges")]
        public async Task<IActionResult> GetNudgeListAsync([FromHeader(Name = "authtoken")] string authToken)
        {
            NudgeRequest request = new NudgeRequest { AuthToken = authToken };
            try
            {
                var response = await _nudgeProvider.GetNudgesForRoleAsync(request);
                if (response == null)
                {
                    //return NoContent();
                    return Ok(ApiResponseMessage.NoDataAvailable);
                }
                List<NudgeDto> results = new List<NudgeDto>();
                foreach (var r in response)
                {
                    results.Add(new NudgeDto { NudgeId = r.NudgeId, Nudge = r.Nudge });
                }

                NudgeResponseModel responseModel = new NudgeResponseModel { APIResponse = response.FirstOrDefault().APIResponse };
                
                //a valid token is supplied 
                if (response.FirstOrDefault().NudgeId > 0) 
                {
                    responseModel.Nudges = results;
                }
                
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in NudgeController.GetOutboxListAsync AuthToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"AuthToken {request.AuthToken} GetOutboxListAsync failed with message {ex.Message}.");
            }
        }

        [HttpGet]
        [Route("nudgeoutbox")]
        public async Task<IActionResult> GetOutboxListAsync([FromHeader(Name = "authtoken")] string authToken)
        {
            NudgeRequest request = new NudgeRequest { AuthToken = authToken };
            try
            {
                var response = await _nudgeProvider.GetOutboxListAsync(request);
                if (response == null)
                {
                    //return StatusCode(StatusCodes.Status204NoContent);
                    return Ok(ApiResponseMessage.NoDataAvailable);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in LocationController.GetOutboxListAsync AuthToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"AuthToken {request.AuthToken} GetOutboxListAsync failed with message {ex.Message}.");
            }
        }

     }

}
