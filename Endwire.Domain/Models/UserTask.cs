using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class UserTask
{
    public int UserTaskId { get; set; }

    public int UserId { get; set; }

    public int TaskId { get; set; }

    public float TaskInterval { get; set; }

    public int TaskStatusId { get; set; }

    public int DateAdded { get; set; }
}
