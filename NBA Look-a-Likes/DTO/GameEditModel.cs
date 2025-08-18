using Microsoft.Identity.Client;
using NBA_App.Model;

namespace NBA_App.DTO
{
    // DTO/GameEditModel.cs
    public class GameEditModel
    {
        public int? GameId { get; set; }       // null = new
        public DateTime GameDate { get; set; } = DateTime.Today;
        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; } = new();
        public Team AwayTeam { get; set; } = new();
        public int AwayTeamId { get; set; }
        public int HomeScore { get; set; } = 0;
        public int AwayScore { get; set; } = 0;
        public int? Winner { get; set; }         // "HOME" | "AWAY"
        public string GameType { get; set; } = "Regular Season"; // REG, PLAYOFF, PRE
        public double? Attendance { get; set; } = 0;
        public int? ArenaId { get; set; }
        public string? GameLabel { get; set; }
        public string? GameSubLabel { get; set; }
        public int? SeriesGameNumber { get; set; }

        public List<PlayerGameRow> Players { get; set; } = new();
    }

    public class PlayerGameRow
    {
        public int PersonId { get; set; } = 0;
        public Player Player { get; set; } = new();

        public Team Team { get; set; } = new();
        public double Minutes { get; set; } = 0;
        public bool Win {  get; set; }= false;
        public bool Home { get; set; }=false;
        public double Points { get; set; } = 0;
        public double ReboundsTotal { get; set; } = 0;
        public double Assists { get; set; } = 0;
        public double Steals { get; set; } = 0;
        public double Blocks { get; set; } = 0;
        public double Turnovers { get; set; } = 0;
        public double FGM { get; set; } = 0;
        public double FGA { get; set; } = 0;
        public double TPM { get; set; } = 0;       // threePointersMade
        public double TPA { get; set; } = 0;
        public double FTM { get; set; } = 0;
        public double FTA { get; set; } = 0;
        // add the stats you track
    }

}
