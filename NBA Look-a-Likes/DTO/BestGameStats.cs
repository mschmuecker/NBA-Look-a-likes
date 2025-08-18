using NBA_App.Model;

namespace NBA_App.DTO
{
    public class BestGameStats
    {
        public DateTime GameDate { get; set; }  
        public double StatValue { get; set; }
        public bool WasHome { get; set; }
        public string GameType { get; set; }
        public double HomePoints { get; set; }
        public double AwayPoints { get; set; } 
        public Team Home { get; set; }
        public Team Away { get; set; }
    }
}
