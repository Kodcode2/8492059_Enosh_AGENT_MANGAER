namespace AgensRest.Models
{
    public enum AgentStatus
    {
        Dormant, Active
    }
    public class AgentModel
    {
        public int Id { get; set; }
        public string? Nickname { get; set; }
        public string? PhotoUrl { get; set; }
        public int X { get; set; } = -1;
        public int Y { get; set; } = -1;
        public AgentStatus Status { get; set; } = AgentStatus.Dormant;
    }
}