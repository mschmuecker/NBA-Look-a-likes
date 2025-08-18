namespace NBA_App.DTO
{
    public class GameSummary
    {

        public int GameId { get; set; }

        public DateTime GameDate { get; set; }

        public string HomeTeamCity { get; set; } = string.Empty;
        public string HomeTeamName { get; set; } = string.Empty;
        public int HomeTeamId { get; set; }

        public string AwayTeamCity { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
        public int AwayTeamId { get; set; }

        public short HomeScore { get; set; }
        public short AwayScore { get; set; }

        public int Winner { get; set; }  

        public string GameType { get; set; } = "REG"; // e.g., "REG", "PLAYOFF"

        public double? Attendance { get; set; }

        public int? ArenaId { get; set; }

      
        public string DisplayScore => $"{HomeScore} - {AwayScore}";
        public string GameLabel => $"{HomeTeamCity} {HomeTeamName} vs. {AwayTeamCity} {AwayTeamName}";
    }
}
