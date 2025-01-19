using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAnalyser.Models
{
    public class FileUploadModel
    {
        /// <summary>
        /// Name of analysis;
        /// </summary>
        [Display(Name = "Name of analysis")]
        [Required]
        public required string NameOfAnalysis { get; set; }

        /// <summary>
        /// The file with input data for analysis.
        /// </summary>
        [Required]
        public required IFormFile AnalysisFile { get; set; }
    }
}
