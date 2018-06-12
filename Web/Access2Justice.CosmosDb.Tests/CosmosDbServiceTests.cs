using Access2Justice.Shared.Interfaces;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Access2Justice.CosmosDb.Tests
{
    // todo: implement IDisposable and consolidate test methods into one class
    public class CosmosDbServiceTests1
    {
        private readonly IBackendDatabaseService cosmosDbService;

        public CosmosDbServiceTests1()
        {
            cosmosDbService = Substitute.For<IBackendDatabaseService>();
        }

        [Fact]
        public void FindItemsWhere_ShouldConstructValidSqlQuery()
        {
            // Arrange
            var sut = new CosmosDbDynamicQueries(cosmosDbService);
            string query = @"SELECT * FROM c WHERE c.name='eviction'";

            // Act
            var result = sut.FindItemsWhere("topicsCollections", "name", "eviction").Result;


            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);
        }
    }

    public class CosmosDbServiceTests2
    {
        private readonly IBackendDatabaseService cosmosDbService;

        public CosmosDbServiceTests2()
        {
            cosmosDbService = Substitute.For<IBackendDatabaseService>();
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

    }

    public class CosmosDbServiceTests3
    {
        private readonly IBackendDatabaseService cosmosDbService;

        public CosmosDbServiceTests3()
        {
            cosmosDbService = Substitute.For<IBackendDatabaseService>();
        }

        [Theory]
        [InlineData("", "PropertyName")]
        [InlineData("CollectionName", "")]
        public void FindItemsWhere_ShouldFail_WhenParametersAreNullOrEmpty(string collection, string property)
        {
            var sut = new CosmosDbDynamicQueries(cosmosDbService);

            Assert.ThrowsAny<Exception>(() => sut.FindItemsWhere(collection, property, Arg.Any<string>()).Result);
        }
    }

    public class CosmosDbServiceTests4
    {
        private readonly IBackendDatabaseService cosmosDbService;

        public CosmosDbServiceTests4()
        {
            cosmosDbService = Substitute.For<IBackendDatabaseService>();
        }

        [Fact]
        public void FindItemsWhereArrayContains_ShouldConstructValidSqlQuery()
        {
            // Arrange
            var sut = new CosmosDbDynamicQueries(cosmosDbService);
            var ids = new List<string>() { "guid1", "guid2", "guid2" };
            string query = @"SELECT * FROM c WHERE  ARRAY_CONTAINS(c.TopicTags, { 'Id' : 'guid1'})OR ARRAY_CONTAINS(c.TopicTags, { 'Id' : 'guid2'}) ARRAY_CONTAINS(c.TopicTags, { 'Id' : 'guid2'})";

            // Act
            sut.FindItemsWhereArrayContains("TopicsCollections", "TopicTags", "Id", ids);

            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);
        }
    }
}
    