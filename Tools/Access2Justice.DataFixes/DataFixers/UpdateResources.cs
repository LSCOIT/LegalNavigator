using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.DataFixes.DataAccess;
using Access2Justice.DataFixes.Helpers;
using Access2Justice.Shared.Models;

namespace Access2Justice.DataFixes.DataFixers
{
    public class UpdateResources : IssueFixerBase, IDataFixer
    {
        protected override string IssueId => "__updateResources";

        public async Task ApplyFixAsync(CosmosDbSettings cosmosDbSettings, CosmosDbService cosmosDbService)
        {
            List<Resource> resources =
                JsonHelper.Deserialize<List<Resource>>(
                await cosmosDbService.FindAllItemsAsync(cosmosDbSettings.ResourcesCollectionId));
            var additionalResources = resources.Where(x => x.ResourceType == "Additional Readings");
            foreach (var item in additionalResources)
            {
                item.ResourceType = "Related Readings";
                await cosmosDbService.UpdateItemAsync(item.ResourceId,item, cosmosDbSettings.ResourcesCollectionId);
            }
        }
    }
}
