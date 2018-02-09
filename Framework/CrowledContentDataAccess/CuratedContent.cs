using CrawledContentDataAccess.StateBasedContents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawledContentDataAccess
{
    public class CuratedContent
    {
        public string Description { get; set; }
        public string StateDeviation { get; set; }
        public List<Resource> Resources { get; set; }
        public List<Scenario> Scenarios { get; set; }
        public List<Process> Processes { get; set; }
        public List<string> RelatedIntents { get; set; }
        public string[] TopSixIntentsForLowConfidenceIntents { get; set; }
    }
    public class CuratedContentForAScenario
    {
        public int? ScenarioId { get; set; }
        public string Description { get; set; }
        public string StateDeviation { get; set; }
        public string Outcome { get; set; }
        public List<Resource> RelatedResources { get; set; }       
        public List<Process> Processes { get; set; }
        public string CurrentIntent { get; set; }
        public List<string> RelatedIntents { get; set; }
        public string[] TopTwoIntentsForLowConfidenceIntents { get; set; }
        public string SelectedState { get; set; }
    }
    public class Resource
    {        
        public int ResourceId { get; set; }
        public string ResourceType { get; set; }
        public string ResourceJson { get; set; }//Body
        public string Title { get; set; }
        public string Action { get; set; }     
    }
    public class Scenario
    {
        public int ScenarioId { get; set; }       
        public int LC_ID { get; set; }
        public string Description { get; set; }
        public string StateDeviation { get; set; }
        public string Outcome { get; set; }
    }

    public class Process
    {      
        public int Id { get; set; }
        public string ParentId { get; set; }
        public string Title { get; set; }
        public string ActionJson { get; set; }
        public string Description { get; set; }
        public StepType stepType { get; set; } 
        public string stepNumber { get; set; }

    }

    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }


}