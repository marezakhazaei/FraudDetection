using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FraudDetection.UI.Models
{
    public class ResultFileViewModel
    {
        public int ParentId { get; set; }
        [Required]
        public string FileCaption { get; set; }
        [Required]
        public IFormFile SendFile { set; get; }
    }
}
