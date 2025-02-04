using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAnalyser.Entitites
{
    public class SocialNetworkAnalysis
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name of analysis")]
        [Required]
        public required string NameOfAnalysis { get; set; }

        [Display(Name = "File name")]
        [Required]
        public required string FileName { get; set; }

        [Display(Name = "Total count of users")]
        public int CountOfUsers { get; set; }

        [Display(Name = "Average count of friends of user")]
        public double AverageCountOfConnectedUsers { get; set; }

        [Required]
        public required ICollection<AnalyzedUser> AnalyzedUsers { get; set; }   
    }
}
