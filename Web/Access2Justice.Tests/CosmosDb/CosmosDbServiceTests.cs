namespace Access2Justice.Tests.CosmosDb
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using NSubstitute;
    using Access2Justice.CosmosDb;
    using Access2Justice.Shared.Interfaces;
    using Microsoft.Azure.Documents;
    using System.IO;

    [TestFixture]
    public class CosmosDbServiceTests
    {
        private ICosmosDbConfigurations config;
        private IConfigurationManager configurationManager;
        private IDocumentClient documentClient;
        private CosmosDbService cosmosDbService;

        [SetUp]
        public void SetUp()
        {
            config = Substitute.For<ICosmosDbConfigurations>();
            configurationManager = Substitute.For<IConfigurationManager>();
            documentClient = Substitute.For<IDocumentClient>();          
            CosmosDbConfigurations cosmosDbConfigurations = new CosmosDbConfigurations();
            config.AuthKey = "authkey";
            config.Endpoint = "http://bing.com";
            config.DatabaseId = "DBid";
            config.CollectionId = "Cid";
            configurationManager.Bind<CosmosDbConfigurations>("","").Returns(config);
            cosmosDbService = new CosmosDbService(documentClient,configurationManager);
        }

        //[Test]
        public void ReadDbConnection()
        {
            var resp = cosmosDbService.GetTopicsFromCollectionAsync("pleaseignore");
        }
    }
}
