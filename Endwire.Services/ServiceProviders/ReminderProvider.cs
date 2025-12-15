using Azure;
using Azure.Core;
using EndWire.Domain;
using EndWire.Domain.DTO;
using EndWire.Domain.Models;
using EndWire.Infrastructure.Repositories.Mobile;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EndWire.Services.ServiceProviders
{
    public class ReminderProvider : IReminderProvider
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IFcmNotification _fcmNotification;
        private readonly ILogger<ReminderProvider> _logger;
        //private readonly IHostedService _iHostedService;

        public ReminderProvider(IReminderRepository reminderRepository, IFcmNotification fcmNotification, ILogger<ReminderProvider> logger)
        {
            _reminderRepository = reminderRepository;
            _fcmNotification = fcmNotification;
            //_iHostedService = iHostedService;
            _logger = logger;
        }
        public async Task<InboxListResponse> GetRemindersAsync(ReminderRequest request)
        {
            var results = await _reminderRepository.GetRemindersAsync(request);
            InboxListResponse response = new InboxListResponse();

            if (results.IsNullOrEmpty()) // will be refactored to use mapper
            {
                return null;
            }
            response.APIResponse = results.FirstOrDefault().APIResponse;
            if (results.FirstOrDefault().ReminderId > 0)
            {
                List<InboxReminderDto> reminders = new List<InboxReminderDto>();

                //var reminderIds = String.Join(",", results.Select(x => x.ReminderId));

                //var recepients = await _reminderRepository.GetReminderRecipientsAsync(reminderIds);

                List<ReminderUserDto> recipientDtos = new List<ReminderUserDto>();
                ReminderUserDto senders = new ReminderUserDto();

                foreach (var r in results)
                {
                    reminders.Add(new InboxReminderDto
                    {
                        ReminderId = r.ReminderId,
                        ReminderType = r.ReminderType,
                        Sender = new ReminderUserDto { FirstName = r.FirstName, LastName = r.LastName, UserId = r.UserId, Picture = r.Picture },
                        MultipleRecipients = r.MultipleRecipients,
                        ResourceId = r.ResourceId,
                        Resource = r.ResourceName,
                        //Picture = r.Picture,    
                        NudgeId = r.NudgeId,
                        Status = r.NudgeStatus,
                        Text = r.Text,
                        TimeStamp = r.DateAdded.ToString(),
                    });
                }
                response.Reminders = reminders;

            }
            return response;
        }

        public async Task<ChangeReminderStatusResponse> SendReminderAsync(SendReminderRequest request)
        {

            var response =  await _reminderRepository.SendReminderAsync(request);

            if(!String.Equals(response.APIResponse, "Success"))
            {
                return response;
            }

            request.OutboxId = response.ReminderId;

            var userResponse = await _reminderRepository.FcmSendReminderAsync(request);

            try
            {
                /*if (userResponse?.FirstOrDefault() is not null) // for debugging 07/09/24 jli
                {
                    string reminderId = response.ReminderId.ToString();


                    var priority = userResponse.Where(x => x.Section.Equals("android") && x.Subsection.Equals("")).FirstOrDefault().Priority;

                    var reminder = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("")).FirstOrDefault();
                    var type = reminder?.Type;
                    var title = reminder?.Title;
                    var text = reminder?.Text;
                    var status = reminder?.Status;

                    var nudge = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("receivedBy")).FirstOrDefault();
                    var interval = nudge?.NudgeInterval;
                    var repeat = nudge?.ReminderRepeatTimes;


                    var sentBy = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("sentBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, outboxId = x.OutboxId }).FirstOrDefault();
                    var receivedBy = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("receivedBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, inboxId = x.InboxId }).ToList();
                    var sentByJson = JsonConvert.SerializeObject(sentBy);

                    var receivedByJson = JsonConvert.SerializeObject(receivedBy);

                    Console.WriteLine(sentByJson);

                    Console.WriteLine(receivedByJson);


                    // Push notification to devices
                    foreach (var ur in userResponse)
                    {
                        if (!string.IsNullOrWhiteSpace(ur.RegToken))
                        {
                            Dictionary<string, string> data = new Dictionary<string, string>();

                            data.Add("type", type);
                            data.Add("title", title);
                            data.Add("text", text);
                            data.Add("status", status);
                            data.Add("sentBy", sentByJson);
                            data.Add("receivedBy", receivedByJson);
                            data.Add("timeStamp", DateTime.Now.ToString());

                            //data.Add("userId", ur.UserId.ToString());
                            //data.Add("reminderId", reminderId);
                            //data.Add("timeStamp", DateTime.Now.ToString());
                            ///data.Add("status", ur.NudgeStatus);
                            //data.Add("status", ur.Status);

                            _logger.LogInformation($"First Sending notification to RegToken {ur.RegToken} with message {ur.Text}");
                            var notficationRepsone = await _fcmNotification.PushFcmNotificationAsync(new NotificationRequest { Title = "Send Reminder", Body = ur.Text, RegToken = ur.RegToken, Data = data, PopupNotification=false });
                        }
                    }
                }*/

                if (userResponse?.FirstOrDefault() is not null)
                {
                    int reminderId = response.ReminderId;

                    foreach (var ur in userResponse)
                    {
                        if (!string.IsNullOrWhiteSpace(ur.RegToken))
                        {
                            ur.ReminderId = reminderId;
                        }
                    }

                    TimedTask.CQ.Enqueue(userResponse);

                }

                /*System.Threading.Tasks.Task.Run(
                    () => {
                        RunTimedTask(userResponse, response.ReminderId);
                    });*/

                //_iHostedService.StartAsync(CancellationToken.None);

                /*System.Threading.Tasks.Task.Run(
                    () => {
                        RunTimedTask(userResponse);
                    });*/

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ReminderProvider.ChangeReminderStatusAsync with message {ex.Message}");
                throw;
            }

            return response;    
        }

        public async Task<ChangeReminderStatusResponse> ChangeReminderStatusAsync(ChangeReminderStatusRequest request)
        {
            var response =  await _reminderRepository.ChangeReminderStatusAsync(request);
            
            var userResponse = await _reminderRepository.FcmNudgeStatusChangeAsync(request);


            try
            {
                //if (userResponse?.FirstOrDefault()?.RegToken is not null)
                if (userResponse?.FirstOrDefault() is not null)
                {

                    string reminderId = response.ReminderId.ToString();
                    // Push notification to devices
                    foreach (var ur in userResponse)
                    {
                        /*if (!string.Equals(ur.NudgeStatus, "Remove"))
                        {
                            Dictionary<string, string> data = new Dictionary<string, string>();
                            data.Add("userId", ur.Userid.ToString());
                            data.Add("reminderId", reminderId);
                            data.Add("timeStamp", ur.TimeStamp);
                            data.Add("status", ur.NudgeStatus);

                            var notficationRepsone = await _fcmNotification.PushFcmNotificationAsync(new NotificationRequest { Title = "nudge status changed", Body = ur.NudgeStatus, RegToken = ur.RegToken, Data= data});
                        }*/


                        /*var priority = userResponse.Where(x => x.Section.Equals("android") && x.Subsection.Equals("")).FirstOrDefault().Priority;

                        var reminder = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("")).FirstOrDefault();
                        var type = reminder?.Type;
                        var title = reminder?.Title;
                        var text = reminder?.Text;
                        var status = reminder?.Status;

                        var nudge = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("receivedBy")).FirstOrDefault();
                        var interval = nudge?.NudgeInterval;
                        var repeat = nudge?.ReminderRepeatTimes;


                        var sentBy = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("sentBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, outboxId = x.OutboxId }).FirstOrDefault();

                        var CanceledBy = userResponse.Where(x => x.Section.Equals("data") &&  x.Subsection.Equals("CanceledBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, outboxId = x.OutboxId }).FirstOrDefault();
                        var clearedBy = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("clearedBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, inboxId = x.InboxId }).FirstOrDefault();
                        var confirmedBy = userResponse.Where(x => x.Section.Equals("data") &&  x.Subsection.Equals("confirmedBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, inboxId = x.InboxId }).FirstOrDefault();

                        var action = CanceledBy != null ? "CanceledBy" : clearedBy != null ? "clearedBy" : "confirmedBy"; 

                        var sendFCMto = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("sendFCMto")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, outboxId = x.OutboxId }).FirstOrDefault();

                        var receivedBy = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("receivedBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, inboxId = x.InboxId }).ToList();

                        var sentByJson = JsonConvert.SerializeObject(sentBy);
                        var receivedByJson = JsonConvert.SerializeObject(receivedBy);
                        //var confirmedByJson = JsonConvert.SerializeObject(confirmedBy); 
                        var actionJson = CanceledBy != null ? JsonConvert.SerializeObject(CanceledBy) : clearedBy != null ? JsonConvert.SerializeObject(clearedBy) : JsonConvert.SerializeObject(confirmedBy);*/


                        if (!string.Equals(ur.Status, "Remove"))
                        {
                            if (!string.IsNullOrWhiteSpace(ur.RegToken) && ur.Subsection.Equals("sendFCMto"))
                            {


                                var priority = userResponse.Where(x => x.Section.Equals("android") && x.Subsection.Equals("")).FirstOrDefault().Priority;

                                var reminder = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("")).FirstOrDefault();
                                var type = reminder?.Type;
                                var title = reminder?.Title;
                                var text = reminder?.Text;
                                var status = reminder?.Status;

                                var nudge = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("receivedBy")).FirstOrDefault();
                                var interval = nudge?.NudgeInterval;
                                var repeat = nudge?.ReminderRepeatTimes;


                                var sentBy = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("sentBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, outboxId = x.OutboxId }).FirstOrDefault();

                                var CanceledBy = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("CanceledBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, outboxId = x.OutboxId }).FirstOrDefault();
                                var clearedBy = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("clearedBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, inboxId = x.InboxId }).FirstOrDefault();
                                var confirmedBy = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("confirmedBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, inboxId = x.InboxId }).FirstOrDefault();

                                var action = CanceledBy != null ? "canceledBy" : clearedBy != null ? "clearedBy" : "confirmedBy";

                                var sendFCMto = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("sendFCMto")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, outboxId = x.OutboxId }).FirstOrDefault();

                                var receivedBy = userResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("receivedBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName, inboxId = x.InboxId }).ToList();

                                var sentByJson = JsonConvert.SerializeObject(sentBy);
                                var receivedByJson = JsonConvert.SerializeObject(receivedBy);
                                //var confirmedByJson = JsonConvert.SerializeObject(confirmedBy); 
                                var actionJson = CanceledBy != null ? JsonConvert.SerializeObject(CanceledBy) : clearedBy != null ? JsonConvert.SerializeObject(clearedBy) : JsonConvert.SerializeObject(confirmedBy);


                                Dictionary<string, string> data = new Dictionary<string, string>();
                               
                                data.Add("title", title);
                                data.Add("text", text);
                                data.Add("status", status);
                                data.Add("timeStamp", DateTime.Now.ToString());
                                data.Add("type", type);
                                data.Add("receivedBy", receivedByJson);
                                data.Add("sentBy", sentByJson);
                                //data.Add("confirmedBy", confirmedByJson);
                                data.Add(action, actionJson);

                                var notficationRepsone = await _fcmNotification.PushFcmNotificationAsync(new NotificationRequest { Title = title, Body = ur.Text, RegToken = ur.RegToken, Data = data, PopupNotification = false });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ReminderProvider.ChangeReminderStatusAsync with message {ex.Message}");
                throw;
            }
            return response;
        }

        private async System.Threading.Tasks.Task<int> RunTimedTask(List<ChangeReminderStatusFcmResponse> response, int reminderId)
        {
            await System.Threading.Tasks.Task.Delay((int)response?.FirstOrDefault().NudgeInterval*1000);

            //var userResponse = await _reminderRepository.FcmSendReminderAsync(request);

            if (response?.FirstOrDefault() is not null)
            {
                List<int> removed = new List<int>();
                for (int i = 0; i < response?.FirstOrDefault().ReminderRepeatTimes; i++)
                {
                    foreach (var ur in response)
                    {
                        if (!removed.Contains(ur.UserId))
                        {
                            var statusRequest = new UserNudgeStatusRequest { UserId = ur.UserId, ReminderId = reminderId };
                            var result = await _reminderRepository.GetUserNudgeStatusAsync(statusRequest);
                            if (string.Equals(result.NudgeStatus, "Cleared"))
                            {
                                removed.Add(ur.UserId);
                                continue;
                            }
                            var notficationRepsone = await _fcmNotification.PushFcmNotificationAsync(new NotificationRequest { Title = "Send Reminder", Body = ur.Text, RegToken = ur.RegToken });
                            Console.WriteLine($"For ReminderId {reminderId} in Loop {i+1} for user {ur.UserId} - Sending notification to RegToken {ur.RegToken} with message {ur.Text}");
                            _logger.LogInformation($"For ReminderId {reminderId} in Loop {i + 1} or user {ur.UserId} - Sending notification to RegToken {ur.RegToken} with message {ur.Text}");
                        }
                    }
                    await System.Threading.Tasks.Task.Delay((int)response?.FirstOrDefault().NudgeInterval * 1000);
                }
            }

            return await System.Threading.Tasks.Task.FromResult(0);

            /*if (response?.FirstOrDefault() is not null)
            {
                for (int i = 0; i < response?.FirstOrDefault().ReminderRepeatTimes; i++)
                {
                    foreach (var ur in response)
                    {
                        var statusRequest = new UserNudgeStatusRequest { UserId = ur.UserId , ReminderId = reminderId };
                        var result = await _reminderRepository.GetUserNudgeStatusAsync(statusRequest);
                        if (string.Equals(result.NudgeStatus, "Cleared"))
                        {

                        }
                        var notficationRepsone = await _fcmNotification.PushFcmNotificationAsync(new NotificationRequest { Title = "Send Reminder", Body = ur.FCMMessage, RegToken = ur.RegToken });
                        Console.WriteLine($"in Loop - Sending notification to RegToken {ur.RegToken} with message {ur.FCMMessage}");
                        _logger.LogInformation($"in Loop - Sending notification to RegToken {ur.RegToken} with message {ur.FCMMessage}");
                    }
                    await System.Threading.Tasks.Task.Delay((int)response?.FirstOrDefault().NudgeInterval * 1000);
                }
            }*/
        }

    }

    public interface IReminderProvider
    {
        Task<InboxListResponse> GetRemindersAsync(ReminderRequest request);
        Task<ChangeReminderStatusResponse> SendReminderAsync(SendReminderRequest request);
        Task<ChangeReminderStatusResponse> ChangeReminderStatusAsync(ChangeReminderStatusRequest request);
        
    }

}
