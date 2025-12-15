using Dapper;
using EndWire.Domain;
using EndWire.Domain.DTO;
using EndWire.Domain.Models;
using EndWire.Infrastructure.Repositories.Mobile;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static System.Net.Mime.MediaTypeNames;
using FirebaseAdmin;
using AgoraNET;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using EndWire.Core.Helpers;
using Microsoft.Extensions.Logging;
using Google.Apis.Logging;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace EndWire.Services.ServiceProviders
{
    public class FcmNotification : IFcmNotification
    {
        private readonly ILogger<FcmNotification> _logger;

        public FcmNotification(ILogger<FcmNotification> logger)
        {
            _logger = logger;
        }

        public async Task<string> PushFcmNotificationAsync(NotificationRequest request)
        {
            //Dictionary<string, string> dataDict = new Dictionary<string, string>();
            //dataDict.Add("userId", "");
            //dataDict.Add("channelId", "");
            //dataDict.Add("callToken", "");

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = await GoogleCredential.FromFileAsync("private-key.json", new CancellationToken())
                });
            }
            var config = new AndroidConfig { Priority = Priority.High};
            var message = request.PopupNotification ? new Message()
            {
                Data = request.Data,
                Token = request.RegToken,
                Notification = new Notification() { Title = request.Title, Body = request.Body },
                Android = config
            }
            : new Message()
            {
                Data = request.Data,
                Token = request.RegToken,
                Android = config
            };

            try
            {
                // for debugging
                string json = JsonConvert.SerializeObject(message);
                Console.WriteLine(json);
                _logger.LogInformation(json);
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in FcmNotification.PushFcmNotificationAsync with message {ex.Message}");
                throw;
            }
        }

    }

    public interface IFcmNotification
    {
       Task<string> PushFcmNotificationAsync(NotificationRequest request);
    }

    public class NotificationRequest
    {
        public string RegToken { get; set; }
        public string Title { get; set; }   
        public string Body { get; set; }

        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
        //public string ExtraTitle { get; set; } = string.Empty;
        //public string ExtraText { get; set; } = string.Empty;

        public bool PopupNotification { get; set; } = true;

    }

}
