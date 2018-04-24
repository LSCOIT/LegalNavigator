using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Models
{
    public class CuratedExperience
    {
        public string id { get; set; }
        public string description { get; set; }
        public string parentId { get; set; }
        public List<string> childern { get; set; }
    }
}
