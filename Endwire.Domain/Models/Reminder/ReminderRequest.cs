using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class ReminderRequest
    {
        public string AuthToken { get; set; }
        public string ReminderType { get; set; }
    }

    public class UserNudgeStatusRequest
    {
        public int UserId { get; set; }
        public int ReminderId { get; set; }
    }
}
