using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class RtcTokenRequest
    {
        public string AppId { get; set; }
        public string AppCertificate { get; set; }
        public string ChannelName { get; set; }
    }
}