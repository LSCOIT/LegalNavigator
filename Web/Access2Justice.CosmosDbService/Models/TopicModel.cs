using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.CosmosDbService.Models
{
   public class TopicModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; } 
    }
    public class SubjectModel
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public string Content { get; set; }    
        public string Articles { get; set; }
        public string Videos { get; set; }
        public string Forms { get; set; }
    }
}
