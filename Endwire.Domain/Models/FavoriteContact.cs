using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class FavoriteContact
{
    public int FavoriteContactId { get; set; }

    public int UserId { get; set; }

    public int FavoriteContactUserId { get; set; }

    public DateTime DateAdded { get; set; }

    public int UsersUserId { get; set; }

    public virtual User UsersUser { get; set; } = null!;
}
