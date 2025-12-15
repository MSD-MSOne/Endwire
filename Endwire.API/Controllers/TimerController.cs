using Azure;
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
    public class TimerController : ControllerBase
    {
        private readonly ITimerRepsitory _timerRepsitory;
        private readonly IFcmNotification _fcmNotification;
        private readonly ILogger<TimerController> _logger;

        public TimerController(ITimerRepsitory timerRepsitory, IFcmNotification fcmNotification, ILogger<TimerController> logger)
        {
            _timerRepsitory = timerRepsitory;
            _logger = logger;
            _fcmNotification = fcmNotification; 
        }

        [HttpPost]
        [Route("settime")]
        public async Task<IActionResult> SetTimerAsync([FromHeader(Name = "authtoken")] string authToken, [FromBody] SetTimerRequest request)
        {
            try
            {
                var result = await _timerRepsitory.SetTimerAsync(authToken, request.TaskId, request.ResourceId);

                if (result.IsNullOrEmpty())
                {
                    return NotFound("No Data Found");
                }

                System.Threading.Tasks.Task.Run(async ()=>
                {
                    foreach (var r in result)
                    {
                        await System.Threading.Tasks.Task.Delay(int.Parse(r.TaskInterval) * 1000);
                        await _fcmNotification.PushFcmNotificationAsync(new NotificationRequest { Title = "Timer Done", Body = r.FCM_Message, RegToken = r.RegToken });
                    }
                }
                );

                SetTimerResponseModel model = new SetTimerResponseModel()
                {
                    status = result.FirstOrDefault().Response,
                    APIResponse = result.FirstOrDefault().APIResponse

                };

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in TimerController.SetTimerAsync ResourceId = {request.ResourceId} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"ResourceId {request.ResourceId} SetTimerAsync failed with message {ex.Message}.");
            }

        }

        [HttpGet]
        [Route("timertasks")]
        public async Task<IActionResult> GetTimerTaskListAsync([FromHeader(Name = "authtoken")] string authToken)
        {
            AuthToken request = new AuthToken { authtoken = authToken };
            try
            {
                var response = await _timerRepsitory.TimerTaskListAsync(request.authtoken);
                TimerTaskResponseModel responseModel = new TimerTaskResponseModel(); //_mapper.Map(response);
                responseModel.APIResponse = response?.FirstOrDefault()?.APIResponse;

                if (response?.FirstOrDefault().TaskId > 0)
                {
                    responseModel.Tasks = response?.Select(x => new TimerTask { TaskId = x.TaskId, Name = x.Name, Time = x.Time }).ToList();
                }

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in TimerController.GetTimerTaskListAsync authToken = {request.authtoken} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"authToken {request.authtoken} GetTimerTaskListAsync failed with message {ex.Message}.");
            }

        }

        private async Task<string> PushFcmNotificationAsync(string chatToken, string text)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = await GoogleCredential.FromFileAsync("private-key.json", new CancellationToken())
            });
            var message = new Message()
            {
                Data = new Dictionary<string, string> { { "myData", "extra text" } },
                //Token = chatToken,
                Topic = "dentist",
                Notification = new Notification() { Title = "This is a test", Body = text }

            };
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }

    }
}
