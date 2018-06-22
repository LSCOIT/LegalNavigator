using Access2Justice.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.CosmosDb
{
    public class CosmosDbDynamicQueries : IDynamicQueries
    {
        private readonly IBackendDatabaseService backendDatabaseService;

        public CosmosDbDynamicQueries(IBackendDatabaseService backendDatabaseService)
        {
            this.backendDatabaseService = backendDatabaseService;
        }

        public async Task<dynamic> FindItemsWhereAsync(string collectionId, string propertyName, string value)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);

            var query = $"SELECT * FROM c WHERE c.{propertyName}='{value}'";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereContainsAsync(string collectionId, string propertyName, string value)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);

            var query = $"SELECT * FROM c WHERE CONTAINS(c.{propertyName}, '{value.ToUpperInvariant()}')";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, string value)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, arrayName, propertyName);

            var ids = new List<string> { value };
            return await FindItemsWhereArrayContainsAsync(collectionId, arrayName, propertyName, ids);
        }

        public async Task<dynamic> FindItemsWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, IEnumerable<string> values)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, arrayName, propertyName);

            var arrayContainsClause = string.Empty;
            var lastItem = values.Last();

            foreach (var value in values)
            {
                arrayContainsClause += $" ARRAY_CONTAINS(c.{arrayName}, {{ '{propertyName}' : '" + value + "'})";
                if (value != lastItem)
                {
                    arrayContainsClause += "OR";
                }
            }

            var query = $"SELECT * FROM c WHERE {arrayContainsClause}";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        private void EnsureParametersAreNotNullOrEmpty(params string[] parameters)
        {
            foreach (var param in parameters)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    throw new ArgumentException("Paramters can not be null or empty spaces.");
                }
            }
        }

        public async Task<dynamic> FindItemsWhereArrayContainsFilterAsync(string collectionId, string arrayName, string propertyName, string value, string filterName, string filterValue)
        {
            //EnsureParametersAreNotOrEmpty(collectionId, arrayName, propertyName);

            var ids = new List<string> { value };
            return await FindItemsWhereArrayContainsFilterAsync(collectionId, arrayName, propertyName, ids, filterName, filterValue);
        }

        public async Task<dynamic> FindItemsWhereArrayContainsFilterAsync(string collectionId, string arrayName, string propertyName, IEnumerable<string> values, string filterName, string filterValue)
        {
            //EnsureParametersAreNotOrEmpty(collectionId, arrayName, propertyName);

            var arrayContainsClause = string.Empty;
            var filterClause = string.Empty;
            var lastItem = values.Last();

            foreach (var value in values)
            {
                arrayContainsClause += $" ARRAY_CONTAINS(c.{arrayName}, {{ '{propertyName}' : '" + value + "'})";
                if (value != lastItem)
                {
                    arrayContainsClause += "OR";
                }
            }

            filterClause += " AND" + $" c.{filterName} = '" + filterValue + "'";
            var query = $"SELECT * FROM c WHERE {arrayContainsClause} {filterClause}";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }
    }
}