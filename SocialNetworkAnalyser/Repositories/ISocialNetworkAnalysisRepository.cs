using SocialNetworkAnalyser.Entitites;

namespace SocialNetworkAnalyser.Repositories
{
    public interface ISocialNetworkAnalysisRepository
    {
        public IEnumerable<SocialNetworkAnalysis> GetAll();
        public SocialNetworkAnalysis? GetById(int id);
        public void SaveSocialNetworkAnalysis(string fileName, string nameOfAnalysis, Dictionary<int, List<int>> graphOfFriends);
    }
}
