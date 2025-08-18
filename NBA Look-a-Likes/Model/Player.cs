using NBA_App.DTO;

namespace NBA_App.Model
{
    public class Player
    {
        public int PlayerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();

        public DateTime BirthDate { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public double? DraftYear { get; set; }
        public double? DraftRound { get; set; }
        public double? DraftNumber { get; set; }
        public string Position { get; set; }
        public int? CollegeID { get; set; }
        public string LastCollege {  get; set; }
        

        public College? College { get; set; } // optional nav property

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

