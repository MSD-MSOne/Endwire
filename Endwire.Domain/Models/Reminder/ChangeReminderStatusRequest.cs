using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class ChangeReminderStatusRequest
    {
        public string? AuthToken { get; set; }
        public string ReminderId { get; set; }
        public string Status { get; set; }
    }
}