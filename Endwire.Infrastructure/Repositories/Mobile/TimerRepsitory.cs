using Azure.Core;
using Dapper;
using EndWire.API.Repositories;
using EndWire.Domain.ConfigurationOptions;
using EndWire.Domain.Models;
using EndWire.Domain.Models.API;
using EndWire.Domain.Models.Timer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace EndWire.Infrastructure.Repositories.Mobile
{
    public class TimerRepsitory: BaseRepository<EndWireContext>, ITimerRepsitory
    {
        private readonly StoredProcedures _storeProcedures;

        public TimerRepsitory(EndWireContext context, IConfiguration configuration, IOptions<StoredProcedures> storedProcedures) : base(context, configuration)
        {
            _storeProcedures = storedProcedures.Value;
        }

        public async Task<List<SetTimerResponse>> SetTimerAsync(string AuthToken, int Taskid, int ResourceId)
        {
            var data = new
            {
                AuthToken = AuthToken,
                TaskID = Taskid,
                ResourceID = ResourceId
            };
          
            return await CallStoredProcedureWithResultsAsync<SetTimerResponse>(_storeProcedures.SetTimerTask, data);
           

        }
        public async Task<List<TimerTaskResponse>> TimerTaskListAsync(string authToken)
        {
            var data = new
            {
                AuthToken = authToken
            };

            return await CallStoredProcedureWithResultsAsync<TimerTaskResponse>(_storeProcedures.GetTimerTaskList, data);
        }

    }

    public interface ITimerRepsitory
    {
        Task<List<SetTimerResponse>> SetTimerAsync(string AuthToken, int Taskid, int ResourceId);
        Task<List<TimerTaskResponse>> TimerTaskListAsync(string authToken);
    }
}
