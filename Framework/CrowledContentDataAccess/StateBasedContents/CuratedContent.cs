using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawledContentDataAccess.StateBasedContents
{
    
    public class CuratedContent1
    {
        public string Description { get; set; }
        public List<Resource> Resources { get; set; }
        public List<Scenario> Scenarios { get; set; }
        public List<Process> Processes { get; set; }
        public List<string> RelatedIntents { get; set; }

    }
}
