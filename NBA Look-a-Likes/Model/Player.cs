using NBA_App.DTO;

namespace NBA_App.Model
{
    public class Player
    {
        public int PlayerID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();

        public DateTime BirthDate { get; set; }
        public double Weight { get; set; } = 0;
        public double Height { get; set; } = 0;
        public double? DraftYear { get; set; } = 0;
        public double? DraftRound { get; set; } = 0;
        public double? DraftNumber { get; set; } = 0;
        public string? Position { get; set; }
        public int? CollegeID { get; set; }
        public string? LastCollege {  get; set; }
        

        public StatSummary CareerStatSummary { get; set; } = new();
        public List<StatSummary> SeasonStatSummary { get; set; } = new();

        public string HeightFormatted
        {
            get
            {
                int totalInches = (int)Math.Round(Height);
                int feet = totalInches / 12;
                int inches = totalInches % 12;
                return $"{feet}' {inches}\"";
            }
        }
        public string WeightFormatted
        {
            get
            {
                return Weight + "lbs";
            }
        }
    }
}

