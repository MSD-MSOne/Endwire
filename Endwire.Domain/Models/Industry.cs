using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class Industry
{
    public int IndustryId { get; set; }

    public string Industry1 { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; } = new List<Client>();
}
