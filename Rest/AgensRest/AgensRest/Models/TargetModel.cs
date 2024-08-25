using AgensRest.Models;

namespace AgensRest.Models
{
    public enum TargetStatus
    {
        Alive, Hunted, Eliminated
    }

    public class TargetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Image { get; set; }
        public int X { get; set; } = -1;
        public int Y { get; set; } = -1;
        public TargetStatus Status { get; set; } = TargetStatus.Alive;

    }

}


