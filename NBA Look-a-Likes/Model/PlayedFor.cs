namespace NBA_App.Model
{
    public class PlayedFor
    {
        public Player Player { get; set; } = new();
        public Team Team  { get; set; } = new();
        public short StartSeason { get; set; } = new();
        public short EndSeason { get; set; } = new();
    }

}
