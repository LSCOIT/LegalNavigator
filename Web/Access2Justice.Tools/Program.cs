
using Access2Justice.Tools.BusinessLogic;
using System;

namespace Access2Justice.Tools
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Executing the script...");
            TopicBusinessLogic p = new TopicBusinessLogic();
            p.GetTopics().Wait();
            ResourceBusinessLogic q = new ResourceBusinessLogic();
            q.GetResources().Wait();
            Console.WriteLine("Script completed.");
            Console.ReadLine();
        }
    }
}