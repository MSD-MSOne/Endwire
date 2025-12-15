using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public string Client1 { get; set; } = null!;

    public int IndustryId { get; set; }

    public int IndustryIndustryId { get; set; }

    public virtual ICollection<ClientTask> ClientTasks { get; } = new List<ClientTask>();

    public virtual ICollection<ClientUser> ClientUsers { get; } = new List<ClientUser>();

    public virtual Industry IndustryIndustry { get; set; } = null!;

    public virtual ICollection<Location> Locations { get; } = new List<Location>();
}
