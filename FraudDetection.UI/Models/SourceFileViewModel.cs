using Microsoft.AspNetCore.Http;

namespace FraudDetection.UI.Models
{
    public class SourceFileViewModel
    {
        public IFormFile SendFile { set; get; }
    }
}
