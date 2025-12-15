using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class ContactsResponse
    {
        public List<UserDto> Users { get; set; } = new List<UserDto>(); 
        public string APIResponse { get; set; }
    }
}
