using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.DataFixes.DataAccess;
using Access2Justice.DataFixes.Helpers;
using Access2Justice.Shared.Models;

namespace Access2Justice.DataFixes.DataFixers
{
    public class Issue953Fixer : IssueFixerBase, IDataFixer
    {
        protected override string IssueId => "#953";

        public async Task ApplyFixAsync(
            CosmosDbSettings cosmosDbSettings,
            CosmosDbService cosmosDbService)
        {
            List<GuidedAssistant> resources = JsonHelper.Deserialize<List<GuidedAssistant>>(
                await cosmosDbService.QueryItemsAsync(cosmosDbSettings.ResourcesCollectionId, "SELECT * FROM c where c.resourceType = 'Guided Assistant' and c.curatedExperienceId != null"));
            var curatedExperiences = 
                await cosmosDbService.FindAllItemsAsync(cosmosDbSettings.CuratedExperiencesCollectionId);
            var itemsToUpdate = new List<dynamic>();
            foreach (var curatedExperience in curatedExperiences)
            {
                var resource = resources.FirstOrDefault(
                    x => !string.IsNullOrWhiteSpace(x.CuratedExperienceId) 
                         && x.CuratedExperienceId == curatedExperience.id
                         && !string.IsNullOrWhiteSpace(x.NsmiCode));
                if (resource != null)
                {
                    curatedExperience.nsmiCode = resource.NsmiCode;
                    itemsToUpdate.Add(curatedExperience);
                }
            }

            foreach (var item in itemsToUpdate)
            {
                await cosmosDbService.UpdateItemAsync(
                    item.id,
                    item,
                    cosmosDbSettings.CuratedExperiencesCollectionId);
            }
        }
    }
}
