using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class UserType
{
    public int UserTypeId { get; set; }

    public string UserType1 { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
