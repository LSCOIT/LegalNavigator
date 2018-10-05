using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api
{
    public static class Constants
    {
        public const string BreadcrumbStoredProcedureName = "GetParentTopics";
        public const string PlanStoredProcedureName = "GetPlanDetails";
        public const string TopicTags = "topicTags";
        public const string Id = "id";
        public const string ParentTopicId = "parentTopicId";
        public const string Keywords = "keywords";
        public const string LuisResponse = "luisResponse";
        public const string Topics = "topics";
        public const string Resources = "resources";
        public const string TopIntent = "topIntent";
        public const string WebResources = "webResources";
        public const string Name = "name";
        public const string ResourceType = "resourceType";
        public const string OId = "oId";
        public const string PlanId = "planId";
        public const string Type = "type";
        public const string Organization = "Organizations";
        public const string PermaLink = "permaLink";
        public const string SharedResource = "sharedResources";
        public const string Url = "url";
        public const string ExpirationDate = "expirationDate";
        public const int ExpirationDateDurationInYears = 1;
        public const string ProfileLink = "/profile";
        public const string All = "ALL";
        public const string GuidedAssistant = "Guided Assistant";
        public const string EmptyArray = "[]";
        public const string UserRole = "Role";
        public const string UserName = "eMail";
        public const string Delimiter = "|";
    }
}
