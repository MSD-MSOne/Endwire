using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using EndWire.API.Communications.Models;
using EndWire.Services.ServiceProviders;
using EndWire.Domain.Models;
using EndWire.Core;
using Google.Apis.CloudScheduler.v1beta1.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using EndWire.Core.Constants;

namespace EndWire.API.Controllers
{
    [Route("api/1.0")]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReminderProvider _reminderProvider;
        private readonly ILogger<TimerController> _logger;
        public RemindersController(IMapper mapper, IReminderProvider reminderProvider, ILogger<TimerController> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _reminderProvider = reminderProvider;
            _logger = logger;
        }

        [HttpGet]
        [Route("reminders")]
        [ProducesResponseType(typeof(InboxListResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRemindersAsync([FromHeader(Name = "authtoken")] string authToken, [FromQuery] string reminderType)
        {
            ReminderRequest request = new ReminderRequest { AuthToken = authToken, ReminderType = reminderType };
            try
            {
                var response = await _reminderProvider.GetRemindersAsync(request);
                if (response == null)
                {
                    //return StatusCode(StatusCodes.Status204NoContent, $"No Data Available for Remind Type: {reminderType}.");
                    return Ok(ApiResponseMessage.NoDataAvailable);
                    //return NoContent();  
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ReminderController.GetRemindersAsync AuthToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"AuthToken {request.AuthToken} GetReminders failed with message {ex.Message}.");
            }
        }

        [HttpPost]
        [Route("sendreminder")]
        public async Task<IActionResult> SendReminderAsync([FromHeader(Name = "authtoken")] string authToken, [FromBody] SendReminderRequest request)
        {
            request.AuthToken = request.AuthToken?? authToken;
            try
            {
                var response = await _reminderProvider.SendReminderAsync(request);
                if (response == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ReminderController.SendReminderAsync AuthToken = {authToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"AuthToken {authToken} SendReminderAsync failed with message {ex.Message}");
            }
        }

        [HttpPost]
        [Route("changereminderstatus")]
        public async Task<IActionResult> ChangeReminderStatusAsync([FromHeader(Name = "authtoken")] string authToken, [FromBody] ChangeReminderStatusRequest request)
        {
            request.AuthToken = request.AuthToken ?? authToken;
            try
            {
                var response = await _reminderProvider.ChangeReminderStatusAsync(request);
                if (response == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ReminderController.ChangeReminderStatusAsync AuthToken = {request.AuthToken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"AuthToken {request.AuthToken} ChangeReminderStatusAsync failed with message {{ex.Message.");
            }

        }

    }

}
