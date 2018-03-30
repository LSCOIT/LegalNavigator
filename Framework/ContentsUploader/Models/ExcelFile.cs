using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentUploader.Model
{
    public class ExcelFile
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string ExtractedText { get; set; }

    }
}
