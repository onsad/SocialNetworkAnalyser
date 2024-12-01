using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAnalyser.Entitites
{
    public class AnalyzedUserToAnalyzedUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SocialNetworkUserId { get; set; }

        [Required]
        public int SocialNetworkConnectedUserId { get; set; }
    }
}
