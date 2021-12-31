using System;
using System.Collections.Generic;

namespace FraudDetection.UI.Models
{
    public class SourceFileDetailViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime? CreateDate { get; set; }
        public IEnumerable<ImageResultViewModel> Images { get; set; }
    }
}
