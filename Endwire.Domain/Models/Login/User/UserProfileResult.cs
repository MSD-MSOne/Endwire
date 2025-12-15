using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class UserProfileResult
    {
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; } = string.Empty;
        public bool Online { get; set; }    
        public string APIResponse { get; set; }
    }

    public class FavResponse
    {
        public string Status { get; set; } = string.Empty;
        public string APIResponse { get; set; } = string.Empty;
    }
}
