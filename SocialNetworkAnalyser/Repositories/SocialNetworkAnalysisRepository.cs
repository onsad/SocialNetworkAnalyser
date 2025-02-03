using Microsoft.EntityFrameworkCore;
using SocialNetworkAnalyser.DAL;
using SocialNetworkAnalyser.Entitites;

namespace SocialNetworkAnalyser.Repositories
{
    public class SocialNetworkAnalysisRepository(SocialNetworkAnalyserContext socialNetworkAnalyserContext, ILogger<SocialNetworkAnalysisRepository> logger) : ISocialNetworkAnalysisRepository
    {
        /// <inheritdoc />
        public IEnumerable<SocialNetworkAnalysis> GetAll()
        {
            return socialNetworkAnalyserContext.SocialNetworkAnalysis.ToArray();
        }

        /// <inheritdoc />
        public SocialNetworkAnalysis? GetById(int id)
        {
            return socialNetworkAnalyserContext.SocialNetworkAnalysis.Find(id);
        }

        /// <inheritdoc />
        public void SaveSocialNetworkAnalysis(string fileName, string nameOfAnalysis, Dictionary<int, List<int>> graphOfFriends)
        {
            using (var transaction = socialNetworkAnalyserContext.Database.BeginTransaction())
            {
                try
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
                        AverageCountOfConnectedUsers = Math.Round((double)graphOfFriends.Sum(u => u.Value.Count) / (double)graphOfFriends.Count, 1)
                    };

                    socialNetworkAnalyserContext.SocialNetworkAnalysis.Add(analysis);
                    socialNetworkAnalyserContext.AnalyzedUser.AddRange(usersToSave);

                    socialNetworkAnalyserContext.SaveChanges();
                    transaction.Commit();
                }
                catch (DbUpdateException exception)
                {
                    transaction.Rollback();
                    logger.LogError(exception.Message);

                    throw;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    logger.LogError(exception.Message);

                    throw;
                }
            }
        }
    }
}
