using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class UserRequest
    {
        public string AuthToken { get; set; }
        public int? UserId { get; set; }
    }
}
