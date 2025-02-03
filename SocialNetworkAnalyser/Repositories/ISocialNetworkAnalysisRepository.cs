using SocialNetworkAnalyser.Entitites;

namespace SocialNetworkAnalyser.Repositories
{
    public interface ISocialNetworkAnalysisRepository
    {
        /// <summary>
        /// Get all social network analysis.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SocialNetworkAnalysis> GetAll();

        /// <summary>
        /// Get social network analysis by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SocialNetworkAnalysis? GetById(int id);

        /// <summary>
        /// Save social network analysis.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="nameOfAnalysis"></param>
        /// <param name="graphOfFriends"></param>
        public void SaveSocialNetworkAnalysis(string fileName, string nameOfAnalysis, Dictionary<int, List<int>> graphOfFriends);
    }
}
