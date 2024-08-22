namespace AgensRest.Models
{
    public enum TargetStatus
    {
        Alive, Eliminated
    }

    public class TargetModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public string? PhotoUrl { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public TargetStatus Status { get; set; } = TargetStatus.Alive;
    }

}

