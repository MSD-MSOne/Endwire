using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class OutboxListResult
    {
        public int ReminderId { get; set; }
        public int ResourceId { get; set; }
        public string Resource { get; set; } = string.Empty;
        public int NudgeId { get; set; }
        public string Text { get; set; } = string.Empty;
        //public DateTime TimeStamp { get; set; }
        public string TimeStamp { get; set; }
        public string Status { get; set; } = string.Empty;

        public string MultipleRecipients { get; set; }
        public string APIResponse { get; set; }
    }
}
