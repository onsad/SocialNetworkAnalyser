using SocialNetworkAnalyser.DAL;
using SocialNetworkAnalyser.Entitites;

namespace SocialNetworkAnalyser.Services
{
    public class AnalysisService(SocialNetworkAnalyserContext socialNetworkAnalyserContext) : IAnalysisService
    {
        public List<SocialNetworkAnalysis> GetSocialNetworkAnalysis()
        {
            return socialNetworkAnalyserContext.SocialNetworkAnalysis.ToList();
        }

        public void SaveSocialNetworkAnalysis(SocialNetworkAnalysis analysis)
        {
            throw new NotImplementedException();
        }
    }
}
