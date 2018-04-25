using System.Collections.Generic;

namespace Access2Justice.Api.Models.CuratedExperience
{
    public class CuratedExperience
    {
        public string id { get; set; }
        public string description { get; set; }
        public string parentId { get; set; }
        public List<string> childern { get; set; }
    }
}
