using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class Apilist
{
    public int ApilistId { get; set; }

    public string? Apicode { get; set; }

    public string? Apiname { get; set; }

    public string? ApiSp { get; set; }
}
