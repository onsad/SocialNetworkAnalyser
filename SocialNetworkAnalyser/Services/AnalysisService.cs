using SocialNetworkAnalyser.Entitites;
using SocialNetworkAnalyser.Models;
using SocialNetworkAnalyser.Repositories;

namespace SocialNetworkAnalyser.Services
{
    public class AnalysisService(ISocialNetworkAnalysisRepository socialNetworkAnalysisRepository) : IAnalysisService
    {
        public List<SocialNetworkAnalysis> GetAllSocialNetworkAnalysis()
        {
            return socialNetworkAnalysisRepository.GetAll();
        }

        public bool SaveInputData(List<string> linesFromFile, string nameOfAnalysis, string fileName)
        {
            var listOfUsersAndFriends = new List<UserFromFile>();

            foreach (var line in linesFromFile)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var splittedLine = line.Split(' ');

                    if (int.TryParse(splittedLine[0].Trim(), out var userId) && int.TryParse(splittedLine[1].Trim(), out var userFriendId))
                    {
                        listOfUsersAndFriends.Add(new() { UserId = userId, UserFriendId = userFriendId });
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            var graphOfFriends = CreateGraphOfFriends(listOfUsersAndFriends);

            socialNetworkAnalysisRepository.SaveSocialNetworkAnalysis(fileName, nameOfAnalysis, graphOfFriends);

            return true;
        }

        public SocialNetworkAnalysis? GetSocialNetworkAnalysis(int idOfAnalysis)
        {
            var analysis =  socialNetworkAnalysisRepository.GetById(idOfAnalysis);
            var pat = GetLargerstPathInTheNetwork(analysis);
            return analysis;
        }

        private Dictionary<int, List<int>> CreateGraphOfFriends(List<UserFromFile> userFromFile)
        {
            var friendsDict = new Dictionary<int, List<int>>();

            foreach (var pairOfFriends in userFromFile)
            {
                if (!friendsDict.TryGetValue(pairOfFriends.UserId, out var listOfFriends))
                {
                    friendsDict[pairOfFriends.UserId] = new List<int> { pairOfFriends.UserFriendId };
                }
                else
                {
                    listOfFriends.Add(pairOfFriends.UserFriendId);
                }

                if (!friendsDict.TryGetValue(pairOfFriends.UserFriendId, out var listOfUserFriends))
                {
                    friendsDict[pairOfFriends.UserFriendId] = new List<int> { pairOfFriends.UserId };
                }
                else
                {
                    listOfUserFriends.Add(pairOfFriends.UserId);
                }
            }
            return friendsDict;
        }

        public int GetLargerstPathInTheNetwork(SocialNetworkAnalysis socialNetworkAnalysis)
        {
            var longestPath = 0;
            int[] distances = new int[socialNetworkAnalysis.CountOfUsers];
            var users = socialNetworkAnalysis.AnalyzedUsers
                .ToDictionary(x => x.SocialNetworkUserId, x => x.SocialNetworkConnectedUsersId?.Select(s => s.SocialNetworkConnectedUserId).ToList());

            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = -1;
            }

            Queue<int> q = new Queue<int>();

            foreach (var user in users)
            {
                q.Enqueue(user.Key);
            }

            while (q.Count != 0)
            {
                int t = q.Dequeue();

                for (int i = 0; i < users[t]?.Count; ++i)
                {
                    int v = users[t][i];

                    if (distances[v] == -1)
                    {
                        q.Enqueue(v);

                        distances[v] = distances[t] + 1;
                    }
                }
            }

            int nodeIdx = 0;

            for (int i = 0; i < users.Count; ++i)
            {
                if (distances[i] > longestPath)
                {
                    longestPath = distances[i];
                    nodeIdx = i;
                }
            }

            return longestPath;
        }
    }
}
