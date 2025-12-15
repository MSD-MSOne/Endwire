using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class Reminder
{
    public int ReminderId { get; set; }

    public string Reminder1 { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime DateAdded { get; set; }

    public int UsersUserId { get; set; }

    public virtual ICollection<UserInbox> UserInboxes { get; } = new List<UserInbox>();

    public virtual ICollection<UserOutbox> UserOutboxes { get; } = new List<UserOutbox>();

    public virtual User UsersUser { get; set; } = null!;
}
