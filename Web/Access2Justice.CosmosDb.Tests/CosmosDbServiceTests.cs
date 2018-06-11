using Xunit;
using NSubstitute;
using System.Collections.Generic;
using System.Net.Http;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Microsoft.Azure.Documents;
using Access2Justice.CosmosDb.Interfaces;
using System;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Azure.Documents.Client;

namespace Access2Justice.CosmosDb.Tests
{
    public class CosmosDbServiceTests
    {
        private readonly IBackendDatabaseService cosmosDbService;
        //private readonly IDocumentClient documentClient;
        //private readonly ICosmosDbSettings cosmosDbSettings;

        public CosmosDbServiceTests()
        {
            cosmosDbService = Substitute.For<IBackendDatabaseService>();
            //documentClient = Substitute.For<IDocumentClient>();
            //cosmosDbSettings = Substitute.For<ICosmosDbSettings>();
        }

        [Fact]
        public void FindItemsWhere_ShouldConstructValidSqlQuery()
        {
            // Arrange
            var sut = new CosmosDbDynamicQueries(cosmosDbService);
            string query = @"SELECT * FROM c WHERE c.Name='Eviction'";

            // Act
            var result = sut.FindItemsWhere("TopicsCollections", "Name", "eviction").Result;


            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);
        }

        [Fact]
        public void FindItemsWhereContains_ShouldConstructValidSqlQuery()
        {
            // Arrange
            var sut = new CosmosDbDynamicQueries(cosmosDbService);
            string query = @"SELECT * FROM c WHERE CONTAINS(c.Name, 'EVICTION')";

            // Act
            var result = sut.FindItemsWhereContains("TopicsCollections", "Name", "EvicTION").Result;


            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);
        }

        [Theory]
        [InlineData("", "Name", "Eviction")]
        [InlineData("Topic", "", "Eviction")]
        [InlineData("Topic", "Name", "")]
        public void FindItemsWhere_ShouldFail_WhenParametersAreNullOrEmpty(string collection, string property, string value)
        {
            var sut = new CosmosDbDynamicQueries(cosmosDbService);

            Assert.ThrowsAny<Exception>(() => sut.FindItemsWhere(collection, property, value).Result);            
        }
    }
}