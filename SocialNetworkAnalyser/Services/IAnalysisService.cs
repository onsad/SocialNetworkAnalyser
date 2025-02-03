using SocialNetworkAnalyser.Entitites;

namespace SocialNetworkAnalyser.Services
{
    public interface IAnalysisService
    {
        /// <summary>
        /// Get all social network analysis.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SocialNetworkAnalysis> GetAllSocialNetworkAnalysis();

        /// <summary>
        /// Get social network analysis by id.
        /// </summary>
        /// <param name="idOfAnalysis"></param>
        /// <returns></returns>
        public SocialNetworkAnalysis? GetSocialNetworkAnalysis(int idOfAnalysis);

        /// <summary>
        /// Save social network analysis.
        /// </summary>
        /// <param name="linesFromFile"></param>
        /// <param name="nameOfAnalsis"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool SaveSocialNetworkAnalysis(List<string> linesFromFile, string nameOfAnalsis, string fileName);

        /// <summary>
        /// Get lines from input file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public Task<List<string>> GetLinesFromInputFile(IFormFile file);
    }
}
