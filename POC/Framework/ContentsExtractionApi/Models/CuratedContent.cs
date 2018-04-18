using System.Collections.Generic;

namespace ContentsExtractionApi.Models
{
    public class CuratedContent
    {
        public string Description { get; set; }
        public List<Resource> Resources { get; set; }
        public List<Scenario> Scenarios { get; set; }
        public List<Process> Processes { get; set; }
        public List<string> RelatedIntents { get; set; }

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
        
    }

    public class Process
    {      
        public int Id { get; set; }
        public string ParentId { get; set; }
        public string Title { get; set; }
        public string ActionJson { get; set; }
        public string Description { get; set; }
        public StepType stepType { get; set; }     

    }

    public enum StepType
    {
        Description = 1,
        Action
    }
}