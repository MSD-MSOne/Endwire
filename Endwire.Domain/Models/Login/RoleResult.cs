using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class RoleResult
    {
        public string Role { get; set; }
        public string Authorities { get; set; }
        public bool AuthoritiesValue { get; set; }
        public string Response { get; set; }    
    }
}
