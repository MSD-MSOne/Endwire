using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class Session
{
    public int SessionId { get; set; }

    public int UserId { get; set; }

    public string RegToken { get; set; } = null!;

    public string AuthToken { get; set; } = null!;

    public DateTime DateAdded { get; set; }

    public DateTime? DateEnded { get; set; }

    public int? LocationId { get; set; }

    public int? LocationsLocationId { get; set; }

    public virtual Location? LocationsLocation { get; set; }
}
