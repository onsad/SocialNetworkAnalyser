using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAnalyser.Entitites
{
    public class AnalyzedUser
    {
        [Key]
        public int Id { get; set; }

        public SocialNetworkAnalysis? SocialNetworkAnalysis { get; set; }

        [Required]
        public int SocialNetworkUserId { get; set; }

        public List<AnalyzedUserToAnalyzedUser>? SocialNetworkConnectedUsersId { get; set; }
    }
}
