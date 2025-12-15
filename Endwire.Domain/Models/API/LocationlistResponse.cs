using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models.API
{
   public class LocationlistResponse
    {
        public int LocationCount { get; set; }
        public List<Loc> Locations { get; set; }
        public string APIResponse { get; set; }
    }
    //--------------Amy -------------------
    public class LocationselectResult
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Response { get; set; }
        public string APIResponse { get; set; }
    }
}
