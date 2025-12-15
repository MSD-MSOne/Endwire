using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class SendReminderRequest
    {
        public string? AuthToken { get; set; }
        public string ReminderType { get; set; }
        public int? NudgeId { get; set; }
        public int ResourceId { get; set; }
        public List<MobileUser> Recipients { get; set; }
        public string Text { get; set; }
        public int? OutboxId { get; set; }
    }
    public class MobileUser
    {
        public int UserId { get; set; }
    }
}