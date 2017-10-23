using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContentsExtractionApi.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class StateToConnectionStringMapper
    {
        private const string prefix = "CrowledContentsDb";
        /// <summary>
        /// Provides connection string name
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string ToConnectionString(string state)
        {
            return string.Format("{0}_{1}", prefix, state);
        }
    }
}