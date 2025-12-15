using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EndWire.Domain.Models
{
    public class ChatLog
    {
        public ChatLog()
        {
            this.ChatLogMessages = new HashSet<ChatLogMessage>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int ConsultantId { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }

        public virtual User User { get; set; }
        public virtual User Consultant { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual ICollection<ChatLogMessage> ChatLogMessages { get; set; }
    }
}
