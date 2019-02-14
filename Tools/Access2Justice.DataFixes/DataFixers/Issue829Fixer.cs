using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Access2Justice.DataFixes.DataAccess;
using Access2Justice.DataFixes.Helpers;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;

namespace Access2Justice.DataFixes.DataFixers
{
    public class Issue829Fixer : IDataFixer
    {
        private const string IssueId = "#829";
        private readonly string LogEntryPrefix = $"\t{IssueId}: ";
        private static readonly Regex IconLocationAbsoluteStoragePathRegEx =
            new Regex(@"^https:\/\/[a-zA-Z0-9\-\.]+\.blob\.core\.windows\.net\/static-resource\/([\w- .\/?%&=])+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public async Task ApplyFixAsync(
            CosmosDbSettings cosmosDbSettings,
            CosmosDbService cosmosDbService)
        {

            List<Topic> topics = null;

            var dataBackupFileName = $"{IssueId}_Topics_backup.json";

            using (var writer = new StreamWriter(File.OpenWrite(dataBackupFileName)))
            {
                LogEntry("Loading topics from DB");
                topics = JsonHelper.Deserialize<List<Topic>>(
                    await cosmosDbService.FindAllItemsAsync(cosmosDbSettings.TopicsCollectionId));

                LogEntry($"Loaded {topics.Count} topic(s)");
                var serializer = JsonSerializer.Create();
                serializer.Serialize(writer, topics);
            }

            if (topics == null || topics.Count == 0)
            {
                LogEntry("No topics found, finishing");
                return;
            }

            LogEntry($"Unmodified Topics saved in {dataBackupFileName}");

            var topicsToUpdate = new List<Topic>();
            foreach (var topic in topics)
            {
                var originalIconLocation = topic.Icon;
                var fixedIconLocation = GetFixedIconLocation(originalIconLocation);

                if (originalIconLocation != null &&
                    fixedIconLocation != null &&
                    string.Compare(originalIconLocation, fixedIconLocation, StringComparison.InvariantCultureIgnoreCase) != 0)
                {
                    topic.Icon = fixedIconLocation;
                    topicsToUpdate.Add(topic);

                    LogEntry($"\tTopic {topic.Id} icon will be fixed - original {originalIconLocation} new {fixedIconLocation}");
                }
            }

            if (topicsToUpdate.Count > 0)
            {
                foreach(var topic in topicsToUpdate)
                {
                    LogEntry($"Updating Topic {topic.Id}...");

                    var topicDocument = JsonHelper.Deserialize<object>(topic);
                    await cosmosDbService.UpdateItemAsync(
                        topic.Id,
                        topicDocument,
                        cosmosDbSettings.TopicsCollectionId);

                    LogEntry($"Updated Topic {topic.Id}");
                }

                LogEntry($"Updated {topicsToUpdate.Count} topics");
            }
            else
            {
                LogEntry("No topics to update found");
            }
        }

        private string GetFixedIconLocation(string iconLocation)
        {
            if (!string.IsNullOrEmpty(iconLocation))
            {
                if (iconLocation.IndexOf('|') > 0)
                {
                    return string.Join(
                        " | ",
                        iconLocation.
                            Split('|', StringSplitOptions.RemoveEmptyEntries).
                            Where(l => !string.IsNullOrWhiteSpace(l)).
                            Select(l => GetFixedIconLocation(l.Trim())));
                }
                else
                {
                    if (IconLocationAbsoluteStoragePathRegEx.IsMatch(iconLocation))
                    {
                        return iconLocation.Substring(iconLocation.IndexOf("/static-resource"));
                    }
                }
            }

            return iconLocation;
        }

        private void LogEntry(string entry)
        {
            Console.WriteLine($"{LogEntryPrefix}{entry}");
        }
    }
}
