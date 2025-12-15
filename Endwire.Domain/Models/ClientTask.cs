using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class ClientTask
{
    public int ClientTaskId { get; set; }

    public int ClientId { get; set; }

    public int TaskId { get; set; }

    public int TaskInterval { get; set; }

    public int ClientsClientId { get; set; }

    public int TasksTaskId { get; set; }

    public virtual Client ClientsClient { get; set; } = null!;

    public virtual Task TasksTask { get; set; } = null!;
}
