
using Access2Justice.Tools.BusinessLogic;
using System;

namespace Access2Justice.Tools
{
    class Program
    {
        // This tool is deprecated. Please use the api endpoints to import Topics and Resources files.
        static void Main(string[] args)
        {
            Console.WriteLine("Executing the script...");
            TopicBusinessLogic p = new TopicBusinessLogic();
            p.GetTopics().Wait();
            Console.WriteLine("Topics created.");
            ResourceBusinessLogic q = new ResourceBusinessLogic();
            q.GetResources().Wait();
            Console.WriteLine("Resources created.");
            Console.WriteLine("Script completed.");
            Console.ReadLine();
        }
    }
}