using Access2Justice.Shared.Interfaces;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace Access2Justice.CosmosDb.Tests
{
    public class CosmosDbDynmaicQueries
    {
        private readonly IBackendDatabaseService cosmosDbService;
        private readonly IDynamicQueries dynamicQueries;

        public CosmosDbDynmaicQueries()
        {
            cosmosDbService = Substitute.For<IBackendDatabaseService>();
            dynamicQueries = new CosmosDbDynamicQueries(cosmosDbService);
        }

        [Fact]
        public void FindItemsWhereContainsShouldConstructValidSqlQuery()
        {
            // Arrange
            string query = @"SELECT * FROM c WHERE CONTAINS(c.Name, 'EVICTION')";

            // Act
            var result = dynamicQueries.FindItemsWhereContains("TopicsCollections", "Name", "EVICTION").Result;

            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);
        }

        [Fact]
        public void FindItemsWhereShouldConstructValidSqlQuery()
        {
            // Arrange
            var sut = new CosmosDbDynamicQueries(cosmosDbService);
            string query = @"SELECT * FROM c WHERE c.name='eviction'";

            // Act
            var result = sut.FindItemsWhere("topicsCollections", "name", "eviction").Result;

            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);
        }

        [Fact]
        public void FindItemsWhereArrayContainsShouldConstructValidSqlQuery()
        {
            // Arrange
            var ids = new List<string>() { "guid1", "guid2", "guid2" };
            string query = @"SELECT * FROM c WHERE  ARRAY_CONTAINS(c.TopicTags, { 'Id' : 'guid1'})OR ARRAY_CONTAINS(c.TopicTags, { 'Id' : 'guid2'}) ARRAY_CONTAINS(c.TopicTags, { 'Id' : 'guid2'})";

            // Act
            dynamicQueries.FindItemsWhereArrayContains("TopicsCollections", "TopicTags", "Id", ids);

            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);
        }
    }
}
