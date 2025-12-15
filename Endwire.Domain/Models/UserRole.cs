using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class UserRole
{
    public int UserRoleId { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public int RolesRoleId { get; set; }

    public int UsersUserId { get; set; }

    public virtual Role RolesRole { get; set; } = null!;

    public virtual User UsersUser { get; set; } = null!;
}
