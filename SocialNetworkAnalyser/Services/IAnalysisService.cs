using SocialNetworkAnalyser.Entitites;

namespace SocialNetworkAnalyser.Services
{
    public interface IAnalysisService
    {
        public List<SocialNetworkAnalysis> GetSocialNetworkAnalysis();

        public void SaveSocialNetworkAnalysis(SocialNetworkAnalysis analysis);
    }
}
