using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            EnsureParametersAreNotOrEmpty(collectionId, propertyName);

            var query = $"SELECT * FROM c WHERE c.{propertyName}='{value}'";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereContainsAsync(string collectionId, string propertyName, string value)
        {
            EnsureParametersAreNotOrEmpty(collectionId, propertyName);

            var query = $"SELECT * FROM c WHERE CONTAINS(c.{propertyName}, '{value.ToUpperInvariant()}')";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, string value)
        {
            EnsureParametersAreNotOrEmpty(collectionId, arrayName, propertyName);

            var ids = new List<string> { value };
            return await FindItemsWhereArrayContainsAsync(collectionId, arrayName, propertyName, ids);
        }

        public async Task<dynamic> FindItemsWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, IEnumerable<string> values)
        {
            EnsureParametersAreNotOrEmpty(collectionId, arrayName, propertyName);

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

        public async Task<dynamic> FindItemsWhereContainsWithLocationAsync(string collectionId, string propertyName, string value, Location location)
        {
            EnsureParametersAreNotOrEmpty(collectionId, propertyName);
            string locationFilter = FindLocationWhereArrayContains(location);
            var query = $"SELECT * FROM c WHERE CONTAINS(c.{propertyName}, '{value.ToUpperInvariant()}')";
            if (!string.IsNullOrEmpty(locationFilter))
            {
                query = query + " AND " + locationFilter;
            }
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereArrayContainsWithAndClauseAsync(string arrayName, string propertyName, string andPropertyName, ResourceFilter resourceFilter, bool isResourceCountCall = false)
        {
            EnsureParametersAreNotOrEmpty(arrayName, propertyName, andPropertyName, resourceFilter.ResourceType);
            var arrayContainsWithAndClause = string.Empty;
            var lastItem = resourceFilter.TopicIds.Last();

            foreach (var value in resourceFilter.TopicIds)
            {
                arrayContainsWithAndClause += $" ARRAY_CONTAINS(c.{arrayName}, {{ '{propertyName}' : '" + value + "'})";
                if (value != lastItem)
                {
                    arrayContainsWithAndClause += "OR";
                }
            }

            if (resourceFilter.ResourceType.ToUpperInvariant() != "ALL")
            {
                arrayContainsWithAndClause = "(" + arrayContainsWithAndClause + ")";

                arrayContainsWithAndClause += $" AND c.{andPropertyName} = '" + resourceFilter.ResourceType + "'";
            }
            string locationFilter = FindLocationWhereArrayContains(resourceFilter.Location);
            if (!string.IsNullOrEmpty(locationFilter))
            {
                arrayContainsWithAndClause = arrayContainsWithAndClause + " AND " + locationFilter;
            }

            PagedResources pagedResources = new PagedResources();
            if (isResourceCountCall)
            {
                var query = $"SELECT c.resourceType FROM c WHERE {arrayContainsWithAndClause}";
                
                pagedResources = await backendDatabaseService.QueryResourcesCountAsync(query);
            }
            else
            {
                var query = $"SELECT * FROM c WHERE {arrayContainsWithAndClause}";
                if (resourceFilter.PageNumber == 0)
                {
                    pagedResources = await backendDatabaseService.QueryPagedResourcesAsync(query, "");
                    pagedResources.TopicIds = resourceFilter.TopicIds;
                }
                else
                {
                    pagedResources = await backendDatabaseService.QueryPagedResourcesAsync(query, resourceFilter.ContinuationToken);
                    pagedResources.TopicIds = resourceFilter.TopicIds;
                }
            }
            return pagedResources;
        }

        // need to work on this code for optimization.. it is very bad :(
        private dynamic FindLocationWhereArrayContains(Location location)
        {
            if (location == null)
            {
                return "";
            }
            string locationQuery = string.Empty;
            string query = " (ARRAY_CONTAINS(c.location,{0}))";
            if (!string.IsNullOrEmpty(location.State))
            {
                locationQuery += " 'state' : '" + location.State + "'";
            }
            if (!string.IsNullOrEmpty(location.City))
            {
                if (!string.IsNullOrEmpty(locationQuery))
                {
                    locationQuery += locationQuery + ",";
                }
                locationQuery += " 'city':'" + location.City + "'";
            }
            if (!string.IsNullOrEmpty(location.County))
            {
                if (!string.IsNullOrEmpty(locationQuery))
                {
                    locationQuery += locationQuery + ",";
                }
                locationQuery += " 'county':'" + location.County + "'";
            }
            if (!string.IsNullOrEmpty(location.County))
            {
                if (!string.IsNullOrEmpty(locationQuery))
                {
                    locationQuery += locationQuery + ",";
                }
                locationQuery += " 'zipCode':'" + location.ZipCode + "'";
            }
            if (!string.IsNullOrEmpty(locationQuery))
            {
                locationQuery = string.Format(CultureInfo.InvariantCulture, query, "{" + locationQuery + "},true");
            }
            return locationQuery;
        }

        private void EnsureParametersAreNotOrEmpty(params string[] parameters)
        {
            foreach (var param in parameters)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    throw new ArgumentException("Paramters can not be null or empty spaces.");
                }
            }
        }
    }
}