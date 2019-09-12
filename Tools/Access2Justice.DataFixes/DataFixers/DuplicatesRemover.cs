using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.DataFixes.DataAccess;
using Access2Justice.DataFixes.Helpers;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;

namespace Access2Justice.DataFixes.DataFixers
{
    public class DuplicatesRemover : IssueFixerBase, IDataFixer
    {
        protected override string IssueId => "#__duplicates";

        public async Task ApplyFixAsync(
            CosmosDbSettings cosmosDbSettings,
            CosmosDbService cosmosDbService)
        {
            List<Topic> topics =
                JsonHelper.Deserialize<List<Topic>>(
                    await cosmosDbService.FindAllItemsAsync(cosmosDbSettings.TopicsCollectionId));

            var topicsToDelete = new List<Topic>();
            foreach (var parentGroup in topics.GroupBy(x => (string)(x.ParentTopicId?.FirstOrDefault()?.ParentTopicIds?.ToString())/*, (x, collection) => collection.Where(y => y.ParentTopicId?.FirstOrDefault() == x)*/))
            {
                foreach (var groupByName in parentGroup.GroupBy(x => x.Name/*, (x, collection) => collection.Where(y => y.Name == x)*/))
                {
                    var g = groupByName.ToList();
                    if (g.Count <= 1)
                    {
                        continue;
                    }

                    var topicToStay = g.OrderByDescending(x => x.CreatedTimeStamp).FirstOrDefault();
                    topicsToDelete.AddRange(g.Where(x => x != topicToStay));
                }
            }

            LogEntry(JsonConvert.SerializeObject(topicsToDelete));

            foreach (var topic in topicsToDelete)
            {
                await cosmosDbService.DeleteItemAsync(topic.Id, topic.OrganizationalUnit, cosmosDbSettings.TopicsCollectionId);
            }
        }
    }
}
