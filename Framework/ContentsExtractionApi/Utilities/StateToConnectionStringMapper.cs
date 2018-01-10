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
        // private const string prefix = "ContentsDb";//"CrowledContentsDb";//
        /// <summary>
        /// Provides connection string name
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string ToConnectionString(string prefix, string state)
        {
            //Todo: adjust the suffix later
            return string.Format("{0}_{1}", prefix, state.Substring(0,2).ToUpper());
        }
    }
}