using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class ResourceResponse
    {
        public List<ResourceDto> Resources { get; set; } = new List<ResourceDto>(); 
        public string APIResponse { get; set; }

    }
}
