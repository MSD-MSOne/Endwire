using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class ContactsRequest
    {
        public string AuthToken { get; set; }
        public int? NudgeId { get; set; }
    }

    public class MarkfavRequest
    {
        public string? AuthToken { get; set; }
        public int UserId { get; set; }
    }

    public class GroupUsersList
    {
        public string AuthToken { get; set; }
        public int GroupId { get; set;}
    }
}
