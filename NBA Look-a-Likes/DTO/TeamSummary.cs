using NBA_App.Model;

namespace NBA_App.DTO
{
    public class TeamSummary{
        public Team Team { get; set; } = new();
        public int? GamesPlayed {  get; set; }
        public int? Wins { get; set; } = 0;
        public int? Losses { get; set; } = 0;
        public double? WinPercentage => GamesPlayed == 0 ? null: (double)Wins / GamesPlayed;
        public double? PPG { get; set; } = 0;
        public double? PointsAllowed { get; set; } = 0;
      
        public int? Season { get; set; }
    }
}
