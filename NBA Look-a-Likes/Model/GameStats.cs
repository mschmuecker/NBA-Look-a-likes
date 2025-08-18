namespace NBA_App.Model
{
    public class GameStats
    {
        public int GameID { get; set; }
        public int PlayerID { get; set; }
        public int PlayerTeamID { get; set; }
        public int OpponentTeamID { get; set; }

        public int NumMins { get; set; }
        public int FieldGoalsMade { get; set; }
        public int FieldGoalsAttempted { get; set; }
        public double FieldGoalsPercentage { get; set; }

        public int ThreePointersMade { get; set; }
        public int ThreePointersAttempted { get; set; }
        public double ThreePointersPercentage { get; set; }

        public int FreeThrowsMade { get; set; }
        public int FreeThrowsAttempted { get; set; }
        public double FreeThrowsPercentage { get; set; }

        public int OffensiveRebounds { get; set; }
        public int DefensiveRebounds { get; set; }
        public int ReboundsTotal { get; set; }

        public int Assists { get; set; }
        public int Steals { get; set; }
        public int Blocks { get; set; }
        public int Turnovers { get; set; }
        public int FoulsPersonal { get; set; }
        public int PlusMinusPoints { get; set; }
    }

}
