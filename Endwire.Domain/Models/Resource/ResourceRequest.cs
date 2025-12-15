using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class ResourceRequest
    {
        public string AuthToken { get; set; }
        public int? NudgeId { get; set; }
    }

    public class AddResourceRequest
    {
        public List<int> LocationIdList { get; set; } = new List<int>();    
        public string ResourceName { get; set; }
        public bool Active { get; set; }
    }

    public class AddResourceResponse
    {
        //public int ResourceId { get; set; }
        public string APIResponse { get; set; }
    }
}
