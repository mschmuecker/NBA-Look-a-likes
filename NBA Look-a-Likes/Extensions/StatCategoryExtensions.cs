using NBA_App.Enums;

namespace NBA_App.Extensions
{
    public static class StatCategoryExtensions
    {
        public static string ToSqlValue(this StatCategory stat)
        {
            return stat switch
            {
                StatCategory.PPG => "PPG",
                StatCategory.RPG => "RPG",
                StatCategory.APG => "APG",
                StatCategory.TOPG => "TOPG",
                StatCategory.ThreePM => "3PM",
                StatCategory.FTM => "FTM",
                StatCategory.STL => "STL",
                StatCategory.BLK => "BLK",
                _ => throw new ArgumentOutOfRangeException(nameof(stat))
            };
        }
        public static string ToDisplayName(this StatCategory stat)
        {
            return stat switch
            {
                StatCategory.PPG => "Points",
                StatCategory.RPG => "Rebounds",
                StatCategory.APG => "Assists",
                StatCategory.TOPG => "Turnovers",
                StatCategory.ThreePM => "3PM (Threes Made)",
                StatCategory.FTM => "Free Throws Made",
                StatCategory.STL => "Steals",
                StatCategory.BLK => "Blocks",
                _ => stat.ToString()
            };
        }
    }
}
