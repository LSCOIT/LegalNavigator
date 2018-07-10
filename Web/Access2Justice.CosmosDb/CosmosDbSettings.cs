using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;

namespace Access2Justice.CosmosDb
{
    public class CosmosDbSettings : ICosmosDbSettings
    {
        public CosmosDbSettings(IConfiguration configuration)
        {
            try
            {
                AuthKey = configuration.GetSection("AuthKey").Value;
                Endpoint = new Uri(configuration.GetSection("Endpoint").Value);
                DatabaseId = configuration.GetSection("DatabaseId").Value;
                TopicCollectionId = configuration.GetSection("TopicCollectionId").Value;
                ResourceCollectionId = configuration.GetSection("ResourceCollectionId").Value;
                UserProfileCollectionId = configuration.GetSection("UserProfileCollectionId").Value;
                PageResultsCount = int.Parse(configuration.GetSection("PageResultsCount").Value, CultureInfo.InvariantCulture);
                CuratedExperienceCollectionId = configuration.GetSection("CuratedExperienceCollectionId").Value;
                CuratedExperienceAnswersCollectionId = configuration.GetSection("CuratedExperienceAnswersId").Value;
            }
            catch
            {
                throw new Exception("Invalid CosmosDB configurations");
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
    }
}
