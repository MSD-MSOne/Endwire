using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class UserLocationResult
    {
        public string LocationId { get; set; }
        public string Location { get; set; }

        public bool CurrentLocation { get; set; }
    }
}
