using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class UnprocessedPersonalizedPlan
    {
        public Guid Id { get; set; }
        public List<CrudeTopic> CrudeTopics { get; set; }
    }

    public class CrudeTopic
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Guid> ResourceIds { get; set; }
    }
}
