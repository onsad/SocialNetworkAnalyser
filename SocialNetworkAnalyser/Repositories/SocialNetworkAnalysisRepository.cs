using Microsoft.EntityFrameworkCore;
using SocialNetworkAnalyser.DAL;
using SocialNetworkAnalyser.Entitites;

namespace SocialNetworkAnalyser.Repositories
{
    public class SocialNetworkAnalysisRepository(SocialNetworkAnalyserContext socialNetworkAnalyserContext, ILogger<SocialNetworkAnalysisRepository> logger) : ISocialNetworkAnalysisRepository
    {
        public List<SocialNetworkAnalysis> GetAll()
        {
            return socialNetworkAnalyserContext.SocialNetworkAnalysis.ToList();
        }

        public SocialNetworkAnalysis? GetById(int id)
        {
            return socialNetworkAnalyserContext.SocialNetworkAnalysis
                .Include(sn => sn.AnalyzedUsers)
                .ThenInclude(sn => sn.SocialNetworkConnectedUsersId)
                .FirstOrDefault(s => s.Id == id);
        }

        public void SaveSocialNetworkAnalysis(string fileName, string nameOfAnalysis, Dictionary<int, List<int>> graphOfFriends)
        {
            try
            {
                using (var transaction = socialNetworkAnalyserContext.Database.BeginTransaction())
                {
                    var usersToSave = new List<AnalyzedUser>();

                    foreach (var user in graphOfFriends)
                    {
                        var userToSave = new AnalyzedUser
                        {
                            SocialNetworkUserId = user.Key,
                            SocialNetworkConnectedUsersId = user.Value.Select(u => new AnalyzedUserToAnalyzedUser { SocialNetworkUserId = user.Key, SocialNetworkConnectedUserId = u }).ToList()
                        };
                        usersToSave.Add(userToSave);
                    }

                    var analysis = new SocialNetworkAnalysis
                    {
                        FileName = fileName,
                        NameOfAnalysis = nameOfAnalysis,
                        AnalyzedUsers = usersToSave,
                        CountOfUsers = usersToSave.Count,
                        AverageCountOfConnectedUsers = graphOfFriends.Sum(u => u.Value.Count) / graphOfFriends.Count
                    };

                    socialNetworkAnalyserContext.SocialNetworkAnalysis.Add(analysis);
                    socialNetworkAnalyserContext.AnalyzedUser.AddRange(usersToSave);

                    socialNetworkAnalyserContext.SaveChanges();
                    transaction.Commit();
                }
            }
            catch (DbUpdateException exception)
            {
                logger.LogError(exception.Message);
            }
        }
    }
}
