using EndWire.Domain.Models;

namespace EndWire.API
{
    public class ReminderResponseModel
    {
        public List<ReminderResult> Reminders { get; set; }
        public string APIResponse { get; set; }
    }
}
