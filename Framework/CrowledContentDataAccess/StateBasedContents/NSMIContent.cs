using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawledContentDataAccess.StateBasedContents
{
    public class NSMIContent
    {
        public string Description { get; set; }
        public List<Resource> Resources { get; set; }
        public List<Process> Processes { get; set; }
        public List<string> SubCategories { get; set; }

    }
}
