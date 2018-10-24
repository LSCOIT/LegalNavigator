using System;
using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public class PersonalizedPlanCustomTags
    {
        public List<Guid> ResourceIds { get; set; }

        public string SanitizedHtml { get; set; }

        public PersonalizedPlanCustomTags()
        {
            ResourceIds = new List<Guid>();
        }
    }
}