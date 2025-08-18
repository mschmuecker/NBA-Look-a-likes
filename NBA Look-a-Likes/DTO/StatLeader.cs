using NBA_App.Model;

namespace NBA_App.DTO
{
    public class StatLeader
    {
        public Player Player { get; set; } = new();
        public double Stat { get; set; } = new();
    }
}
