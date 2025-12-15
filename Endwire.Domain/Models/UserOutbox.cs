using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class UserOutbox
{
    public int UserOutboxId { get; set; }

    public int UserId { get; set; }

    public int ReminderId { get; set; }

    public int NudgeId { get; set; }

    public DateTime DateAdded { get; set; }

    public DateTime DateRemoved { get; set; }

    public int UsersUserId { get; set; }

    public int NudgesNudgeId { get; set; }

    public int RemindersReminderId { get; set; }

    public virtual Nudge NudgesNudge { get; set; } = null!;

    public virtual Reminder RemindersReminder { get; set; } = null!;

    public virtual User UsersUser { get; set; } = null!;
}
