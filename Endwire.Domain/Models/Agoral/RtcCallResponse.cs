using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class RtcCallResponse
    {
        public string ChannelId { get; set; } 
        public List<RtcCallUser> Users { get; set; } = new List<RtcCallUser>();

        public string CallToken { get; set; } = string.Empty;
        public string APIResponse { get; set; } = string.Empty; 
    }

    public class RtcCallResponseModel
    {
        public string ChannelId { get; set; } = string.Empty;
        public string CallToken { get; set; } = string.Empty;   
        public string APIResponse { get; set; }
    }

    public class RtcCallUser
    {
        public int UserId { get; set; }
        public string CallToken { get; set; }

        public string Status { get; set; }
    }

}
