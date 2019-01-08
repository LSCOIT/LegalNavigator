using System.Collections.Generic;

namespace Access2Justice.Shared.Models
{
    public class ResourceFilter
    {
        public ResourceFilter()
        {
            TopicIds = new List<string>();
            ResourceIds = new List<string>();
        }
        public string ResourceType { get; set; }
        public string ContinuationToken { get; set; }
        public IEnumerable<string> TopicIds { get; set; }
        public IEnumerable<string> ResourceIds { get; set; }
        public short PageNumber { get; set; }
        public Location Location { get; set; }
        public bool IsResourceCountRequired { get; set; }
        public bool IsOrder { get; set; }
        public string OrderByField { get; set; }
        public string OrderBy { get; set; }
    }
}
