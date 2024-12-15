using System.ComponentModel.DataAnnotations;

namespace SocialNetworkAnalyser.Models
{
    public class FileUploadModel
    {
        [Display(Name = "Name of analysis")]
        [Required]
        public required string NameOfAnalysis { get; set; }

        [Required]
        public required IFormFile AnalysisFile { get; set; }
    }
}
