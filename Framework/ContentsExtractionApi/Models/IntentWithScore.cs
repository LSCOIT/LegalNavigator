using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContentsExtractionApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class IntentWithScore
    {
        /// <summary>
        /// Top Scoring Intent
        /// </summary>
        public string TopScoringIntent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  decimal Score { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public  string [] TopTwoIntents { get; set; }
    }
}