
using Access2Justice.Tools.BusinessLogic;
using System;

namespace Access2Justice.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Executing the script...");
            TopicBusinessLogic.GetTopics();
        }
    }
}