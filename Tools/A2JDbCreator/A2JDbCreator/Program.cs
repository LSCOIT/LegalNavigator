using A2JDbCreator.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace A2JDbCreator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // setup configurations
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();

            // read configs
            var dbSettings = new DbSettings();
            configuration.GetSection("DbSettings").Bind(dbSettings);
            var dbCollections = new List<DbCollection>();
            configuration.GetSection("DbCollections").Bind(dbCollections);

            // run the creator
            CreateAsync(dbSettings, dbCollections).Wait();
            Console.ReadKey();
        }

        private static async Task CreateAsync(DbSettings dbSettings, List<DbCollection> collections)
        {
            using (var client = new DocumentClient(new Uri(dbSettings.Endpoint), dbSettings.AuthKey))
            {
                // create databae if it doesn't exist
                Console.Write($"Creating '{dbSettings.Name}'");
                var db = new Database { Id = dbSettings.Name };
                await client.CreateDatabaseIfNotExistsAsync(db);
                Console.WriteLine("...finished successfully.");

                // create collection(s) if not exist
                foreach (var collection in collections)
                {
                    Console.Write($"Creating '{collection.Name}'");
                    var coll = new DocumentCollection { Id = collection.Name };
                    coll.PartitionKey.Paths.Add(collection.PartitionKey);

                    var dbUri = UriFactory.CreateDatabaseUri(dbSettings.Name);
                    await client.CreateDocumentCollectionIfNotExistsAsync(dbUri, coll, new RequestOptions { OfferThroughput = collection.Throughput });
                    Console.WriteLine("...finished successfully.");
                }
            }

            Console.WriteLine("You are all set. Click any key to exit.");
        }
    }
}
