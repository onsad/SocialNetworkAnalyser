using Microsoft.AspNetCore.Mvc;
using SocialNetworkAnalyser.Models;
using SocialNetworkAnalyser.Services;
using System.Diagnostics;

namespace SocialNetworkAnalyser.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IAnalysisService analysisService;

        public HomeController(ILogger<HomeController> logger, IAnalysisService analysisService)
        {
            this.logger = logger;
            this.analysisService = analysisService;
        }

        public IActionResult Index()
        {
            return View(this.analysisService.GetSocialNetworkAnalysis());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> FileUpload(IFormFile file, string NameOfAnalysis)
        {
            await UploadFile(file);
            TempData["msg"] = "File Uploaded successfully.";
            return View();
        }
        // Upload file on server
        public async Task<bool> UploadFile(IFormFile file)
        {
            string path = "";
            bool iscopied = false;
            try
            {
                if (file.Length > 0)
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Upload"));
                    using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }
                    iscopied = true;
                }
                else
                {
                    iscopied = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return iscopied;
        }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
