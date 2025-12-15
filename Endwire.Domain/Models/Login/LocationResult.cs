using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class LocationResult
    {
        public int LocationCount { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string APIResponse { get; set; }
    }
}
