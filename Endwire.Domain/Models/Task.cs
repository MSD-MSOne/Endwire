using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public string Task1 { get; set; } = null!;

    public int TaskType { get; set; }

    public int DefaultTaskinterval { get; set; }

    public virtual ICollection<ClientTask> ClientTasks { get; } = new List<ClientTask>();
}
