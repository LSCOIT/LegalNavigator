using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentsExtractionApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentExtractionRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string State { get; set; }
    }
}
