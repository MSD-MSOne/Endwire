using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class LoginResponseModel
    {
        public string AuthToken { get; set; } = string.Empty;

        public RoleResponse? Roles { get; set; }
        public int LocationCount { get; set; }

        public List<Loc>? Locations { get; set; }

        public string APIResponse { get; set; }

    }


}
