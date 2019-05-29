using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Access2Justice.DataFixes.DataAccess;
using Access2Justice.DataFixes.Helpers;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;

namespace Access2Justice.DataFixes.DataFixers
{
    public class Issue875Fixer : IssueFixerBase, IDataFixer
    {
        private readonly string _filePath;
        private const string _predefinedFilePath = ".\\AppData\\NSMIv2.json";
        protected override string IssueId => "#875";
        
        public Issue875Fixer(string filePath = null)
        {
            _filePath = filePath;
            if (string.IsNullOrWhiteSpace(_filePath))
            {
                _filePath = _predefinedFilePath;
            }
        }

        public async Task ApplyFixAsync(
            CosmosDbSettings cosmosDbSettings,
            CosmosDbService cosmosDbService)
        {

            // read json settings
            var allCodes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            try
            {
                using (var file = new StreamReader(_filePath))
                {
                    var jsonData = await file.ReadToEndAsync();
                    foreach (var legalTopicSubject in JsonConvert.DeserializeObject<List<LegalTopicSubject>>(jsonData))
                    {
                        if (allCodes.ContainsKey(legalTopicSubject.Name) && allCodes[legalTopicSubject.Name] != legalTopicSubject.Code)
                        {
                            LogEntry(
                                $"Found nsmi code collision: name {legalTopicSubject.Name}, " +
                                $"codes: {allCodes[legalTopicSubject.Name]}, {legalTopicSubject.Code}");
                        }
                        allCodes[legalTopicSubject.Name] = legalTopicSubject.Code;
                    }
                }
            }
            catch
            {
                LogEntry($"Error reading file on path {_filePath}");
                return;
            }

            if (allCodes.Count == 0)
            {
                LogEntry("Could not read topic codes");
                return;
            }

            var updateEntries = new HashSet<string>();
            // read topics
            var topics =
                ((List<Topic>) JsonHelper.Deserialize<List<Topic>>(
                    await cosmosDbService.FindAllItemsAsync(cosmosDbSettings.TopicsCollectionId)))
                .ToDictionary(x => (string)x.Id.ToString(), x => x);
            foreach (var topic in topics.Values)
            {
                if (allCodes.TryGetValue(topic.Name, out var code))
                {
                    topic.NsmiCode = code;
                    updateEntries.Add(topic.Id.ToString());
                }
                else
                {
                    LogEntry($"Could not find code for topic {{ name: {topic.Name}, id: {topic.Id} }}");

                    // has to add empty field
                    topic.NsmiCode = string.Empty;
                    updateEntries.Add(topic.Id.ToString());
                }
            }

            List<Resource> resources = JsonHelper.Deserialize<List<Resource>>(
                await cosmosDbService.FindAllItemsAsync(cosmosDbSettings.ResourcesCollectionId));
            foreach (var resource in resources)
            {
                var topic = findTopic(topics, resource);
                if (topic == null)
                {
                    LogEntry($"Could not find topic with code for resource {{ name: {resource.Name}, id: {resource.ResourceId} }}");

                    // has to add empty field
                    resource.NsmiCode = string.Empty;
                    updateEntries.Add(resource.ResourceId.ToString());
                    continue;
                }

                resource.NsmiCode = topic.NsmiCode;
                updateEntries.Add(resource.ResourceId.ToString());
            }

            foreach (var topic in topics.Values)
            {
                if (updateEntries.Contains(topic.Id))
                {
                    await cosmosDbService.UpdateItemAsync(topic.Id, topic, cosmosDbSettings.TopicsCollectionId);
                }
            }

            foreach (var resource in resources)
            {
                if (updateEntries.Contains(resource.ResourceId.ToString()))
                {
                    await cosmosDbService.UpdateItemAsync(resource.ResourceId.ToString(), resource, cosmosDbSettings.ResourcesCollectionId);
                }
            }
        }

        private Topic findTopic(IReadOnlyDictionary<string, Topic> topics, Resource resource)
        {
            foreach (var tag in resource.TopicTags)
            {
                if (!topics.TryGetValue((string) tag.TopicTags.ToString(), out var topic))
                {
                    continue;
                }
                if (string.IsNullOrWhiteSpace(topic.NsmiCode))
                {
                    continue;
                }

                return topic;
            }

            return null;
        }
    }

    public class LegalTopicSubject
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
