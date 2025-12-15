using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class NudgeResource
{
    public int NudgeResourceId { get; set; }

    public int NudgeId { get; set; }

    public int ResourceId { get; set; }

    public int NudgesNudgeId { get; set; }

    public int ResourcesResourceId { get; set; }

    public virtual Nudge NudgesNudge { get; set; } = null!;

    public virtual Resource ResourcesResource { get; set; } = null!;
}
