﻿using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateTopicAndResources
{
    public class Topic_BL
    {

        private const string EndpointUrl = "";
        private const string PrimaryKey = "";
        private const string Database = "access2justicedb";
        private DocumentClient client;

        public async Task<IEnumerable<Topic>> GetTopics()
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "access2justicedb" });

            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("access2justicedb"),
            new DocumentCollection { Id = "Topics" });

            InsertTopics obj = new InsertTopics();
            var content = obj.CreateJsonFromCSV();

            Topics topics = new Topics
            {
                TopicsList = content.TopicsList
            };

            foreach (var topicList in topics.TopicsList)
            {
                int count = topicList.ParentTopicID.Length;
                foreach (var parentId in topicList.ParentTopicID)
                {
                    if (parentId.ParentTopicId != "")
                    {
                        var updatedId = from a in content.ParentTopicList where a.DummyId == parentId.ParentTopicId select a.NewId.ToString();
                        parentId.ParentTopicId = updatedId.FirstOrDefault();
                    }
                }
                await this.CreateTopicDocumentIfNotExists("access2justicedb", "Topics", topicList);
            }
            var items = await this.GetItemsFromCollectionAsync();
            return items;
        }

        private async Task CreateTopicDocumentIfNotExists(string databaseName, string collectionName, Topic td)
        {
            await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), td);
        }

        public async Task<IEnumerable<Topic>> GetItemsFromCollectionAsync()
        {
            var documents = client.CreateDocumentQuery<Topic>(
                  UriFactory.CreateDocumentCollectionUri("access2justicedb", "Topics"),
                  new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();
            List<Topic> td = new List<Topic>();
            while (documents.HasMoreResults)
            {
                td.AddRange(await documents.ExecuteNextAsync<Topic>());
            }
            return td;
        }
    }
}
