using AgensRest.Models;

namespace AgensRest.Dto
{
    public class MissionDto
    {
        public AgentModel? Agent { get; set; }
        public TargetModel? Target { get; set; }
        public double TimeRemaining { get; set; }

    }
}
