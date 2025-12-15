using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class LoginResponse
    {
        public string AuthToken { get; set; } = string.Empty;
        //public string Response { get; set; } = string.Empty;

        public int LocationCount { get; set; }

        public List<Loc>? Locations { get; set; }

        public RoleResponse? Roles { get; set; }

        public string Error { get; set; } = string.Empty;

        public string APIResponse { get; set; } = string.Empty;

    }

    public class RoleResponse
    {
        public string Name { get; set; }
        public Auth Authorities { get; set; }
    }

    public class LocationResponse
    {
        public int LocationCount { get; set; }
        public List<Loc> Locations { get; set; }
    }

    /*public class Auth
    {
        public string Name { get; set; }  
        public bool Value { get; set; }   
    }*/

    public class Auth
    {
        public bool Broadcast { get; set; }
        public bool Nudge { get; set; }
        public bool GroupCall { get; set; }
        public bool VoiceCall { get; set; }
        public bool Reminders { get; set; }
    }

    public class Loc
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //public bool CurrentLocation { get; set; }
    }
}
