using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class BroadcastResponse
    {
        public List<RtcUser> Recipients { get; set; } = new List<RtcUser>();    
        public string ChannelId { get; set; }
        public string CallToken { get; set; }

        public string APIResponse { get; set; }
    }
    public class BroadcastResult
    {
        public int UserId { get; set; }
        public string RegToken { get; set;}
        public string APIResponse { get; set; }
        public string FCMMessage { get; set; }
    }
}
