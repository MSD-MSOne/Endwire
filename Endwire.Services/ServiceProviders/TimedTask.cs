using EndWire.Domain.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Services.ServiceProviders
{
    public static class TimedTask
    {
        public static ConcurrentQueue<List<ChangeReminderStatusFcmResponse>> CQ = new ConcurrentQueue<List<ChangeReminderStatusFcmResponse>>();
    }
}
