using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class RtcCallRequest
    {
        public string? AuthToken { get; set; }
        public List<RtcUser> Users { get; set; }

        public string?  ChannelId { get; set; }
    }

    public class AcceptCallRequest
    {
        public string? AuthToken { get; set; }
        public string ChannelId { get; set; }
        public string? CallToken { get; set; }

        public bool? Accept { get; set; }
    }

    public class ExitCallRequest
    {
        public string? AuthToken { get; set; }
        public string ChannelId { get; set; }
        public string? CallToken { get; set; }
        public bool? Accept { get; set; }
    }

    public class AcceptCallResponse
    {
        public string CallStatus { get; set; } 
        public string APIResponse { get; set; } 
    }

    public class AcceptCallFCMResponse
    {
        public string RegToken { get; set; }
        public int NudgeInterval { get; set; }
        public int ReminderRepeatTimes { get; set; }
        public string Section { get; set; }
        public string Subsection { get; set; }
        public string Priority { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OutboxId { get; set; }
        public int InboxId { get; set; }
        public int ReminderId { get; set; }

        public string ChannelId { get; set; }
        public string CallStatus { get; set; }
        public string APIResponse { get; set; }
    }

    public class ExitCallResponse
    {
        public string CallStatus { get; set; } 
        public string APIResponse { get; set; }
    }

    public class FcmExitCallResponse
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string ChannelId { get; set; }

        public string RegToken { get; set; }   
        //public string CallStatus { get; set; }
    }

    public class RtcUser
    {

        public int UserId { get; set; } 
    }

    public class RtcUserResponse
    {
        public int UserId { get; set; }
        public string RegToken { get; set; }
        public string Caller { get; set; }=string.Empty;
        public string FCMMessage { get; set; }
        public string APIResponse { get; set; }
    }

    public class RtcUserFCMResponse
    {
        public string RegToken { get; set; }
        public int NudgeInterval { get; set; }
        public int ReminderRepeatTimes { get; set; }
        public string Section { get; set; }
        public string Subsection { get; set; }
        public string Priority { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OutboxId { get; set; }
        public int InboxId { get; set; }
        public int ReminderId { get; set; }


        //public int UserId { get; set; }
        //public string RegToken { get; set; }
        //public string Caller { get; set; } = string.Empty;
        //public string FCMMessage { get; set; }
        public string APIResponse { get; set; }
    }
}