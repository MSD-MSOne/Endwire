using EndWire.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain
{
    public class ReminderDto
    {
        public int ReminderId { get; set; }
        public List<ReminderUserDto> Recipients { get; set; }
        public int ResourceId { get; set; }
        public int NudgeId { get; set; }
        public string Resource { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = string.Empty;
        public string Status { get; set; }
        public List<ConfirmByDto> ConfirmedBy { get; set; }
    }

    public class InboxReminderDto
    {
        public int ReminderId { get; set; }
        public string ReminderType { get; set; }
        public ReminderUserDto Sender { get; set; }
        public int ResourceId { get; set; }
        public string Resource { get; set; } = string.Empty;
        public int NudgeId { get; set; }    
        public string Text { get; set; } = string.Empty;
        //public string Picture { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = string.Empty;
        public string Status { get; set; }
        public bool MultipleRecipients { get; set; }
    }
}
