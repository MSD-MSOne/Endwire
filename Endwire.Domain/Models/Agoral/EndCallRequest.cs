using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class EndCallRequest
    {
        public string? AuthToken { get; set; }
        public string? ChannelId { get; set; }
    }
}
