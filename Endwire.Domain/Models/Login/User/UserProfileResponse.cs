using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class UserProfileResponse
    {
        public int UserId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public bool Online { get; set; }  
        public bool Favorite { get; set; }  
        public List<RoleDto> Roles { get; set; } = new List<RoleDto>(); 
        public List<Loc> Locations { get; set; } = new List<Loc>();  
        public string APIResponse { get; set; }
    }


}
