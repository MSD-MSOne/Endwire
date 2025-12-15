using EndWire.API.Repositories;
using EndWire.Core.Helpers;
using EndWire.Domain.ConfigurationOptions;
using EndWire.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using EndWire.Domain.Models.API;

namespace EndWire.Infrastructure.Repositories.Mobile
{
    public class RtcCallRepository : BaseRepository<EndWireContext>, IRtcCallRepository
    {
        private readonly StoredProcedures _storeProcedures;
             
        public RtcCallRepository(EndWireContext context, IConfiguration configuration, IOptions<StoredProcedures> storedProcedures) : base(context, configuration)
        {
            _storeProcedures = storedProcedures.Value;
        }
        public async Task<List<RtcUserFCMResponse>> GetStartCallAsync(RtcCallRequest request)
        {
            var users = String.Join(",", request.Users.Select(x => x.UserId));
            var data = new
            {
                AuthToken = request.AuthToken,
                UserIDList = users,
                ChannelId = request.ChannelId
            };
            return await CallStoredProcedureWithResultsAsync<RtcUserFCMResponse>(_storeProcedures.GetStartCall, data);

        }

        public async Task<List<AcceptCallFCMResponse>> AcceptCallAsync(AcceptCallRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken,
                ChannelId = request.ChannelId,
                Accept = request.Accept
            };
            return await CallStoredProcedureWithResultsAsync<AcceptCallFCMResponse>(_storeProcedures.AcceptCall, data);

        }

        public async Task<EndCallResponse> EndRtcCallAsync(EndCallRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken,
                ChannelId = request.ChannelId
            };
            return await CallStoredProcedureWithResultAsync<EndCallResponse>(_storeProcedures.EndCall, data);
        }

        public async Task<ExitCallResponse> ExitCallAsync(ExitCallRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken,
                ChannelId = request.ChannelId
            };
            return await CallStoredProcedureWithResultAsync<ExitCallResponse>(_storeProcedures.ExitCall, data);
        }

        public async Task<List<FcmExitCallResponse>> FcmExitCallAsync(ExitCallRequest request)
        {
            var data = new
            {
                AuthToken = request.AuthToken,
                ChannelId = request.ChannelId
            };
            return await CallStoredProcedureWithResultsAsync<FcmExitCallResponse>(_storeProcedures.GetExitCall_FCM, data);
        }
    }
    public interface IRtcCallRepository
    {
        Task<List<RtcUserFCMResponse>> GetStartCallAsync(RtcCallRequest request);
        Task<EndCallResponse> EndRtcCallAsync(EndCallRequest request);
        Task<ExitCallResponse> ExitCallAsync(ExitCallRequest request);

        Task<List<FcmExitCallResponse>> FcmExitCallAsync(ExitCallRequest request);

        Task<List<AcceptCallFCMResponse>>  AcceptCallAsync(AcceptCallRequest request);
    }
}
