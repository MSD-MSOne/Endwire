using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string PassCode { get; set; }
        public string RegToken { get; set; }
    }

    public class SendFCMRequest
    {
        public Dictionary <string, string > Data { get; set; }

    }
}
