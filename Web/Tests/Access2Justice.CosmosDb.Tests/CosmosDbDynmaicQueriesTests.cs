using Access2Justice.Shared.Interfaces;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace Access2Justice.CosmosDb.Tests
{
    public class CosmosDbDynmaicQueriesTests
    {
        private readonly IBackendDatabaseService cosmosDbService;
        private readonly IDynamicQueries dynamicQueries;

        public CosmosDbDynmaicQueriesTests()
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
            var result = dynamicQueries.FindItemsWhereContainsAsync("TopicsCollections", "Name", "EVICTION").Result;

            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);
        }

        [Fact]
        public void FindItemsWhereShouldConstructValidSqlQuery()
        {
            // Arrange
            string query = @"SELECT * FROM c WHERE c.name='eviction'";

            // Act
            var result = dynamicQueries.FindItemsWhereAsync("topicsCollections", "name", "eviction").Result;

            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);
        }

        [Fact]
        public void FindItemsWhereShouldConstructValidSqlQueryWith2Inputs()
        {
            // Arrange
            string query = @"SELECT * FROM c WHERE c.name='eviction form' AND c.resourceType = 'Forms'";

            // Act
            var result = dynamicQueries.FindItemsWhereAsync("topicsCollections", "name", "eviction form","resourceType","Forms").Result;

            // Assert
            cosmosDbService.ReceivedWithAnyArgs().QueryItemsAsync(Arg.Any<string>(), query);
        }


        [Fact]
        public void FindItemsWhereArrayContainsShouldConstructValidSqlQuery()
        {
            // Arrange
            var ids = new List<string>() { "guid1", "guid2", "guid2" };
            string query = @"SELECT * FROM c WHERE  ARRAY_CONTAINS(c.TopicTags, { 'Id' : 'guid1'})OR ARRAY_CONTAINS(c.TopicTags, { 'Id' : 'guid2'}) ARRAY_CONTAINS(c.TopicTags, { 'Id' : 'guid2'})";

            // Act
            dynamicQueries.FindItemsWhereArrayContainsAsync("TopicsCollections", "TopicTags", "Id", ids);

            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);
        }
    }
}
