using Dapper;
using EndWire.API.Repositories;
using EndWire.Domain.ConfigurationOptions;
using EndWire.Domain.Models;
using EndWire.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Infrastructure.Repositories.Mobile
{
    public class ReminderRepository : BaseRepository<EndWireContext>, IReminderRepository
    {
        private readonly StoredProcedures _storeProcedures;
        //private readonly ILogger<ReminderRepository> _logger;   

        public ReminderRepository(EndWireContext context, IConfiguration configuration, IOptions<StoredProcedures> storedProcedures) : base(context, configuration)
        {
            _storeProcedures = storedProcedures.Value;
        }
        public async Task<List<ReminderResult>> GetRemindersAsync(ReminderRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken,
                ReminderType = request.ReminderType
            };
            return await CallStoredProcedureWithResultsAsync<ReminderResult>(_storeProcedures.GetInboxList, data);
        }

        public async Task<ChangeReminderStatusResponse> SendReminderAsync(SendReminderRequest request)
        {
            var recipients = String.Join(",", request.Recipients.Select(x => x.UserId));

            var data = new
            {
                AuthToken = request.AuthToken,
                ReminderType = request.ReminderType,
                NudgeId = request.NudgeId,
                ResourceId = request.ResourceId,
                Recepients = recipients,
                Text = request.Text   
            };
            return await CallStoredProcedureWithResultAsync<ChangeReminderStatusResponse>(_storeProcedures.SendReminder, data);
        }

        public async Task<List<ChangeReminderStatusFcmResponse>> FcmSendReminderAsync(SendReminderRequest request)
        {
            var recipients = String.Join(",", request.Recipients.Select(x => x.UserId));

            var data = new
            {
                AuthToken = request.AuthToken,
                ReminderType = request.ReminderType,
                NudgeId = request.NudgeId,
                ResourceId = request.ResourceId,
                Recepients = recipients,
                Text = request.Text,
                OutboxId = request.OutboxId 
            };
            return await CallStoredProcedureWithResultsAsync<ChangeReminderStatusFcmResponse>(_storeProcedures.FcmSendReminder, data);
        }

        public async Task<ChangeReminderStatusResponse> ChangeReminderStatusAsync(ChangeReminderStatusRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken,
                ReminderId = request.ReminderId,
                Status = request.Status
            };
            return await CallStoredProcedureWithResultAsync<ChangeReminderStatusResponse>(_storeProcedures.UpdateNudgeStatus, data);
        }
        public async Task<List<RecipientResult>> GetReminderRecipientsAsync(string remainderIds)
        {
            var data = new
            {
                RminderIDS = remainderIds
            };
            return await CallStoredProcedureWithResultsAsync<RecipientResult>(_storeProcedures.GetReminderRecepients, data);
        }

        public async Task<List<NudgeStatusChangeResponse>> FcmNudgeStatusChangeAsync(ChangeReminderStatusRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken,
                ReminderId = request.ReminderId,
                Status = request.Status 
            };

            return await CallStoredProcedureWithResultsAsync<NudgeStatusChangeResponse>(_storeProcedures.NudgeStatusChange_FCM, data);
        }

        public async Task<UserNudgeStatusResult> GetUserNudgeStatusAsync(UserNudgeStatusRequest request)
        {
            var data = new
            {
                UserId = request.UserId,        
                ReminderId = request.ReminderId
            };
            return await CallStoredProcedureWithResultAsync<UserNudgeStatusResult>(_storeProcedures.GetUserNudgeStatus, data);
        }
    }
    

    public interface IReminderRepository
    {
        Task<List<ReminderResult>> GetRemindersAsync(ReminderRequest request);
        Task<ChangeReminderStatusResponse> SendReminderAsync (SendReminderRequest request);
        Task<List<ChangeReminderStatusFcmResponse>> FcmSendReminderAsync(SendReminderRequest request);
        Task<ChangeReminderStatusResponse> ChangeReminderStatusAsync(ChangeReminderStatusRequest request);

        Task <List<RecipientResult>> GetReminderRecipientsAsync(string remainderIds);

        Task<List<NudgeStatusChangeResponse>> FcmNudgeStatusChangeAsync(ChangeReminderStatusRequest request);

        Task<UserNudgeStatusResult> GetUserNudgeStatusAsync(UserNudgeStatusRequest request);
    }
        
}
