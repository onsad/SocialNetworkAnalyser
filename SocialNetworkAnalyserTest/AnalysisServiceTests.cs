using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SocialNetworkAnalyser.DAL;
using SocialNetworkAnalyser.Repositories;
using SocialNetworkAnalyser.Services;
using System.Text;

namespace SocialNetworkAnalyserTest
{
    public class AnalysisServiceTests
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
        public void GetLinesFromInputFileTest()
        {
            Mock<ISocialNetworkAnalysisRepository> mock = new Mock<ISocialNetworkAnalysisRepository>();
            var service = new AnalysisService(mock.Object);

            byte[] filebytes = Encoding.UTF8.GetBytes($"0 1{Environment.NewLine}1 2");
            IFormFile file = new FormFile(new MemoryStream(filebytes), 0, filebytes.Length, "Data", "data.txt");

            var expectedLines = new List<string> { "0 1", "1 2" };
            var resultLines = service.GetLinesFromInputFile(file);

            for (int i = 0; i < expectedLines.Count; i++)
            {
                Assert.That(resultLines.Result[i], Is.EqualTo(expectedLines[i]));
            }
        }

        [Test]
        public void GetLinesFromInputFileEmptyFileTest()
        {
            Mock<ISocialNetworkAnalysisRepository> mock = new Mock<ISocialNetworkAnalysisRepository>();
            var service = new AnalysisService(mock.Object);

            byte[] filebytes = Encoding.UTF8.GetBytes(string.Empty);
            IFormFile file = new FormFile(new MemoryStream(filebytes), 0, filebytes.Length, "Data", "data.txt");

            var resultLines = service.GetLinesFromInputFile(file);

            Assert.That(resultLines.Result, Is.Empty);
        }

        [Test]
        public void SaveAnalysisTest()
        {
            if (socialNetworkAnalysisRepository != null)
            {
                var service = new AnalysisService(socialNetworkAnalysisRepository);

                byte[] filebytes = Encoding.UTF8.GetBytes($"0 1{Environment.NewLine}1 2");
                IFormFile file = new FormFile(new MemoryStream(filebytes), 0, filebytes.Length, "Data", "data.txt");

                var linesFromInputFile = service.GetLinesFromInputFile(file);
                var result = service.SaveSocialNetworkAnalysis(linesFromInputFile.Result, "TestAnalysis", "fileName");

                var analysis = socialNetworkAnalysisRepository.GetAll().First();
                Assert.Multiple(() =>
                {
                    Assert.That(analysis.NameOfAnalysis, Is.EqualTo("TestAnalysis"));
                    Assert.That(analysis.FileName, Is.EqualTo("fileName"));
                    Assert.That(analysis.AverageCountOfConnectedUsers, Is.EqualTo(1.3));
                    Assert.That(analysis.CountOfUsers, Is.EqualTo(3));
                });
            }
        }

        [Test]
        public void GetByIdAnalysisTest()
        {
            if (socialNetworkAnalysisRepository != null)
            {
                var service = new AnalysisService(socialNetworkAnalysisRepository);

                byte[] filebytes = Encoding.UTF8.GetBytes($"0 1{Environment.NewLine}1 2");
                IFormFile file = new FormFile(new MemoryStream(filebytes), 0, filebytes.Length, "Data", "data.txt");

                var linesFromInputFile = service.GetLinesFromInputFile(file);
                service.SaveSocialNetworkAnalysis(linesFromInputFile.Result, "TestAnalysis1", "fileName1");

                var analysis = socialNetworkAnalysisRepository.GetById(1);

                if (analysis != null)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(analysis.NameOfAnalysis, Is.EqualTo("TestAnalysis1"));
                        Assert.That(analysis.FileName, Is.EqualTo("fileName1"));
                        Assert.That(analysis.AverageCountOfConnectedUsers, Is.EqualTo(1.3));
                        Assert.That(analysis.CountOfUsers, Is.EqualTo(3));
                    });
                }
            }
        }

        [Test]
        public void GetByIdAnalysisNullTest()
        {
            if (socialNetworkAnalysisRepository != null)
            {
                var analysis = socialNetworkAnalysisRepository.GetById(1);
                Assert.That(analysis, Is.Null);
            }
        }

        [Test]
        public void GetAllAnalysisTest()
        {
            if (socialNetworkAnalysisRepository != null)
            {
                var service = new AnalysisService(socialNetworkAnalysisRepository);

                byte[] filebytes = Encoding.UTF8.GetBytes($"0 1{Environment.NewLine}1 2");
                IFormFile file = new FormFile(new MemoryStream(filebytes), 0, filebytes.Length, "Data", "data.txt");

                var linesFromInputFile = service.GetLinesFromInputFile(file);
                service.SaveSocialNetworkAnalysis(linesFromInputFile.Result, "TestAnalysis1", "fileName1");

                service.SaveSocialNetworkAnalysis(linesFromInputFile.Result, "TestAnalysis2", "fileName2");

                var analysis = socialNetworkAnalysisRepository.GetAll();

                Assert.That(analysis.Count, Is.EqualTo(2));
                Assert.Multiple(() =>
                {
                    Assert.That(analysis.First().NameOfAnalysis, Is.EqualTo("TestAnalysis1"));
                    Assert.That(analysis.First().FileName, Is.EqualTo("fileName1"));
                    Assert.That(analysis.First().AverageCountOfConnectedUsers, Is.EqualTo(1.3));
                    Assert.That(analysis.First().CountOfUsers, Is.EqualTo(3));

                    Assert.That(analysis.Last().NameOfAnalysis, Is.EqualTo("TestAnalysis2"));
                    Assert.That(analysis.Last().FileName, Is.EqualTo("fileName2"));
                    Assert.That(analysis.Last().AverageCountOfConnectedUsers, Is.EqualTo(1.3));
                    Assert.That(analysis.Last().CountOfUsers, Is.EqualTo(3));
                });
            }
        }

        [Test]
        public void SaveAnalysisLeftInpuNotNumberTest()
        {
            if (socialNetworkAnalysisRepository != null)
            {
                var service = new AnalysisService(socialNetworkAnalysisRepository);

                byte[] filebytes = Encoding.UTF8.GetBytes($"a 1{Environment.NewLine}1 2");
                IFormFile file = new FormFile(new MemoryStream(filebytes), 0, filebytes.Length, "Data", "data.txt");

                var linesFromInputFile = service.GetLinesFromInputFile(file);
                var result = service.SaveSocialNetworkAnalysis(linesFromInputFile.Result, "TestAnalysis", "fileName");

                Assert.That(result, Is.EqualTo(false));
            }   
        }

        [Test]
        public void SaveAnalysisRightInpuNotNumberTest()
        {
            if (socialNetworkAnalysisRepository != null)
            {
                var service = new AnalysisService(socialNetworkAnalysisRepository);

                byte[] filebytes = Encoding.UTF8.GetBytes($"0 a{Environment.NewLine}1 2");
                IFormFile file = new FormFile(new MemoryStream(filebytes), 0, filebytes.Length, "Data", "data.txt");

                var linesFromInputFile = service.GetLinesFromInputFile(file);
                var result = service.SaveSocialNetworkAnalysis(linesFromInputFile.Result, "TestAnalysis", "fileName");

                Assert.That(result, Is.EqualTo(false));
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