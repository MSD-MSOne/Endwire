using Dapper;
using EndWire.Domain;
using EndWire.Domain.DTO;
using EndWire.Domain.Models;
using EndWire.Infrastructure.Repositories.Mobile;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
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
using EndWire.Core.Helpers;
using Microsoft.Extensions.Logging;

namespace EndWire.Services.ServiceProviders
{
    public class BroadcastProvider : IBroadcastProvider
    {
        private readonly IBroadcastRepository _broadcastRepository;
        private readonly IRtcServiceProvider _rtcServiceProvider;
        private readonly IFcmNotification _fcmNotification;
        private readonly ILogger<BroadcastProvider> _logger;
        private readonly string _channelName = "endwire-broadcast";

        public BroadcastProvider(IBroadcastRepository broadcastRepository, IRtcServiceProvider rtcServiceProvider, IFcmNotification fcmNotification, ILogger<BroadcastProvider> logger)
        {
            _broadcastRepository = broadcastRepository;
            _rtcServiceProvider = rtcServiceProvider;
            _fcmNotification = fcmNotification;
            _logger = logger; 
        }
        public async Task<BroadcastResponse> StartBroadcastAsync(BroadcastRequest request)
        {
            request.ChannelId = request.ChannelId ?? (_channelName +RandomStringHelper.RandomString(20));
            BroadcastResponse response = new BroadcastResponse();
            List<RtcUser> rtcUsers = new List<RtcUser>();

            var result = await _broadcastRepository.StartBroadcastAsync(request);
            try
            {
                if (result?.FirstOrDefault()?.UserId > 0)
                {
                    foreach (var r in result)
                    {
                        var callToken = _rtcServiceProvider.GetRtcAccessToken(request.ChannelId);
                        var notificationRequest = new NotificationRequest { Title = "Start Broadcast", Body = r.FCMMessage, RegToken = r.RegToken };

                        notificationRequest.Data.Add("userid", r.UserId.ToString());
                        notificationRequest.Data.Add("channelid", request.ChannelId);
                        notificationRequest.Data.Add("calltoken", callToken);

                        var fcmResponse = await _fcmNotification.PushFcmNotificationAsync(notificationRequest);
                    }
                }
                //response.Recipients = rtcUsers;
                if (result?.FirstOrDefault()?.UserId > 0)
                {
                    response.CallToken = _rtcServiceProvider.GetRtcAccessToken(request.ChannelId);
                    response.ChannelId = request.ChannelId;
                    response.Recipients = result.Select (x=>new RtcUser { UserId = x.UserId}).ToList();
                }
                response.APIResponse = result?.FirstOrDefault()?.APIResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in BroadcastProvider.StartBroadcastAsync with message {ex.Message}");
                throw;
            }
            return response;
        }

        public async Task<EndBroadcastResponse> EndBroadcastAsync(EndBroadcastRequest request)
        {
            var response = await _broadcastRepository.EndBroadcastAsync(request);
            return response?? new EndBroadcastResponse { Status="", APIResponse="BadToken" };
        }

    }

    public interface IBroadcastProvider
    {
        Task<BroadcastResponse> StartBroadcastAsync(BroadcastRequest request);
        Task<EndBroadcastResponse> EndBroadcastAsync(EndBroadcastRequest request);
    }

}
