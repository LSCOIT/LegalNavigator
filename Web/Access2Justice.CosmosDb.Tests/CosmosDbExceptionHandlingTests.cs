using Access2Justice.Shared.Interfaces;
using NSubstitute;
using System;
using System.Collections.Generic;
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
            var settings = Substitute.For<ICosmosDbSettings>();
            dynamicQueries = new CosmosDbDynamicQueries(cosmosDbService, settings);
        }

        [Theory]
        [InlineData("", "PropertyName")]
        [InlineData("CollectionName", "")]
        public void FindItemsWhereShouldFailWhenParametersAreNullOrEmpty(string collection, string property)
        {
            Assert.ThrowsAny<Exception>(() => dynamicQueries.FindItemsWhereAsync(collection, property, Arg.Any<string>()).Result);
        }

        [Fact]
        public void FindItemsWhereContainsShouldAlwaysSendUpperCasePropertyValuesToCosmos()
        {
            // Arrange
            string query = @"SELECT * FROM c WHERE CONTAINS(c.Name, @param0)";

            // Act
            var result = dynamicQueries.FindItemsWhereContainsAsync("TopicsCollections", "Name", "EvicTION").Result;

            // Assert
            cosmosDbService.Received().QueryItemsAsync(Arg.Any<string>(), query, Arg.Any<Dictionary<string, object>>());
        }
    }
}
