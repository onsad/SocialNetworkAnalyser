using Microsoft.AspNetCore.Http;
using Moq;
using SocialNetworkAnalyser.Repositories;
using SocialNetworkAnalyser.Services;
using System.Text;

namespace SocialNetworkAnalyserTest
{
    public class AnalysisServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetLinesFromInputFileTest()
        {
            Mock<ISocialNetworkAnalysisRepository> mock = new Mock<ISocialNetworkAnalysisRepository>();
            var service = new AnalysisService(mock.Object);

            byte[] filebytes = Encoding.UTF8.GetBytes($"0 1{Environment.NewLine}1 1");
            IFormFile file = new FormFile(new MemoryStream(filebytes), 0, filebytes.Length, "Data", "data.txt");

            var expectedLines = new List<string> { "0 1", "1 1" };
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

            Assert.That(resultLines.Result.Count, Is.EqualTo(0));
        }
    }
}