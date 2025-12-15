using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class ContactListResult
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; } = string.Empty;
        public bool Online { get; set; } = false;
        public bool Favorite { get; set; }=false;

        public string Role { get; set; }
        public string APIResponse { get; set; }
    }

    public class ContactListResultDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; } = string.Empty;
        public bool Online { get; set; }
        public string Role { get; set; }
        public bool Favorite { get; set; }
    }

    public class ContactListResultModel
    {
        public List<ContactListResultDto> contacts { get; set; } = new List<ContactListResultDto>();    
        public string APIResponse { get; set; }

    }
}
