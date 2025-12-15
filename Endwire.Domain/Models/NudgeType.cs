using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class NudgeType
{
    public int NudgeTypeId { get; set; }

    public string NudgeType1 { get; set; } = null!;

    public int NudgesNudgeId { get; set; }

    public virtual Nudge NudgesNudge { get; set; } = null!;
}
