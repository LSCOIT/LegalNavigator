using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class UnprocessedPersonalizedPlan
    {
        public Guid Id { get; set; }
        public List<UnprocessedTopic> CrudeTopics { get; set; }
    }

    public class UnprocessedTopic
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Guid> ResourceIds { get; set; }
    }
}
