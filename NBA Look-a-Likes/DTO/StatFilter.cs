namespace NBA_App.DTO
{
    public class StatFilter
    {
        public double? MinPPG { get; set; }
        public double? MaxTurnovers { get; set; }
        public double? MinThreeP { get; set; }
        public double? MinAPG { get; set; }
        public double? MinRPG { get; set; }
        public double? MinFGP { get; set; }

        public string? TeamAbbrev { get; set; }
        public string? Position { get; set; }
        public int? CollegeID { get; set; }

        public string? SortBy { get; set; } = "PPG";
        public bool SortDescending { get; set; } = true;
    }


}
