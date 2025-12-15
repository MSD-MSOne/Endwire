using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models.Timer
{
    public class SetTimerRequest
    {
        //public string AuthToken { get; set; }
        public int TaskId { get; set; }
        public int ResourceId { get; set; }
    }

}
