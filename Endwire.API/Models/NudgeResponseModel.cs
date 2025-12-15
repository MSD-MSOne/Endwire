using EndWire.Domain.Models;

namespace EndWire.API
{
    public class NudgeResponseModel
    {
        public List<NudgeDto>? Nudges { get; set; }
        public string APIResponse { get; set; }
    }

    public class NudgeDto
    {
        public int NudgeId { get; set; }
        public string Nudge { get; set; }

    }

}
