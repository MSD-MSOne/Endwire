using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class RegistrationToken
{
    public int RegistrationTokenId { get; set; }

    public string RegistrationToken1 { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime DateAdded { get; set; }
}
