using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string Role1 { get; set; } = null!;

    public virtual ICollection<RoleFeature> RoleFeatures { get; } = new List<RoleFeature>();

    public virtual ICollection<RoleNudge> RoleNudges { get; } = new List<RoleNudge>();

    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
}
