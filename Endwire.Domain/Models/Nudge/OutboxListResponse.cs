using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
   public class OutboxListResponse
   {
        public List<ReminderDto> Reminders { get; set; } = new List<ReminderDto>();
        public string APIResponse { get; set; }
    }

    public class InboxListResponse
    {
        public List<InboxReminderDto> Reminders { get; set; } = new List<InboxReminderDto>();    
        public string APIResponse { get; set; }
    }
}
