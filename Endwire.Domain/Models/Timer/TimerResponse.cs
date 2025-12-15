using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models.Timer
{
    public class SetTimerResponse
    {
        public string Action { get; set; } = string.Empty;
        public string FCM_Message { get; set; } = string.Empty;
        public string TaskInterval { get; set; } = string.Empty;
        public string RegToken { get; set; } = string.Empty;
        public int UserTaskId { get; set; } 
        public string Response { get; set; } = string.Empty;
        public string APIResponse { get; set; } = string.Empty;
    }

    public class SetTimerResponseModel
    {
        
        public string status { get; set; } = string.Empty;
        public string APIResponse { get; set; } = string.Empty;
    }

    public class TimerTaskResponse
    {
        public int TaskId   { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }
        public string APIResponse { get; set; }
    }

    public class TimerTask
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }
    }

    public class TimerTaskResponseModel
    {
        public List<TimerTask>? Tasks { get; set; } = new List<TimerTask>(); 
        public string? APIResponse { get; set; }
    }
}
