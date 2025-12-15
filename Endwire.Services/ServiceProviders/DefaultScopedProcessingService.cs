using EndWire.Domain.Models;
using EndWire.Infrastructure.Repositories.Mobile;
using EndWire.Services.ServiceProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace App.ScopedService;

public sealed class DefaultScopedProcessingService : IScopedProcessingService
{
    private int _executionCount;
    private readonly ILogger<DefaultScopedProcessingService> _logger;
    private readonly IReminderRepository _reminderRepository;
    private readonly IFcmNotification _fcmNotification;

    public DefaultScopedProcessingService(
        IReminderRepository reminderRepository,
        IFcmNotification fcmNotification,
        ILogger<DefaultScopedProcessingService> logger) =>
        (_reminderRepository, _fcmNotification, _logger) = (reminderRepository, fcmNotification, logger);

    public async System.Threading.Tasks.Task DoWorkAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await System.Threading.Tasks.Task.Delay(1000);
            TimedTask.CQ.TryDequeue(out var response);
            if (response is not null)
            {
                System.Threading.Tasks.Task.Run(async () => {

                    _logger.LogInformation("start ... ");

                    if (response?.FirstOrDefault() is not null)
                    {
                        List<int> removed = new List<int>();
                        bool confirmed = false;


                        var reminder = response.Where(x => x.Section.Equals("data") && x.Subsection.Equals("")).FirstOrDefault();
                        var type = reminder?.Type;
                        var title = reminder?.Title;
                        var text = reminder?.Text;
                        var status = reminder?.Status;

                        //var nudge = response.Where(x => x.Section.Equals("data") && x.Subsection.Equals("receivedBy")).FirstOrDefault();
                        var nudge = response.Where(x => x.Section.Equals("data") && x.Subsection.Equals("sendFCMto")).FirstOrDefault();
                        int interval = nudge?.NudgeInterval??0;
                        int repeat = nudge?.ReminderRepeatTimes??0;

                        var sentBy = response.Where(x => x.Section.Equals("data") && x.Subsection.Equals("sentBy")).Select(x => new { UserId = x.UserId, FirstName = x.FirstName, LastName = x.LastName, OutboxId = x.OutboxId }).FirstOrDefault();
                        var receivedBy = response.Where(x => x.Section.Equals("data") && x.Subsection.Equals("receivedBy")).Select(x => new { UserId = x.UserId, FirstName = x.FirstName, LastName = x.LastName, InboxId = x.InboxId }).ToList();
                        var sentByJson = JsonConvert.SerializeObject(sentBy);
                        var receivedByJson = JsonConvert.SerializeObject(receivedBy);



                        for (int i = 0; i < repeat; i++)
                        //for (int i = 0; i < response?.FirstOrDefault().ReminderRepeatTimes; i++)
                        {
                            foreach (var ur in response)
                            {

                                if (!string.IsNullOrWhiteSpace(ur.RegToken))
                                {  // jli


                                    if (!removed.Contains(ur.UserId))
                                    {
                                        var statusRequest = new UserNudgeStatusRequest { UserId = ur.UserId, ReminderId = ur.ReminderId };
                                        var result = await _reminderRepository.GetUserNudgeStatusAsync(statusRequest);
                                        if (string.Equals(result.NudgeStatus, "Cleared"))
                                        {
                                            removed.Add(ur.UserId);
                                            continue;
                                        }
                                        if (string.Equals(result.NudgeStatus, "Confirmed") || string.Equals(result.NudgeStatus, "Canceled"))
                                        {
                                            removed.Add(ur.UserId);
                                            confirmed = true;
                                            // send notification to others, then stop main loop
                                            continue;
                                        }

                                        Dictionary<string, string> data = new Dictionary<string, string>();

                                        //data.Add("userId", ur.UserId.ToString());  // jli
                                        //data.Add("reminderId", ur.ReminderId.ToString());
                                        //data.Add("timeStamp", DateTime.Now.ToString());
                                        //data.Add("status", ur.Status);


                                        data.Add("type", type);
                                        data.Add("title", title);
                                        data.Add("text", text);
                                        data.Add("status", status);
                                        data.Add("sentBy", sentByJson);
                                        data.Add("receivedBy", receivedByJson);
                                        data.Add("timeStamp", DateTime.Now.ToString());


                                        var notficationRepsone = await _fcmNotification.PushFcmNotificationAsync(new NotificationRequest { Title = "Send Reminder", Body = ur.Text, RegToken = ur.RegToken, Data = data, PopupNotification=false });
                                        Console.WriteLine($"For ReminderId {ur.ReminderId} in Loop {i + 1} for user {ur.UserId} - Sending notification to RegToken {ur.RegToken} with message {ur.Text}");
                                        _logger.LogInformation($"For ReminderId {ur.ReminderId} in Loop {i + 1} or user {ur.UserId} - Sending notification to RegToken {ur.RegToken} with message {ur.Text}");
                                    }

                                }
                            }
                            if (confirmed)
                            {
                                break;
                            }
                            //await System.Threading.Tasks.Task.Delay((int)response?.FirstOrDefault().NudgeInterval * 1000);
                            await System.Threading.Tasks.Task.Delay(interval * 1000);
                        }
                    }

                    try
                    {
                        await System.Threading.Tasks.Task.Delay(1000, stoppingToken);
                    }
                    catch (TaskCanceledException ex)
                    {
                        _logger.LogError($"Error in ExecuteAsync with message {ex.StackTrace}");
                    }

                });

            }
        }
    }

}
public interface IScopedProcessingService
{
    System.Threading.Tasks.Task DoWorkAsync(CancellationToken stoppingToken);
}