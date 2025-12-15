using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class Resource
{
    public int ResourceId { get; set; }

    public string ResourceName { get; set; } = null!;

    public int LocationId { get; set; }

    public int LocationsLocationId { get; set; }

    public virtual Location LocationsLocation { get; set; } = null!;

    public virtual ICollection<NudgeResource> NudgeResources { get; } = new List<NudgeResource>();
}
