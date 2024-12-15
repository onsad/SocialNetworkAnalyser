using SocialNetworkAnalyser.Entitites;

namespace SocialNetworkAnalyser.Services
{
    public interface IAnalysisService
    {
        public List<SocialNetworkAnalysis> GetAllSocialNetworkAnalysis();

        public SocialNetworkAnalysis? GetSocialNetworkAnalysis(int idOfAnalysis);

        public bool SaveInputData(List<string> linesFromFile, string nameOfAnalsis, string fileName);
    }
}
