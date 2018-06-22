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
            string key = "<App Insights Instrumentation Key will come here....>";
            var _fameLogger = AppInsightsLogger.GetInstance(key);
            _fameLogger.TrackEvent("logging custom event....Execution Started from program.cs ");
            _fameLogger.FlushInsights();

            try
            {
                Console.WriteLine("Hello World!");
                System.IO.DirectoryInfo dr = new System.IO.DirectoryInfo("f:\\dfadfas\\a.txt");
                //dr.CreateSubdirectory("f:\\dfadfas\\a");
            }
            catch(Exception ex)
            {
                _fameLogger.TrackException(ex);
            }
        
           // Console.ReadKey();

        }
    }
}
