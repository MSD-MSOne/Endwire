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

namespace EndWire.Services.ServiceProviders
{
    public class NudgeProvider : INudgeProvider
    {
        private readonly INudgeRepository _nudgeRepository;

        public NudgeProvider(INudgeRepository nudgeRepository)
        {
            _nudgeRepository = nudgeRepository;
        }
        public async Task<List<NudgeResponse>> GetNudgesForRoleAsync(NudgeRequest request)
        {
            var response = await _nudgeRepository.GetNudgesForRoleAsync(request.AuthToken);
            if (response.IsNullOrEmpty())
            {
                return null;
            }

            return response;
        }

        public async Task<OutboxListResponse> GetOutboxListAsync(NudgeRequest request)
        {
            var results = await _nudgeRepository.GetOutboxListAsync(request);
            OutboxListResponse response = new OutboxListResponse();
            response.APIResponse = results.FirstOrDefault().APIResponse;

            if (results.FirstOrDefault().ReminderId > 0)
            {
                List<ReminderDto> reminders = new List<ReminderDto>();
                foreach (var r in results) // will be refactored to use mapper
                {
                    var recepients = await _nudgeRepository.GetReminderRecepientsAsync(r.ReminderId);
                    List<ReminderUserDto> recipientDtos = new List<ReminderUserDto>();
                    foreach (var recipient in recepients)
                    {
                        recipientDtos.Add(new ReminderUserDto { FirstName = recipient.FirstName, LastName = recipient.LastName, UserId = recipient.UserId, Picture = recipient.Picture });
                    }

                    var confirmbys = await _nudgeRepository.GetConfirmedByAsync(r.ReminderId);
                    List<ConfirmByDto> confirmByDtos = new List<ConfirmByDto>();
                    foreach (var confirmby in confirmbys)
                    {
                        confirmByDtos.Add(new ConfirmByDto { FirstName = confirmby.FirstName, UserId = confirmby.UserId, Picture = confirmby.Picture });

                    }
                    reminders.Add(new ReminderDto
                    {
                        ReminderId = r.ReminderId,
                        Recipients = recipientDtos,
                        ResourceId = r.ResourceId,
                        NudgeId = r.NudgeId,
                        Resource = r.Resource,
                        Text = r.Text,
                        TimeStamp = r.TimeStamp.ToString(),
                        Status = r.Status,
                        ConfirmedBy = confirmByDtos,
                    });
                    response.Reminders = reminders;
                }
            }
            return response;


        }

        public async Task<List<ConfirmByResult>> GetConfirmedByAsync(int UserOutboxId)
        {
            var response = await _nudgeRepository.GetConfirmedByAsync(UserOutboxId);
            return response;
        }

        public async Task<List<RecipientResult>> GetReminderRecepientsAsync(int reminderId)
        {
            var response = await _nudgeRepository.GetReminderRecepientsAsync(reminderId);
            return response;
        }

    }

    public interface INudgeProvider
    {
        Task<List<NudgeResponse>> GetNudgesForRoleAsync(NudgeRequest request);
        Task<OutboxListResponse> GetOutboxListAsync(NudgeRequest request);
        Task<List<ConfirmByResult>> GetConfirmedByAsync(int UserOutboxId);
        Task<List<RecipientResult>> GetReminderRecepientsAsync(int reminderId);
    }

}
