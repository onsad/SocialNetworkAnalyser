using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using SocialNetworkAnalyser.DAL;
using SocialNetworkAnalyser.Entitites;
using SocialNetworkAnalyser.Repositories;

namespace SocialNetworkAnalyserTest
{
    public class SocialNetworkAnalysisRepositoryTests
    {
        ISocialNetworkAnalysisRepository? socialNetworkAnalysisRepository;
        SocialNetworkAnalyserContext apiContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SocialNetworkAnalyserContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            apiContext = new SocialNetworkAnalyserContext(options);
            socialNetworkAnalysisRepository = new SocialNetworkAnalysisRepository(apiContext, NullLogger<SocialNetworkAnalysisRepository>.Instance);
        }

        [Test]
        public void SaveSocialNetworkAnalysisTest()
        {
            socialNetworkAnalysisRepository?.SaveSocialNetworkAnalysis("fileName", "TestAnalysis", new Dictionary<int, List<int>> { { 1, new List<int> { 2, 3 } } });    

            var socialNetworkAnalysis = socialNetworkAnalysisRepository?.GetAll().FirstOrDefault();

            if (socialNetworkAnalysis != null)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(socialNetworkAnalysis.FileName, Is.EqualTo("fileName"));
                    Assert.That(socialNetworkAnalysis.NameOfAnalysis, Is.EqualTo("TestAnalysis"));
                    Assert.That(socialNetworkAnalysis.CountOfUsers, Is.EqualTo(1));
                    Assert.That(socialNetworkAnalysis.AverageCountOfConnectedUsers, Is.EqualTo(2));
                });
            }
        }

        [TearDown]
        public void TearDown()
        {
            apiContext.Database.EnsureDeleted();
            socialNetworkAnalysisRepository = null;
        }
    }
}
