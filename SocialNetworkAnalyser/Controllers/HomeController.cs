using Microsoft.AspNetCore.Mvc;
using SocialNetworkAnalyser.Entitites;
using SocialNetworkAnalyser.Models;
using SocialNetworkAnalyser.Services;
using System.Diagnostics;

namespace SocialNetworkAnalyser.Controllers
{
    /// <summary>
    /// Provides functionality for SN analyser.
    /// </summary>
    /// <param name="logger">Logging.</param>
    /// <param name="analysisService">Analysis service.</param>
    public class HomeController(ILogger<HomeController> logger, IAnalysisService analysisService) : Controller
    {
        private readonly ILogger<HomeController> logger = logger;
        private readonly IAnalysisService analysisService = analysisService;

        /// <summary>
        /// Provides data for home page view.
        /// </summary>
        /// <returns>View for home page.</returns>
        public IActionResult Index()
        {
            return View(this.analysisService.GetAllSocialNetworkAnalysis());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        
        private async Task<List<string>> UploadFile(IFormFile file)
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

        /// <summary>
        /// Provides data for detail view of analysis.
        /// </summary>
        /// <param name="id">Identifier of analysis.</param>
        /// <returns>Detail view of analysis.</returns>
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
