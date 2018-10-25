using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;

namespace Access2Justice.CosmosDb
{
    public class CosmosDbSettings : ICosmosDbSettings
    {
        public CosmosDbSettings(IConfiguration configuration, IConfiguration kvConfiguration)
        {
            try
            {
                //if we get null from calling program, we will use config settings.
                if (kvConfiguration != null)
                {
                    IKeyVaultSettings kv = new Access2Justice.Shared.Utilities.KeyVaultSettings(kvConfiguration);
                    var kvSecret = kv.GetKeyVaultSecrets("CosmosDbAuthKey");
                    kvSecret.Wait();          
                    AuthKey = kvSecret.Result;
                }
                else
                {
                    AuthKey = configuration.GetSection("AuthKey").Value;
                }
                Endpoint = new Uri(configuration.GetSection("Endpoint").Value);
                DatabaseId = configuration.GetSection("DatabaseId").Value;
                TopicsCollectionId = configuration.GetSection("TopicsCollectionId").Value;
                ResourcesCollectionId = configuration.GetSection("ResourcesCollectionId").Value;
                UserProfilesCollectionId = configuration.GetSection("UserProfilesCollectionId").Value;
                PageResultsCount = int.Parse(configuration.GetSection("PageResultsCount").Value, CultureInfo.InvariantCulture);
                CuratedExperiencesCollectionId = configuration.GetSection("CuratedExperiencesCollectionId").Value;
                CuratedExperienceAnswersCollectionId = configuration.GetSection("CuratedExperienceAnswersCollectionId").Value;
                PersonalizedActionsPlanCollectionId = configuration.GetSection("PersonalizedActionPlansCollectionId").Value;
                StaticResourcesCollectionId = configuration.GetSection("StaticResourcesCollectionId").Value;
                UserSavedResourcesCollectionId = configuration.GetSection("UserSavedResourcesCollectionId").Value;
                UserResourcesCollectionId = configuration.GetSection("UserResourcesCollectionId").Value;
                A2JAuthorTemplatesCollectionId = configuration.GetSection("A2JAuthorTemplatesCollectionId").Value;
                UserRolesCollectionId = configuration.GetSection("UserRolesCollectionId").Value;
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
        public string UserProfilesCollectionId { get; private set; }
        public string CuratedExperiencesCollectionId { get; private set; }
        public string CuratedExperienceAnswersCollectionId { get; private set; }
        public string PersonalizedActionsPlanCollectionId { get; private set; }
        public string StaticResourcesCollectionId { get; private set; }
        public string UserSavedResourcesCollectionId { get; private set; }
        public string UserResourcesCollectionId { get; private set; }
        public string A2JAuthorTemplatesCollectionId { get; private set; }
        public string UserRolesCollectionId { get; private set; }
    }
}
