using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public string Group1 { get; set; } = null!;

    public virtual ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
}
