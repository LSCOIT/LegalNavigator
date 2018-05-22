namespace Access2Justice.Api
{
    using Access2Justice.CosmosDb.Interfaces;
    using Access2Justice.Shared.Interfaces;    
    using System.Threading.Tasks;
    using Access2Justice.Shared;
    using Newtonsoft.Json;

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
            // we need to use a quey format to retrieve items because we are returning a dynmaic object.
            var qeury = string.Format("SELECT * FROM c WHERE CONTAINS(c.keywords, '{0}')", keywords);
            var result = await backendDatabaseService.QueryItemsAsync(cosmosDbSettings.TopicCollectionId, qeury);

            return JsonConvert.SerializeObject(result);
        }
    }
}
