using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class SystemDefault
{
    public int SystemDefaultId { get; set; }

    public int? MaxFailedLogins { get; set; }
}
