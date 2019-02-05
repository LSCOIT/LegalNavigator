using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;

namespace Access2Justice.CosmosDb
{
    public class CosmosDbSettings : ICosmosDbSettings
    {
        public CosmosDbSettings(IConfiguration configuration, ISecretsService secretsService)
        {
            try
            {
                AuthKey = secretsService.GetSecret("CosmosDbAuthKey");

                Endpoint = new Uri(configuration.GetSection("Endpoint").Value);
                DatabaseId = configuration.GetSection("DatabaseId").Value;
                TopicsCollectionId = configuration.GetSection("TopicsCollectionId").Value;
                ResourcesCollectionId = configuration.GetSection("ResourcesCollectionId").Value;
                ProfilesCollectionId = configuration.GetSection("ProfilesCollectionId").Value;
                PageResultsCount = int.Parse(configuration.GetSection("PageResultsCount").Value, CultureInfo.InvariantCulture);
                CuratedExperiencesCollectionId = configuration.GetSection("CuratedExperiencesCollectionId").Value;
                ActionPlansCollectionId = configuration.GetSection("ActionPlansCollectionId").Value;
                StaticResourcesCollectionId = configuration.GetSection("StaticResourcesCollectionId").Value;
                UserResourcesCollectionId = configuration.GetSection("UserResourcesCollectionId").Value;
                A2JAuthorDocsCollectionId = configuration.GetSection("A2JAuthorDocsCollectionId").Value;
                RolesCollectionId = configuration.GetSection("RolesCollectionId").Value;
                GuidedAssistantAnswersCollectionId = configuration.GetSection("GuidedAssistantAnswersCollectionId").Value;
                StateProvincesCollectionId = configuration.GetSection("StateProvincesCollectionId").Value;
            }
            catch(Exception ex)
            {
                throw new Exception("Invalid CosmosDB configurations or key vault error", ex.InnerException);
            }
        }
        public string AuthKey { get; private set; }
        public Uri Endpoint { get; private set; }
        public string DatabaseId { get; private set; }
        public string TopicsCollectionId { get; private set; }
        public string ResourcesCollectionId { get; private set; }
        public int PageResultsCount { get; private set; }
        public string ProfilesCollectionId { get; private set; }
        public string CuratedExperiencesCollectionId { get; private set; }
        public string CuratedExperienceAnswersCollectionId { get; private set; }
        public string ActionPlansCollectionId { get; private set; }
        public string StaticResourcesCollectionId { get; private set; }
        public string UserSavedResourcesCollectionId { get; private set; }
        public string UserResourcesCollectionId { get; private set; }
        public string A2JAuthorDocsCollectionId { get; private set; }
        public string RolesCollectionId { get; private set; }
        public string GuidedAssistantAnswersCollectionId { get; private set; }
        public string StateProvincesCollectionId { get; private set; }


    }
}
