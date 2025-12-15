using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class ChangeReminderStatusResponse
    {
        public int ReminderId { get; set; } 
        //public DateTime TimeStamp { get;set; }
        public string TimeStamp { get; set; }
        public string Status { get;set; }
        public string APIResponse { get; set; }
    }
    public class ChangeReminderStatusFcmResponse
    {
        //public string Action { get; set; }
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
        ////public DateTime TimeStamp { get;set; }
        //public string FCMMessage { get; set; }
        
        //public int UserInboxId { get; set; }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OutboxId { get; set; }
        public int InboxId { get; set; }
        public int ReminderId { get; set; }
        
        //public string NudgeStatus { get; set; }
        
        //public string Response { get; set; }
        //public string APIResponse { get; set; }
    }
}
