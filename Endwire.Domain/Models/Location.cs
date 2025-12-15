using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string Location1 { get; set; } = null!;

    public int ClientId { get; set; }

    public string Street1 { get; set; } = null!;

    public string Street2 { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string? City { get; set; }

    public string State { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public int ClientsClientId { get; set; }

    public virtual Client ClientsClient { get; set; } = null!;

    public virtual ICollection<Resource> Resources { get; } = new List<Resource>();

    public virtual ICollection<Session> Sessions { get; } = new List<Session>();

    public virtual ICollection<UserLocation> UserLocations { get; } = new List<UserLocation>();
}
