using Access2Justice.Shared.Helper;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
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
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);

            var query = $"SELECT * FROM c WHERE c.{propertyName}='{value}'";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereAsync(string collectionId, List<string> propertyNames, List<string> values)
        {
            var query = "SELECT * FROM c WHERE ";
            for (int iterator = 0; iterator< propertyNames.Count(); iterator++)
            {
                EnsureParametersAreNotNullOrEmpty(collectionId, propertyNames[iterator]);
                query += $"c.{propertyNames[iterator]}='{values[iterator]}'";
                if (iterator != propertyNames.Count()-1) {
                    query += " AND ";
                }
            }
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

            string arrayContainsClause = ArrayContainsWithOrClause(arrayName, propertyName, values);
            var query = $"SELECT * FROM c WHERE {arrayContainsClause}";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereContainsWithLocationAsync(string collectionId, string propertyName, string value, Location location)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);
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
            EnsureParametersAreNotNullOrEmpty(arrayName, propertyName, andPropertyName, resourceFilter.ResourceType);
            string arrayContainsWithAndClause = ArrayContainsWithOrClause(arrayName, propertyName, resourceFilter.TopicIds);

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

        private dynamic FindLocationWhereArrayContains(Location location)
        {
            if (location == null || (string.IsNullOrEmpty(location.State) && string.IsNullOrEmpty(location.County)
                && string.IsNullOrEmpty(location.City) && string.IsNullOrEmpty(location.ZipCode)))
            {
                return "";
            }
            return ArrayContainsWithMulitpleProperties("location", location);
        }

        private void EnsureParametersAreNotNullOrEmpty(params string[] parameters)
        {
            foreach (var param in parameters)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    throw new ArgumentException("Parameters can not be null or empty spaces.");
                }
            }
        }

        private string ArrayContainsWithMulitpleProperties(string propertyName, dynamic input)
        {
            var jsonSettings = UtilityHelper.JSONSanitizer();

            string arrayContainsClause = JsonConvert.SerializeObject(input, jsonSettings);
            string query = $" (ARRAY_CONTAINS(c.{propertyName},{{0}}))";
            if (!string.IsNullOrEmpty(arrayContainsClause))
            {
                arrayContainsClause = string.Format(CultureInfo.InvariantCulture, query, arrayContainsClause + ",true");
            }
            return arrayContainsClause;
        }

        private string ArrayContainsWithOrClause(string arrayName, string propertyName, IEnumerable<string> values)
        {
            string arrayContainsWithOrClause = string.Empty;
            var lastItem = values.Last();
            foreach (var value in values)
            {
                arrayContainsWithOrClause += $" ARRAY_CONTAINS(c.{arrayName}, {{ '{propertyName}' : '" + value + "'})";
                if (value != lastItem)
                {
                    arrayContainsWithOrClause += "OR";
                }
            }
            return arrayContainsWithOrClause;
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

        public async Task<dynamic> FindItemsWhereInClauseAsync(string collectionId, string propertyName, IEnumerable<string> values)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);

            var inClause = string.Empty;
            var lastItem = values.Last();

            foreach (var value in values)
            {
                inClause += $"'" + value + "'";
                if (value != lastItem)
                {
                    inClause += ",";
                }
            }

            var query = $"SELECT * FROM c WHERE c.{propertyName} IN ({inClause})";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }
    }
}