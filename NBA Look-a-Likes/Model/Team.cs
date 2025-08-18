namespace NBA_App.Model
{
    public class Team
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public string TeamAbbrev { get; set; }
        public string TeamCity { get; set; }   
        public string League { get; set; }
        public List<TeamHistory> History { get; set; } = new ();
    }

}
