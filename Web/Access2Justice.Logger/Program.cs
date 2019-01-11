using System;
using System.Collections.Generic;
using Access2Justice.Logger.Logging;

namespace Access2Justice.Logger
{
    class Program
    {
        static void Main(string[] args)
        {
            //Sample code for logging....         
            var aiLogProperties = new Dictionary<string, string> { { "Search Topic", "Kicked out from home" }, { "Intent Result", "Eviction" } };
            //pass null for key to pick it up from app.config...or use keyvalut (preferred) to get instrumentation key.

            var aiLog = LoggerFactory.GetLogger(AppEnum.LoggerType.AppInsight, null);
            aiLog.TrackEvent("TopicSearch", aiLogProperties);
            aiLog.FlushInsights();
            try
            {
                Console.WriteLine("logging exception...");
                System.IO.DirectoryInfo dr = new System.IO.DirectoryInfo("j:\\test\\a.txt");
                dr.CreateSubdirectory("f:\\test\\a");
            }
            catch (Exception ex)
            {
                aiLog.TrackException(ex, "100", AppEnum.EventDictionary.MessageFailedValidation);
                aiLog.FlushInsights();
            }
        }
    }
}
