using A2JDbCreator.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            Console.WriteLine("Starting DB creation -");

            // read configs
            var dbSettings = new DbSettings();
            configuration.GetSection("DbSettings").Bind(dbSettings);
            var dbCollections = new List<DbCollection>();
            configuration.GetSection("DbCollections").Bind(dbCollections);

            // run the creator
            CreateAsync(dbSettings, dbCollections);

            Console.WriteLine("All DBs were created successfully. Click any key to exit.");
            Console.ReadKey();
        }

        private static async System.Threading.Tasks.Task CreateAsync(DbSettings dbSettings, List<DbCollection> collections)
        {
            using (var client = new DocumentClient(new Uri(dbSettings.Endpoint), dbSettings.AuthKey))
            {
                var db = new Database { Id = dbSettings.Name };
                await client.CreateDatabaseAsync(db);

                var coll = new DocumentCollection { Id = "Resources" };
                coll.PartitionKey.Paths.Add("/organizationalUnit");

                var dbUri = UriFactory.CreateDatabaseUri("DevDb");
                await client.CreateDocumentCollectionAsync(dbUri, coll, new RequestOptions { OfferThroughput = 20000 });
            }
            Console.WriteLine(dbSettings.Name);
            Console.WriteLine(collections.First().Name);


            Console.WriteLine();
        }
    }
}
