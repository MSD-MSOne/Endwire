using Azure.Core;
using EndWire.API.Repositories;
using EndWire.Domain.ConfigurationOptions;
using EndWire.Domain.Models;
using EndWire.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Infrastructure.Repositories.Mobile
{
    public class NudgeRepository : BaseRepository<EndWireContext>, INudgeRepository
    {
        private readonly StoredProcedures _storeProcedures;

        public NudgeRepository(EndWireContext context, IConfiguration configuration, IOptions<StoredProcedures> storedProcedures) : base (context, configuration)    
        {
            _storeProcedures = storedProcedures.Value;
        }
        public async Task<List<NudgeResponse>> GetNudgesForRoleAsync(string authToken)
        {
            var data = new
            {
                AuthToken = authToken
            };
            return await CallStoredProcedureWithResultsAsync<NudgeResponse>(_storeProcedures.GetNudgeList, data);
        }

        public async Task<List<OutboxListResult>> GetOutboxListAsync(NudgeRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken
            };
            return await CallStoredProcedureWithResultsAsync<OutboxListResult>(_storeProcedures.GetOutboxList, data);
        }

        public async Task<List<ConfirmByResult>> GetConfirmedByAsync(int UserOutboxId)
        {
            var data = new
            {
                UserOutboxId = UserOutboxId
            };
            return await CallStoredProcedureWithResultsAsync<ConfirmByResult>(_storeProcedures.GetConfirmBy, data);
        }

        public async Task<List<RecipientResult>> GetReminderRecepientsAsync(int reminderId)
        {
            var data = new
            {
                RminderIDS = reminderId
            };
            return await CallStoredProcedureWithResultsAsync<RecipientResult>(_storeProcedures.GetReminderRecepients, data);

        }
    }

    public interface INudgeRepository
    {
        Task<List<NudgeResponse>> GetNudgesForRoleAsync(string authToken);
        Task<List<OutboxListResult>> GetOutboxListAsync(NudgeRequest request);
        Task<List<ConfirmByResult>> GetConfirmedByAsync(int UserOutboxId);
        Task<List<RecipientResult>> GetReminderRecepientsAsync(int reminderId);
    }
        
}
