using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAnalyser.Entitites
{
    public class AnalyzedUser
    {
        [Key]
        public int Id { get; set; }

        public required SocialNetworkAnalysis SocialNetworkAnalysis { get; set; }

        [Required]
        public int SocialNetworkUserId { get; set; }

        [Required]
        public int SocialNetworkConnectedUserId { get; set; }
    }
}
