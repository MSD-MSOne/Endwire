using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class Nudge
{
    public int NudgeId { get; set; }

    public string Nudge1 { get; set; } = null!;

    public int NudgeTypeId { get; set; }

    public virtual ICollection<NudgeResource> NudgeResources { get; } = new List<NudgeResource>();

    public virtual ICollection<NudgeType> NudgeTypes { get; } = new List<NudgeType>();

    public virtual ICollection<RoleNudge> RoleNudges { get; } = new List<RoleNudge>();

    public virtual ICollection<UserInbox> UserInboxes { get; } = new List<UserInbox>();

    public virtual ICollection<UserOutbox> UserOutboxes { get; } = new List<UserOutbox>();
}
