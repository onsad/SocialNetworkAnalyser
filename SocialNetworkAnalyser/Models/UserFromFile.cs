namespace SocialNetworkAnalyser.Models
{
    /// <summary>
    /// Represents one line from input file.
    /// </summary>
    public class UserFromFile
    {
        /// <summary>
        /// Identifier of user.
        /// </summary>
        public int UserId { get; set; } 

        /// <summary>
        /// Identifier of user's friend.
        /// </summary>
        public int UserFriendId { get; set; }
    }
}
