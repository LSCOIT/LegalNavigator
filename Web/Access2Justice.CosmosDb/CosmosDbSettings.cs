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
                TopicCollectionId = configuration.GetSection("TopicCollectionId").Value;
                ResourceCollectionId = configuration.GetSection("ResourceCollectionId").Value;
                UserProfileCollectionId = configuration.GetSection("UserProfileCollectionId").Value;
                PageResultsCount = int.Parse(configuration.GetSection("PageResultsCount").Value, CultureInfo.InvariantCulture);
                CuratedExperienceCollectionId = configuration.GetSection("CuratedExperienceCollectionId").Value;
                CuratedExperienceAnswersCollectionId = configuration.GetSection("CuratedExperienceAnswersCollectionId").Value;
                PersonalizedActionPlanCollectionId = configuration.GetSection("PersonalizedActionPlanCollectionId").Value;
                StaticResourceCollectionId = configuration.GetSection("StaticResourceCollectionId").Value;
                UserSavedResourcesCollectionId = configuration.GetSection("UserSavedResourcesCollectionId").Value;
                UserResourceCollectionId = configuration.GetSection("UserResourceCollectionId").Value;

            }
            catch(Exception ex)
            {
                throw new Exception("Invalid CosmosDB configurations or key vault error", ex.InnerException);
            }
        }
        public string AuthKey { get; private set; }
        public Uri Endpoint { get; private set; }
        public string DatabaseId { get; private set; }
        public string TopicCollectionId { get; private set; }
        public string ResourceCollectionId { get; private set; }
        public int PageResultsCount { get; private set; }
        public string UserProfileCollectionId { get; private set; }
        public string CuratedExperienceCollectionId { get; private set; }
        public string CuratedExperienceAnswersCollectionId { get; private set; }
        public string PersonalizedActionPlanCollectionId { get; private set; }
        public string StaticResourceCollectionId { get; private set; }
        public string UserSavedResourcesCollectionId { get; private set; }
        public string UserResourceCollectionId { get; private set; }
    }
}
