using NBA_App.Model;

namespace NBA_App.DTO
{
    public class TeamSummary{
        public Team Team { get; set; } = new();
        public int? GamesPlayed {  get; set; }
        public int? Wins { get; set; } = 0;
        public int? Losses { get; set; } = 0;
        public double? WinPercentage { get => Wins / GamesPlayed; }
        public double? PPG { get; set; } = 0;
        public double? APG { get; set; } = 0;
        public double? RPG { get; set; } = 0;
        public short? Season { get; set; }
    }
}
