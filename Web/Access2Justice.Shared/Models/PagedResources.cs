using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class PagedResources
    {
        public PagedResources()
        {
            Results = new List<dynamic>();
        }
        /// <summary>
        /// Continuation Token for DocumentDB
        /// </summary>
        public string ContinuationToken { get; set; }

        /// <summary>
        /// Results
        /// </summary>
        public List<dynamic> Results { get; set; }

        public string Id { get; set; }
    }
}
