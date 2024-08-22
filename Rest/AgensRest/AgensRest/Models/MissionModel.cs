namespace AgensRest.Models
{
    public enum MissionStatus
    {
        proposal, task ,ended
    }
    public class MissionModel
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int TargetId { get; set; }
        public AgentModel? Agent { get; set; }
        public TargetModel? Target { get; set; }
        public double TimeRemaining { get; set; }
        public MissionStatus Status { get; set; } = MissionStatus.proposal;

    }

}


