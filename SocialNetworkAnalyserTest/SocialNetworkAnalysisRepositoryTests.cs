using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using SocialNetworkAnalyser.DAL;
using SocialNetworkAnalyser.Repositories;

namespace SocialNetworkAnalyserTest
{
    public class SocialNetworkAnalysisRepositoryTests
    {
        ISocialNetworkAnalysisRepository? socialNetworkAnalysisRepository;
        SocialNetworkAnalyserContext socialNetworkAnalyserContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SocialNetworkAnalyserContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            socialNetworkAnalyserContext = new SocialNetworkAnalyserContext(options);
            socialNetworkAnalysisRepository = new SocialNetworkAnalysisRepository(socialNetworkAnalyserContext, NullLogger<SocialNetworkAnalysisRepository>.Instance);
        }

        [Test]
        public void SaveSocialNetworkAnalysisSaveTest()
        {
            var graphOfUsersDict = new Dictionary<int, List<int>>
            {
                { 1, new List<int> { 2, 3 } },
                { 2, new List<int> { 3 } },
                { 3, new List<int> { 4 } }
            };
            socialNetworkAnalysisRepository?.SaveSocialNetworkAnalysis("fileName", "TestAnalysis", graphOfUsersDict);    

            var socialNetworkAnalysis = socialNetworkAnalysisRepository?.GetAll();

            if (socialNetworkAnalysis != null)
            {
                var firstItem = socialNetworkAnalysis.First();

                Assert.Multiple(() =>
                {
                    Assert.That(socialNetworkAnalysis.Count, Is.EqualTo(1));
                    Assert.That(firstItem.FileName, Is.EqualTo("fileName"));
                    Assert.That(firstItem.NameOfAnalysis, Is.EqualTo("TestAnalysis"));
                    Assert.That(firstItem.CountOfUsers, Is.EqualTo(3));
                    Assert.That(firstItem.AverageCountOfConnectedUsers, Is.EqualTo(1.3));
                });
            }
        }

        [Test]
        public void SaveSocialNetworkAnalysisGetAllTest()
        {
            var graphOfUsersDict1 = new Dictionary<int, List<int>>
            {
                { 1, new List<int> { 2, 3 } },
                { 2, new List<int> { 1, 3 } },
                { 3, new List<int> { 1, 2 } }
            };

            var graphOfUsersDict2 = new Dictionary<int, List<int>>
            {
                { 1, new List<int> { 2, 3, 5, 6, 7 } },
                { 2, new List<int> { 1, 3, 585, 12 } },
                { 3, new List<int> { 12, 4} },
                { 5, new List<int> { 36, 6, 7 } },
                { 6, new List<int> { 12, 8, 10, 122 } },
                { 7, new List<int> { 2, 12, 8, 10 } },
                { 8, new List<int> { 1, 2, 12, 8, 36, 45 } },
                { 10, new List<int> { 3, 12, 36 } },
                { 12, new List<int> { 1, 2, 12, 8, 10, 12, 36, 45 } },
                { 36, new List<int> { 3, 5, 6 } },
                { 45, new List<int> { 2, 585, 36 } },
                { 585, new List<int> { 1 } },
            };

            socialNetworkAnalysisRepository?.SaveSocialNetworkAnalysis("fileName1", "TestAnalysis1", graphOfUsersDict1);
            socialNetworkAnalysisRepository?.SaveSocialNetworkAnalysis("fileName2", "TestAnalysis2", graphOfUsersDict2);

            var socialNetworkAnalysis = socialNetworkAnalysisRepository?.GetAll();

            if (socialNetworkAnalysis != null)
            {
                var firstItem = socialNetworkAnalysis.First();
                var lastItem = socialNetworkAnalysis.Last();

                Assert.Multiple(() =>
                {
                    Assert.That(socialNetworkAnalysis.Count, Is.EqualTo(2));
                    Assert.That(firstItem.FileName, Is.EqualTo("fileName1"));
                    Assert.That(firstItem.NameOfAnalysis, Is.EqualTo("TestAnalysis1"));
                    Assert.That(firstItem.CountOfUsers, Is.EqualTo(3));
                    Assert.That(firstItem.AverageCountOfConnectedUsers, Is.EqualTo(2));

                    Assert.That(lastItem.FileName, Is.EqualTo("fileName2"));
                    Assert.That(lastItem.NameOfAnalysis, Is.EqualTo("TestAnalysis2"));
                    Assert.That(lastItem.CountOfUsers, Is.EqualTo(12));
                    Assert.That(lastItem.AverageCountOfConnectedUsers, Is.EqualTo(3.8));
                });
            }
        }

        [TearDown]
        public void TearDown()
        {
            socialNetworkAnalyserContext.Database.EnsureDeleted();
            socialNetworkAnalysisRepository = null;
        }
    }
}
