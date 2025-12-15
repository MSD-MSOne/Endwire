using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class RoleNudge
{
    public int RoleNudgeId { get; set; }

    public int RoleId { get; set; }

    public int NudgeId { get; set; }

    public bool Receive { get; set; }

    public bool Send { get; set; }

    public int RolesRoleId { get; set; }

    public int NudgesNudgeId { get; set; }

    public virtual Nudge NudgesNudge { get; set; } = null!;

    public virtual Role RolesRole { get; set; } = null!;
}
