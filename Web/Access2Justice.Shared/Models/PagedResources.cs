using System.Collections.Generic;

namespace Access2Justice.Shared.Models
{
    public class PagedResources
    {
        public PagedResources()
        {
            Results = new List<dynamic>();
            TopicIds = new List<string>();
        }
        public string ContinuationToken { get; set; }
        public IEnumerable<dynamic> Results { get; set; }
        public IEnumerable<string> TopicIds { get; set; }
    }
}
