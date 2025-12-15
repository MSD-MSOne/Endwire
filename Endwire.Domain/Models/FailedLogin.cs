using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class FailedLogin
{
    public int FailedLoginId { get; set; }

    public int? UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserPassword { get; set; }

    public DateTime DateAdded { get; set; }
}
