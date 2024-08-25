using Microsoft.Identity.Client;

namespace AgensRest.Models
{
    public enum MissionStatus
    {
        Proposal, Active ,Completed
    }
    public class MissionModel
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int TargetId { get; set; }
        public AgentModel Agent { get; set; }
        public TargetModel Target { get; set; }
        public double RemainingTime { get; set; }
        public DateTime StarTime { get; set; }
        public MissionStatus Status { get; set; } = MissionStatus.Proposal;
    

    }

}


