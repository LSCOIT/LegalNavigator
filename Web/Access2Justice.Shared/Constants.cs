namespace Access2Justice.Shared
{
    public static class Constants
    {
        public const string ResourceTypeAll = "ALL";
        public const string BreadcrumbStoredProcedureName = "GetParentTopics";
        public const string PlanStoredProcedureName = "GetPlanDetails";
        public const string TopicTags = "topicTags";
        public const string Id = "id";
        public const string ParentTopicId = "parentTopicId";
        public const string Keywords = "keywords";
        public const string LuisResponse = "luisResponse";
        public const string Topics = "topics";
        public const string Resources = "resources";
        public const string IncomingResources = "incoming-resources";
        public const string TopIntent = "topIntent";
        public const string WebResources = "webResources";
        public const string Name = "name";
        public const string ResourceType = "resourceType";
        public const string OId = "oId";
        public const string Email = "eMail";
        public const string PlanId = "planId";
        public const string Type = "type";
        public const string Organization = "Organizations";
        public const string EmptyArray = "[]";
        public const string PermaLink = "permaLink";
        public const string SharedResource = "sharedResources";
        public const string Url = "url";
        public const string ExpirationDate = "expirationDate";
        public const int ExpirationDateDurationInYears = 1;
        public const string ProfileLink = "/profile";
        public const string All = "ALL";
        public const string GuidedAssistant = "Guided Assistant";
        public const string UserRole = "roleName";
        public const string UserName = "eMail";
        public const string Delimiter = "|";
        public const string sharedResourceId = "sharedResourceId";
        public const string HtmlRightBracket = ">";
        public const string HtmlLeftBracket = "<";
        public const string A2JAuthorCustomFunctionTags = "%%";
        public const string StateProvinceType = "stateProvince";
        public const string StateProvince = "stateProvinces";
        public const string Location = "location";
        public const string StateCode = "state";
        public const string ServiceProviderResourceType = "Service Providers";
        public const string IntegrationAPI = "Integration API";
        public const string ExternalId = "externalId";
        public const string RTMSessionId = "session_id";
        public const string EFormFormFields = "onboardingInfo";
        public const string Plans = "plan";
        public const string SharedResources = "shared-resources";
        public const string Code = "code";
        public const string DefaultOgranizationalUnit = "Default";
        public static readonly int StrigifiedGuidLength = System.Guid.Empty.ToString().Length;

        public static class ReasourceTypes
        {
            public const string AdditionalReadings = "Additional Readings";
        }

        public static class StaticResourceTypes
        {
            public const string AboutPage = "AboutPage";
            public const string GuidedAssistantPrivacyPage = "GuidedAssistantPrivacyPage";
            public const string HelpAndFAQPage = "HelpAndFAQPage";
            public const string HomePage = "HomePage";
            public const string Navigation = "Navigation";
            public const string PersonalizedActionPlanPage = "PersonalizedActionPlanPage";
            public const string PrivacyPromisePage = "PrivacyPromisePage";
        }

        public const string A2JTemplateFileExtension = ".zip";
    }
}

