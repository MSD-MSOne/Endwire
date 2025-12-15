using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class Apiresponse
{
    public int ApiresponseId { get; set; }

    public string Response { get; set; } = null!;

    public string? StoredProcedure { get; set; }

    public string? ResponseCode { get; set; }
}
