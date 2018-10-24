using System;
using System.Collections.Generic;

namespace Access2Justice.HTMLContentExtractor
{
    public class CuratedExperienceContent
    {
        public List<Guid> ResourceIds { get; set; }

        public string SanitizedString { get; set; }

        public CuratedExperienceContent()
        {
            ResourceIds = new List<Guid>();
        }
    }
}
