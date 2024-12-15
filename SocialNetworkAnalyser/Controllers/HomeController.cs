using Microsoft.AspNetCore.Mvc;
using SocialNetworkAnalyser.Entitites;
using SocialNetworkAnalyser.Models;
using SocialNetworkAnalyser.Services;
using System.Diagnostics;

namespace SocialNetworkAnalyser.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IAnalysisService analysisService) : Controller
    {
        private readonly ILogger<HomeController> logger = logger;
        private readonly IAnalysisService analysisService = analysisService;

        public IActionResult Index()
        {
            return View(this.analysisService.GetAllSocialNetworkAnalysis());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> FileUpload(FileUploadModel fileUploadModel)
        {
            if (string.IsNullOrEmpty(fileUploadModel.NameOfAnalysis))
            {
                ModelState.AddModelError(nameof(fileUploadModel.NameOfAnalysis), "Name of analysis must be exists.");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (fileUploadModel.AnalysisFile == null)
            {
                ModelState.AddModelError(nameof(fileUploadModel.AnalysisFile), "Please add file with data.");
            }
            else
            {
                var res = await UploadFile(fileUploadModel.AnalysisFile);
                
                if (res != null)
                {
                    var savingResult = this.analysisService.SaveInputData(res, fileUploadModel.NameOfAnalysis, fileUploadModel.AnalysisFile.FileName);
                    if(!savingResult)
                    {
                        ModelState.AddModelError(nameof(fileUploadModel.AnalysisFile), "File contains incorrect data.");

                        return View();
                    }
                }
            }

            return RedirectToAction("Index");
        }
        
        public async Task<List<string>> UploadFile(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    var result = new List<string>();
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        while (reader.Peek() >= 0)
                        {
                            var line = await reader.ReadLineAsync();

                            if (!string.IsNullOrEmpty(line))
                            {
                                result.Add(line);
                            }
                        }
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return new List<string>();
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SocialNetworkAnalysis? socialNetworkAnalysis = analysisService.GetSocialNetworkAnalysis(id.Value);

            if (socialNetworkAnalysis == null)
            {
                return NotFound();
            }

            return View(socialNetworkAnalysis);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
