using EndWire.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public bool Online { get; set; }
        public bool Favorite { get; set; }
        public string Role { get; set; }
        //public List<GroupResult> Groups { get; set; }
    }
}
