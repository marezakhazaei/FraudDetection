using System;

#nullable disable

namespace FraudDetection.UI.Models
{
    public partial class SourceFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileCaption { get; set; }
        public int? ParentId { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
