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
using Newtonsoft.Json;
using System.Threading.Channels;

namespace EndWire.Services.ServiceProviders
{
    public class RtcServiceProvider : IRtcServiceProvider
    {
        private readonly IRtcCallRepository _rtcCallRepository;
        private readonly IFcmNotification _fcmNotification;
        private readonly ILogger<RtcServiceProvider> _logger;
        private string _appId = "c5ada4e280e249f185432cc3498ad906"; // af87b56cb96a4d539f0171d842e3e7e2
        private string _appCertificate = "6400b96609fe4d8997a02d1f243f53b0"; // 5504df3cf59343aea29432aff29893d2
        private string _channelName = "endwire-chat"; // "7d72365eb983485397e3e3f9d460bdda";
        private string _uid = "ff5cc111825d47768acd05ffe49d64d5"; //"2882341273";
        private uint _ts = 1111111;
        private uint _salt = 1;
        private uint _expiredTs = 1446455471;

        public RtcServiceProvider(IRtcCallRepository rtcCallRepository, ILogger<RtcServiceProvider> logger, IFcmNotification fcmNotification)
        {
            _rtcCallRepository = rtcCallRepository;
            _logger = logger;
            _fcmNotification = fcmNotification; 
        }
        public async Task<List<RtcUserFCMResponse>> GetStartCallAsync(RtcCallRequest request)
        {

            var fcmDataResponse = await _rtcCallRepository.GetStartCallAsync(request);


            // Send FCM data message

            //if (fcmDataResponse?.FirstOrDefault()?.APIResponse is not null)
            //{
                //return new AcceptCallResponse { CallStatus = "", APIResponse = fcmDataResponse?.FirstOrDefault()?.APIResponse };
            //}

            if (fcmDataResponse?.FirstOrDefault() is not null)
            {
                foreach (var ur in fcmDataResponse)
                {
                    //if (!string.IsNullOrWhiteSpace(ur.RegToken) && ur.Subsection.Equals("sendFCMto"))
                    //{
                    if (ur.UserId != 0)
                    {
                        var callToken = GetRtcAccessToken(request.ChannelId);


                        var priority = fcmDataResponse.Where(x => x.Section.Equals("android") && x.Subsection.Equals("")).FirstOrDefault().Priority;
                        var reminder = fcmDataResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("")).FirstOrDefault();
                        var type = reminder?.Type;
                        var title = reminder?.Title;
                        var text = reminder?.Text;
                        var status = reminder?.Status;

                        var startedBy = fcmDataResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("startedBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName }).FirstOrDefault();

                        var callInfo = fcmDataResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("callInfo")).Select(x => new { channelId = request.ChannelId, callToken = callToken }).FirstOrDefault();

                        var sendFCMto = fcmDataResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("sendFCMto")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName }).FirstOrDefault();

                        var receivedBy = fcmDataResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("receivedBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName }).ToList();

                        var startedByJson = JsonConvert.SerializeObject(startedBy);
                        var receivedByJson = JsonConvert.SerializeObject(receivedBy);
                        var callInfoJson = JsonConvert.SerializeObject(callInfo);


                        Dictionary<string, string> data = new Dictionary<string, string>();

                        data.Add("title", title);
                        data.Add("text", text);
                        data.Add("status", status);
                        data.Add("timeStamp", DateTime.Now.ToString());
                        data.Add("type", type);
                        data.Add("receivedBy", receivedByJson);
                        data.Add("startedBy", startedByJson);
                        data.Add("callInfo", callInfoJson);

                        var notficationRepsone = ur.RegToken ?? await _fcmNotification.PushFcmNotificationAsync(new NotificationRequest { Title = title, Body = ur.Text, RegToken = ur.RegToken, Data = data, PopupNotification = false });
                        //   }
                    }
                }
            }




            return fcmDataResponse ?? new List<RtcUserFCMResponse>();
        }

        public async Task<AcceptCallResponse> AcceptCallAsync(AcceptCallRequest request)
        {
            var fcmDataResponse =  await _rtcCallRepository.AcceptCallAsync(request);

            if(fcmDataResponse?.FirstOrDefault()?.APIResponse is not null)
            {
                return new AcceptCallResponse { CallStatus = "", APIResponse = fcmDataResponse?.FirstOrDefault()?.APIResponse };
            }

            if (fcmDataResponse?.FirstOrDefault() is not null)
            {
                foreach (var ur in fcmDataResponse)
                {
                    if (!string.IsNullOrWhiteSpace(ur.RegToken) && ur.Subsection.Equals("sendFCMto"))
                    {
                        var priority = fcmDataResponse.Where(x => x.Section.Equals("android") && x.Subsection.Equals("")).FirstOrDefault().Priority;
                        var reminder = fcmDataResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("")).FirstOrDefault();
                        var type = reminder?.Type;
                        var title = reminder?.Title;
                        var text = reminder?.Text;
                        var status = reminder?.Status;

                        var startedBy = fcmDataResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("startedBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName}).FirstOrDefault();

                        var callInfo = fcmDataResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("callInfo")).Select(x => new { channelId = x.ChannelId}).FirstOrDefault();

                        var sendFCMto = fcmDataResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("sendFCMto")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName}).FirstOrDefault();

                        var receivedBy = fcmDataResponse.Where(x => x.Section.Equals("data") && x.Subsection.Equals("receivedBy")).Select(x => new { userId = x.UserId, firstName = x.FirstName, lastName = x.LastName}).ToList();

                        var startedByJson = JsonConvert.SerializeObject(startedBy);
                        var receivedByJson = JsonConvert.SerializeObject(receivedBy);
                        var callInfoJson = JsonConvert.SerializeObject(callInfo);


                        Dictionary<string, string> data = new Dictionary<string, string>();

                        data.Add("title", title);
                        data.Add("text", text);
                        data.Add("status", status);
                        data.Add("timeStamp", DateTime.Now.ToString());
                        data.Add("type", type);
                        data.Add("receivedBy", receivedByJson);
                        data.Add("startedBy", startedByJson);
                        data.Add("callInfo", callInfoJson);

                        var notficationRepsone = await _fcmNotification.PushFcmNotificationAsync(new NotificationRequest { Title = title, Body = ur.Text, RegToken = ur.RegToken, Data = data, PopupNotification = false });
                    }
                }
            }
            
            var response = new AcceptCallResponse {  APIResponse = "Success", CallStatus = request.Accept.Value ? "call accepted" :"call declined"};

            return response;
        }

        public async Task<EndCallResponse> EndRtcCallAsync(EndCallRequest request)
        {
            var response = await _rtcCallRepository.EndRtcCallAsync(request);
            return response;
        }

        public async Task<ExitCallResponse> ExitCallAsync(ExitCallRequest request)
        {
            var response = await _rtcCallRepository.ExitCallAsync(request);
            return response;
        }

        public async Task<List<FcmExitCallResponse>> FcmExitCallAsync(ExitCallRequest request)
        {
            var response = await _rtcCallRepository.FcmExitCallAsync(request);
            return response;
        }

        public string GetRtcAccessToken(string channelName)
        {
            // Generate Access Token for Agora RTC
            AccessToken token =
                new AccessToken(_appId, _appCertificate, channelName, _uid);
            token.message.ts = _ts;
            token.message.salt = _salt;
            token.AddPrivilege(Privileges.JoinChannel, _expiredTs);

            int yield = 123;
            Console.WriteLine(yield.ToString());
            return token.Build();
        }

    }

    public interface IRtcServiceProvider
    {
        Task<List<RtcUserFCMResponse>> GetStartCallAsync(RtcCallRequest request);
        Task<AcceptCallResponse> AcceptCallAsync(AcceptCallRequest request);
        Task<EndCallResponse> EndRtcCallAsync(EndCallRequest request);
        Task<ExitCallResponse>  ExitCallAsync(ExitCallRequest request);
        Task<List<FcmExitCallResponse>>  FcmExitCallAsync(ExitCallRequest request);
        string GetRtcAccessToken(string channelName);
    }

}
