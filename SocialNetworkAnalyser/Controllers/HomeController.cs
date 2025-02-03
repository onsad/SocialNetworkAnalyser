using Microsoft.AspNetCore.Mvc;
using SocialNetworkAnalyser.Entitites;
using SocialNetworkAnalyser.Models;
using SocialNetworkAnalyser.Services;
using System.Diagnostics;

namespace SocialNetworkAnalyser.Controllers
{
    
    /// <summary>
    /// Provides functionality for Social Network analyser.
    /// </summary>
    /// <param name="logger">Logging.</param>
    /// <param name="analysisService">Analysis service.</param>
    public class HomeController(ILogger<HomeController> logger, IAnalysisService analysisService) : Controller
    {
        /// <summary>
        /// Provides data for home page view.
        /// </summary>
        /// <returns>View for home page.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View(analysisService.GetAllSocialNetworkAnalysis());
        }

        /// <summary>
        /// Provides data for analysis upload page.
        /// </summary>
        /// <returns>View for analysis upload page.</returns>
        [HttpGet]
        public IActionResult FileUpload()
        {
            return View();
        }

        /// <summary>
        /// Provides andpoint foà uploading file with data for analysis.
        /// </summary>
        /// <param name="fileUploadModel"></param>
        /// <returns></returns>
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
                List<string>? linesFromFile;
                try
                {
                    linesFromFile = await analysisService.GetLinesFromInputFile(fileUploadModel.AnalysisFile);

                    if (linesFromFile != null && linesFromFile.Count > 0)
                    {
                        var savingResult = analysisService.SaveSocialNetworkAnalysis(linesFromFile, fileUploadModel.NameOfAnalysis, fileUploadModel.AnalysisFile.FileName);
                        if (!savingResult)
                        {
                            ModelState.AddModelError(nameof(fileUploadModel.AnalysisFile), "File contains incorrect data.");

                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(fileUploadModel.AnalysisFile), "File contains incorrect data.");

                        return View();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorMessage = ex.Message });
                }   
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Provides data for detail view of analysis.
        /// </summary>
        /// <param name="id">Identifier of analysis.</param>
        /// <returns>Detail view of analysis.</returns>
        [HttpGet]
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorMessage = "Unknown error." });
        }
    }
}
