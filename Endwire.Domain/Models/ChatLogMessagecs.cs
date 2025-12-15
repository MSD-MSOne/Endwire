using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class ChatLogMessage
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ChatLogId { get; set; }
        public Nullable<System.DateTime> SentDate { get; set; }
        public string ItemType { get; set; }
        public string StringData { get; set; }
        public byte[] BinaryData { get; set; }
        public string BinaryUrl { get; set; }
        public string MetaInfo { get; set; }
        public virtual ChatLog ChatLog { get; set; }
        public virtual User User { get; set; }
    }
}
