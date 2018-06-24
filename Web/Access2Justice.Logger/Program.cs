using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access2Justice.Logger.Logging;

namespace Access2Justice.Logger
{
    class Program
    {
        static void Main(string[] args)
        {
            //Sample code for logging....
            string key = "<App Insights Instrumentation Key will come here....>";
            var _logger = AppInsightsLogger.GetInstance(key);

            var properties = new Dictionary<string, string> { { "Method Name", "Main" }, { "Search Topic", "User Keyword" } };
            _logger.TrackEvent("logging custom event with properties", properties);
            _logger.FlushInsights();

            try
            {
                Console.WriteLine("Checking exception...");
                System.IO.DirectoryInfo dr = new System.IO.DirectoryInfo("f:\\dfadfas\\a.txt");
                dr.CreateSubdirectory("f:\\dfadfas\\a");
            }
            catch (Exception ex)
            {
                _logger.TrackException(ex, "1", AppEnum.EventDictionary.MessageFailedValidation);
            }
        }
    }
}
