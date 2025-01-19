using SocialNetworkAnalyser.Entitites;

namespace SocialNetworkAnalyser.Services
{
    public interface IAnalysisService
    {
        public IEnumerable<SocialNetworkAnalysis> GetAllSocialNetworkAnalysis();

        public SocialNetworkAnalysis? GetSocialNetworkAnalysis(int idOfAnalysis);

        public bool SaveSocialNetworkAnalysis(List<string> linesFromFile, string nameOfAnalsis, string fileName);

        public Task<List<string>> GetLinesFromInputFile(IFormFile file);
    }
}
