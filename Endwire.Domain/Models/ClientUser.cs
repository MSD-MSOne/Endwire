using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class ClientUser
{
    public int ClientUserId { get; set; }

    public int ClientId { get; set; }

    public int UserId { get; set; }

    public int ClientsClientId { get; set; }

    public int UsersUserId { get; set; }

    public virtual Client ClientsClient { get; set; } = null!;

    public virtual User UsersUser { get; set; } = null!;
}
