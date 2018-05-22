namespace Access2Justice.Api
{
    using Access2Justice.CosmosDb.Interfaces;
    using Access2Justice.Shared.Interfaces;    
    using System.Threading.Tasks;
    using Access2Justice.Shared;

    public class Helper : IHelper
    {
        private IBackendDatabaseService backendDatabaseService;
        private ICosmosDbSettings cosmosDbSettings;

        public Helper(IBackendDatabaseService backendDatabaseService,ICosmosDbSettings cosmosDbSettings)
        {
            this.backendDatabaseService = backendDatabaseService;
            this.cosmosDbSettings = cosmosDbSettings;
        }


        public async Task<dynamic> GetTopicAsync(string keywords)
        {
            return await backendDatabaseService.GetItemsAsync<TopicModel>(a => a.keywords == keywords, cosmosDbSettings.TopicCollectionId);
        }
    }
}
