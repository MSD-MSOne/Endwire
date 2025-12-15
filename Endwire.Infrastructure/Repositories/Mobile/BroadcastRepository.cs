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
    public class BroadcastRepository : BaseRepository<EndWireContext>, IBroadcastRepository
    {
        private readonly StoredProcedures _storeProcedures;

        public BroadcastRepository(EndWireContext context, IConfiguration configuration, IOptions<StoredProcedures> storedProcedures) : base (context, configuration)    
        {
            _storeProcedures = storedProcedures.Value;
        }

        public async Task<List<BroadcastResult>> StartBroadcastAsync(BroadcastRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken,
                ChannelId = request.ChannelId
            };
            return await CallStoredProcedureWithResultsAsync<BroadcastResult>(_storeProcedures.GetStartBroadcast, data);
        }

        public async Task<EndBroadcastResponse> EndBroadcastAsync(EndBroadcastRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken,
                ChannelId = request.ChannelId
            };
            return await CallStoredProcedureWithResultAsync<EndBroadcastResponse>(_storeProcedures.GetEndBroadcast, data);
        }
    }

    public interface IBroadcastRepository
    {
        Task<List<BroadcastResult>> StartBroadcastAsync(BroadcastRequest request);
        Task<EndBroadcastResponse> EndBroadcastAsync(EndBroadcastRequest request);
     }
        
}
