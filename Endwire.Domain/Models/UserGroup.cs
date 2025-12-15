using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class UserGroup
{
    public int UserGroupId { get; set; }

    public int GroupId { get; set; }

    public int UserId { get; set; }

    public int GroupsGroupId { get; set; }

    public int UsersUserId { get; set; }

    public virtual Group GroupsGroup { get; set; } = null!;

    public virtual User UsersUser { get; set; } = null!;
}
