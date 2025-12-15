using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain
{
    public class ConfirmByDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
    }
}
