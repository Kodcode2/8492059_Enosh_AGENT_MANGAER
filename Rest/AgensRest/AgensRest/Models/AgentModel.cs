namespace AgensRest.Models
{
    public enum AgentStatus
    {
        Dormant, Active
    }
    public class AgentModel
    {
        public int Id { get; set; }
        public required string? Nickname { get; set; }
        public required string? Image { get; set; }
        public int X { get; set; } = -1;
        public int Y { get; set; } = -1;
        public List<MissionModel> Missions { get; set; } = [];

        public AgentStatus Status { get; set; } = AgentStatus.Dormant;
    }
}