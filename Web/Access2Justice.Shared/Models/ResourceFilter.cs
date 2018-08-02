﻿using System;
using System.Collections.Generic;

namespace Access2Justice.Shared.Models
{
    public class ResourceFilter
    {
        public string ResourceType { get; set; }
        public string ContinuationToken { get; set; }
        public IEnumerable<string> TopicIds { get; set; }
        public IEnumerable<string> ResourceIds { get; set; }
        public Int16 PageNumber { get; set; }
        public Location Location { get; set; }
        public Boolean IsResourceCountRequired { get; set; }
    }
}
