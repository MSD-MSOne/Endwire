using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class NudgeResponse
    {
        public int NudgeId { get; set; }
        public string Nudge { get; set; }

        public string APIResponse { get; set; }
    }

    public class NudgeStatusChangeResponse
    {
        /*public string RegToken { get; set; }
        public int Userid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string TimeStamp { get; set; }
        public string NudgeStatus { get; set; }*/

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
    }

}
