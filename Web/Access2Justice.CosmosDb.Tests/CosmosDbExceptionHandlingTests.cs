using Access2Justice.Shared.Interfaces;
using NSubstitute;
using System;
using Xunit;

namespace Access2Justice.CosmosDb.Tests
{
    public class CosmosDbExceptionHandlingTests
    {
        private readonly IBackendDatabaseService cosmosDbService;
        private readonly IDynamicQueries dynamicQueries;

        public CosmosDbExceptionHandlingTests()
        {
            cosmosDbService = Substitute.For<IBackendDatabaseService>();
            dynamicQueries = new CosmosDbDynamicQueries(cosmosDbService);
        }

        [Theory]
        [InlineData("", "PropertyName")]
        [InlineData("CollectionName", "")]
        public void FindItemsWhereShouldFailWhenParametersAreNullOrEmpty(string collection, string property)
        {
            Assert.ThrowsAny<Exception>(() => dynamicQueries.FindItemsWhere(collection, property, Arg.Any<string>()).Result);
        }

        [Fact]
        public void FindItemsWhereContainsShouldAlwaysSendUpperCasePropertyValuesToCosmos()
        {
            // Arrange
            string query = @"SELECT * FROM c WHERE CONTAINS(c.Name, 'EVICTION')";

            // Act
            var result = dynamicQueries.FindItemsWhereContains("TopicsCollections", "Name", "EvicTION").Result;


            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query);
        }
    }
}
