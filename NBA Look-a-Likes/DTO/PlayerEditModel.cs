using System.ComponentModel.DataAnnotations;

namespace NBA_App.DTO
{
    public class PlayerEditModel
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; } = "";

        [Required, StringLength(50)]
        public string LastName { get; set; } = "";

        public string? Position { get; set; }
        public string? College { get; set; }

        [Range(1947, 2100, ErrorMessage = "Draft year must be 1947 or later")]
        public double? DraftYear { get; set; }

        [Range(1, 2, ErrorMessage = "Draft round must be between 1 and 2")]
        public double? DraftRound { get; set; }

        [Range(1, 30, ErrorMessage = "Draft number must be between 1 and 30")]
        public double? DraftNumber { get; set; }

        [Range(4, 8, ErrorMessage = "Feet must be between 4 and 8")]
        public double? HeightFeet { get; set; }

       
        [Range(0, 11, ErrorMessage = "Inches must be between 0 and 11")]
        public double? HeightInches { get; set; }

        [Range(80, 400, ErrorMessage = "Weight must be between 80–400 lbs")]
        public double? Weight { get; set; }
    }
    
}
