using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class RoleFeature
{
    public int RoleFeatureId { get; set; }

    public int RoleId { get; set; }

    public int FeatureId { get; set; }

    public bool FeatureAvailable { get; set; }

    public int RolesRoleId { get; set; }

    public int FeaturesFeatureId { get; set; }

    public virtual Feature FeaturesFeature { get; set; } = null!;

    public virtual Role RolesRole { get; set; } = null!;
}
