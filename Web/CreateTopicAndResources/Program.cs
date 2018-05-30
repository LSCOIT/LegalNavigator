using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateTopicAndResources
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Executing the script...");
            Topic_BL p = new Topic_BL();
            p.GetTopics().Wait();
            Console.WriteLine("Script completed.");
            Console.ReadLine();
        }
    }
}
