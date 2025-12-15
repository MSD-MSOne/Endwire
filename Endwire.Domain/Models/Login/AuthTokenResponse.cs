using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class AuthTokenResponse
    {
        public string AuthToken { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;

        public string APIResponse { get; set; } = string.Empty;
    }
}
