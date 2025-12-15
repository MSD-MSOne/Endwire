using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class ReminderResult
    {
        public int ReminderId { get; set; }
        public string ReminderType { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public int ResourceId { get; set; }
        public string ResourceName { get; set; } = string.Empty;
        public int NudgeId { get; set; }
        public string Text { get; set; } = string.Empty;
        //public DateTime DateAdded { get; set; }
        public string DateAdded { get; set; }
        public string NudgeStatus { get; set; } = string.Empty;
        public bool MultipleRecipients { get; set; }
        public string APIResponse { get; set; }

    }

    public class UserNudgeStatusResult
    {
        public string NudgeStatus { get; set; }
    }
}
