using System;
using System.Linq;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;

namespace Access2Justice.Api.ViewModels
{
    public class TopicView : Topic
    {
        [JsonProperty("curatedExperienceId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid CuratedExperienceId { get; set; }

        public TopicView(Topic topic)
        {
            Id = topic.Id;
            Name = topic.Name;
            Overview = topic.Overview;
            ParentTopicId = topic.ParentTopicId?.ToList();
            ResourceType = topic.ResourceType;
            Keywords = topic.Keywords;
            OrganizationalUnit = topic.OrganizationalUnit;
            Location = topic.Location?.ToList();
            Icon = topic.Icon;
            CreatedBy = topic.CreatedBy;
            CreatedTimeStamp = topic.CreatedTimeStamp;
            ModifiedBy = topic.ModifiedBy;
            ModifiedTimeStamp = topic.ModifiedTimeStamp;
            NsmiCode = topic.NsmiCode;
        }

        public TopicView()
        {
        }
    }
}