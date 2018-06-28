using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using NSubstitute;
using System;
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
        public void FindItemsWhereArrayContainsShouldConstructValidSqlQuery()
        {
            // Arrange
            var ids = new List<string>() { "guid1", "guid2", "guid3" };
            string query = @"SELECT * FROM c WHERE  ARRAY_CONTAINS(c.TopicTags, { 'Id' : 'guid1'})OR ARRAY_CONTAINS(c.TopicTags, { 'Id' : 'guid2'})OR ARRAY_CONTAINS(c.TopicTags, { 'Id' : 'guid3'})";

            // Act
            dynamicQueries.FindItemsWhereArrayContainsAsync("TopicsCollections", "TopicTags", "Id", ids);

            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);
        }

        [Fact]
        public void FindItemsWhereContainsWithLocationAsyncShouldConstructValidSqlQueryWithoutLocation()
        {
            // Arrange            
            string query = @"SELECT * FROM c WHERE CONTAINS(c.keywords, 'EVICTION')";

            //Act
            dynamicQueries.FindItemsWhereContainsWithLocationAsync("TopicsCollections", "keywords", "eviction", new Location());

            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);

        }

        [Fact]
        public void FindItemsWhereContainsWithLocationAsyncShouldConstructValidSqlQueryWithLocationCondition()
        {
            // Arrange
            string query = "SELECT * FROM c WHERE CONTAINS(c.keywords, 'EVICTION') AND  (ARRAY_CONTAINS(c.location,{\"state\":\"Hawaii\"},true))";

            //Act
            dynamicQueries.FindItemsWhereContainsWithLocationAsync("TopicsCollections", "keywords", "eviction", new Location() { State = "Hawaii" });

            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);

        }

        [Fact]
        public void FindItemsWhereContainsWithLocationAsyncShouldThrowException()
        {
            //Act
            var result = dynamicQueries.FindItemsWhereContainsWithLocationAsync("TopicsCollections", "", "eviction", new Location() { State = "Hawaii" }).Exception;

            // Assert
            Assert.Contains("Parameters can not be null or empty spaces.",result.Message,StringComparison.InvariantCulture);
        }

        [Fact]
        public void FindItemsWhereArrayContainsWithAndClauseAsyncShouldVaildSqlQuery()
        {
            // Arrange
            var ids = new List<string>() { "guid1" };
            ResourceFilter resourceFilter = new ResourceFilter { TopicIds = ids, PageNumber = 0, ResourceType = "All" };
            string query = @"SELECT * FROM c WHERE  ARRAY_CONTAINS(c.topicTags, { 'id' : 'guid1'})";
            

            //Act
            dynamicQueries.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter);

            // Assert
            cosmosDbService.Received().QueryPagedResourcesAsync(query, "");            

        }

        [Fact]
        public void FindItemsWhereArrayContainsWithAndClauseAsyncShouldVaildSqlQueryWithLocation()
        {
            // Arrange
            var ids = new List<string>() { "guid1" };
            Location location = new Location { State = "Hawaii" };
            ResourceFilter resourceFilter = new ResourceFilter { TopicIds = ids, PageNumber = 0, ResourceType = "All",Location = location };
            string query = "SELECT * FROM c WHERE  ARRAY_CONTAINS(c.topicTags, { 'id' : 'guid1'}) AND  (ARRAY_CONTAINS(c.location,{\"state\":\"Hawaii\"},true))";


            //Act
            dynamicQueries.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter);

            // Assert
            cosmosDbService.Received().QueryPagedResourcesAsync(query, "");

        }

        [Fact]
        public void FindItemsWhereArrayContainsWithAndClauseAsyncShouldVaildSqlQueryWithContinuationToken()
        {
            // Arrange
            var ids = new List<string>() { "guid1" };
            Location location = new Location { State = "Hawaii" };
            ResourceFilter resourceFilter = new ResourceFilter { TopicIds = ids, PageNumber = 2, ResourceType = "All", Location = location, ContinuationToken = "222" };
            string query = "SELECT * FROM c WHERE  ARRAY_CONTAINS(c.topicTags, { 'id' : 'guid1'}) AND  (ARRAY_CONTAINS(c.location,{\"state\":\"Hawaii\"},true))";


            //Act
            dynamicQueries.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter);

            // Assert
            cosmosDbService.Received().QueryPagedResourcesAsync(query, resourceFilter.ContinuationToken);

        }

        [Fact]
        public void FindItemsWhereArrayContainsWithAndClauseAsyncShouldVaildSqlQueryWithLocationAndResourceType()
        {
            // Arrange
            var ids = new List<string>() { "guid1" };
            Location location = new Location { State = "Hawaii" };
            ResourceFilter resourceFilter = new ResourceFilter { TopicIds = ids, PageNumber = 0, ResourceType = "Forms", Location = location };
            string query = "SELECT * FROM c WHERE ( ARRAY_CONTAINS(c.topicTags, { 'id' : 'guid1'})) AND c.resourceType = 'Forms' AND  (ARRAY_CONTAINS(c.location,{\"state\":\"Hawaii\"},true))";
            //Act
            dynamicQueries.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter);

            // Assert
            cosmosDbService.Received().QueryPagedResourcesAsync(query, "");

        }

        [Fact]
        public void FindItemsWhereArrayContainsWithAndClauseAsyncShouldVaildSqlQueryWithProperLocationDetails()
        {
            // Arrange
            var ids = new List<string>() { "guid1" };
            Location location = new Location { State = "Hawaii", City = "Honolulu", County = "Honolulu", ZipCode = "96801" };
            ResourceFilter resourceFilter = new ResourceFilter { TopicIds = ids, PageNumber = 0, ResourceType = "Forms", Location = location };
            string query = "SELECT * FROM c WHERE ( ARRAY_CONTAINS(c.topicTags, { 'id' : 'guid1'})) AND c.resourceType = 'Forms' AND  (ARRAY_CONTAINS(c.location,{\"state\":\"Hawaii\",\"city\":\"Honolulu\",\"county\":\"Honolulu\",\"zipCode\":\"96801\"},true))";                            

            //Act
            dynamicQueries.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter);

            // Assert
            cosmosDbService.Received().QueryPagedResourcesAsync(query, "");

        }

        [Fact]
        public void FindItemsWhereArrayContainsWithAndClauseAsyncShouldVaildSqlQueryForResourceCount()
        {
            // Arrange
            var ids = new List<string>() { "guid1" };
            Location location = new Location { State = "Hawaii" };
            ResourceFilter resourceFilter = new ResourceFilter { TopicIds = ids, PageNumber = 0, ResourceType = "ALL", Location = location };
            string query = "SELECT c.resourceType FROM c WHERE  ARRAY_CONTAINS(c.topicTags, { 'id' : 'guid1'}) AND  (ARRAY_CONTAINS(c.location,{\"state\":\"Hawaii\"},true))";                           

            //Act
            dynamicQueries.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter, true);

            // Assert
            cosmosDbService.Received().QueryResourcesCountAsync(query);

        }

    }
}
