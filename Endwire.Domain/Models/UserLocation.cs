using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class UserLocation
{
    public int UserLocationId { get; set; }

    public int UserId { get; set; }

    public int LocationId { get; set; }

    public int LocationsLocationId { get; set; }

    public int UsersUserId { get; set; }

    public virtual Location LocationsLocation { get; set; } = null!;

    public virtual User UsersUser { get; set; } = null!;
}
