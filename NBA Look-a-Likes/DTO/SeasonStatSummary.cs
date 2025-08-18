namespace NBA_App.DTO
{
    public class SeasonStatSummary
    {
        public string Season { get; set; } = string.Empty;
        public int Games { get; set; }
        public double PPG { get; set; }
        public double RPG { get; set; }
        public double APG { get; set; }
    }

}
