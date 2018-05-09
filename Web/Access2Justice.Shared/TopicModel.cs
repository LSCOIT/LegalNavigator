using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared
{
    public class TopicModel
    {
        public string id { get; set; }
      
        public string type { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public string keywords { get; set; }
        public string description { get; set; }
        public string lang { get; set; }
        public List<SubTopic> subtopic { get; set; }
    }
    public class SubTopic
    {
        public string id { get; set; }
        public string title { get; set; }
        public bool IsActive { get; set; }
    }
}
