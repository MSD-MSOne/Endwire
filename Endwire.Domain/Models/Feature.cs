using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class Feature
{
    public int FeatureId { get; set; }

    public string Feature1 { get; set; } = null!;

    public virtual ICollection<RoleFeature> RoleFeatures { get; } = new List<RoleFeature>();
}
