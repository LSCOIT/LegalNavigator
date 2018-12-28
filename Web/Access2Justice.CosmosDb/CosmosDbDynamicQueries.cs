using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
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

        public async Task<T> FindItemWhereAsync<T>(string collectionId, string propertyName, string value)
        {
            var result = await FindItemsWhereAsync(collectionId, propertyName, value);
            return (T)result[0];
        }

        public async Task<T> FindItemWhereContainsAsync<T>(string collectionId, string propertyName, string value)
        {
            var result = await FindItemsWhereContainsAsync(collectionId, propertyName, value);
            return (T)result[0];
        }

        public async Task<dynamic> FindItemsWhereAsync(string collectionId, string propertyName, string value)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);

            var query = string.Empty;
            if (string.IsNullOrWhiteSpace(value))
            {
                query = $"SELECT * FROM c WHERE c.{propertyName}=[]";
            }
            else
            {
                query = $"SELECT * FROM c WHERE c.{propertyName}='{value}'";
            }

            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereAsync(string collectionId, List<string> propertyNames, List<string> values)
        {
            var query = "SELECT * FROM c WHERE ";
            for (int iterator = 0; iterator < propertyNames.Count(); iterator++)
            {
                EnsureParametersAreNotNullOrEmpty(collectionId, propertyNames[iterator]);
                query += $"c.{propertyNames[iterator]}='{values[iterator]}'";
                if (iterator != propertyNames.Count() - 1)
                {
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

        public async Task<dynamic> FindItemsWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, string value, bool isPartialQuery)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, arrayName, propertyName);

            if (isPartialQuery)
            {
                return await backendDatabaseService.QueryItemsAsync(collectionId, $"SELECT * FROM c WHERE ARRAY_CONTAINS(c.{arrayName}, {{ '{propertyName}' : '" + value + "'}, true)");
            }

            return await FindItemsWhereArrayContainsAsync(collectionId, arrayName, propertyName, value);
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
            var query = $"SELECT * FROM c WHERE CONTAINS(c.{propertyName}, \"{value}\")";
            if (!string.IsNullOrEmpty(locationFilter))
            {
                query = query + " AND " + locationFilter;
            }
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereWithLocationAsync(string collectionId, string propertyName, string value, Location location)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);
            string locationFilter = FindLocationWhereArrayContains(location);
            var query = string.Empty;
            if (string.IsNullOrEmpty(value) && (location != null))
            {
                query = $"SELECT * FROM c WHERE (c.{propertyName}=[{value}] OR c.{propertyName}=null)";
            }
            else
            {
                query = $"SELECT * FROM c WHERE c.{propertyName}='{value}'";
            }

            if (!string.IsNullOrEmpty(locationFilter))
            {
                query = query + " AND " + locationFilter;
            }
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereWithLocationAsync(string collectionId, string propertyName, Location location)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);
            string locationFilter = FindLocationWhereArrayContains(location);
            var query = string.Empty;
            query = $"SELECT * FROM c";

            if (!string.IsNullOrEmpty(locationFilter))
            {
                query += " WHERE " + locationFilter;
            }
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereArrayContainsWithAndClauseAsync(string arrayName, string propertyName, string andPropertyName, ResourceFilter resourceFilter, bool isResourceCountCall = false)
        {
            EnsureParametersAreNotNullOrEmpty(arrayName, propertyName, andPropertyName, resourceFilter.ResourceType);
            string arrayContainsWithAndClause = ArrayContainsWithOrClause(arrayName, propertyName, resourceFilter.TopicIds);
            if (!string.IsNullOrEmpty(arrayContainsWithAndClause))
            {
                arrayContainsWithAndClause = "(" + arrayContainsWithAndClause + ")";
            }
            if (resourceFilter.ResourceType.ToUpperInvariant() != Constants.ResourceTypeAll && !isResourceCountCall)
            {
                arrayContainsWithAndClause += string.IsNullOrEmpty(arrayContainsWithAndClause) ? $" c.{andPropertyName} = '" + resourceFilter.ResourceType + "'"
                                             : $" AND c.{andPropertyName} = '" + resourceFilter.ResourceType + "'";
            }
            string resourceIsActiveFilter = FindItemsWhereResourceIsActive(resourceFilter.ResourceType);
            if (!string.IsNullOrEmpty(resourceIsActiveFilter))
            {
                arrayContainsWithAndClause = string.IsNullOrEmpty(arrayContainsWithAndClause) ? resourceIsActiveFilter
                                          : arrayContainsWithAndClause + " AND " + resourceIsActiveFilter;
            }
            string locationFilter = FindLocationWhereArrayContains(resourceFilter.Location);
            if (!string.IsNullOrEmpty(locationFilter))
            {
                arrayContainsWithAndClause = string.IsNullOrEmpty(arrayContainsWithAndClause) ? locationFilter
                                          : arrayContainsWithAndClause + " AND " + locationFilter;
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
                if (resourceFilter.IsOrder)
                {
                    if (resourceFilter?.OrderByField == "date")
                    {
                        resourceFilter.OrderByField = "modifiedTimeStamp";
                    }
                    var orderByField = (resourceFilter.OrderByField != null) ? resourceFilter.OrderByField : "name";
                    query = $"SELECT * FROM c WHERE {arrayContainsWithAndClause} order by c.{orderByField} {resourceFilter.OrderBy}";
                }
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

        public async Task<dynamic> FindItemsWhereInClauseAsync(string collectionId, string propertyName, IEnumerable<string> values)
        {
            if (values == null || !values.Any())
            {
                return Constants.EmptyArray;
            }

            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);
            var inClause = InClause(propertyName, values);
            var query = $"SELECT * FROM c WHERE c.{propertyName} IN ({inClause})";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereInClauseAsync(string collectionId, string propertyName, IEnumerable<string> values, Location location)
        {
            if (values == null || !values.Any())
            {
                return Constants.EmptyArray;
            }

            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);
            var inClause = InClause(propertyName, values);
            string locationFilter = FindLocationWhereArrayContains(location);
            var query = $"SELECT * FROM c WHERE c.{propertyName} IN ({inClause})";

            if (!string.IsNullOrEmpty(locationFilter))
            {
                query = query + " AND " + locationFilter;
            }
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        private string InClause(string propertyName, IEnumerable<string> values)
        {
            var inClause = string.Empty;
            var lastItem = values.LastOrDefault();

            foreach (var value in values)
            {
                inClause += $"'" + value + "'";
                if (value != lastItem)
                {
                    inClause += ",";
                }
            }
            return inClause;
        }

        public async Task<dynamic> FindFieldWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, string value, string dateProperty)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);

            var query = $"SELECT c.id, f.url FROM c JOIN f in c.{arrayName} WHERE CONTAINS(f.{propertyName}, '{value}') AND f.{dateProperty} > '{DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture)}'";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindFieldWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, string value)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);

            var query = $"SELECT f.name, f.code FROM c JOIN f in c.{arrayName} WHERE CONTAINS(f.{propertyName}, '{value}')";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindFieldWhereArrayContainsAsync(string collectionId, string propertyName, string value)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);

            var query = $"SELECT c.name, c.firstName, c.lastName, c.oId FROM c WHERE c.{propertyName} = '{value}'";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        private string FindItemsWhereResourceIsActive(string resourceType)
        {
            string resourceIsActiveFilter = string.Empty;
            if (resourceType.ToUpperInvariant() == Constants.GuidedAssistant.ToUpperInvariant())
            {
                resourceIsActiveFilter = $" c.isActive = true";
            }
            else if (resourceType.ToUpperInvariant() == Constants.All)
            {
                resourceIsActiveFilter = $" (c.resourceType != '{Constants.GuidedAssistant}' OR (c.resourceType = '{Constants.GuidedAssistant}' AND c.isActive = true))";
            }
            return resourceIsActiveFilter;
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
            var jsonSettings = JsonUtilities.JSONSanitizer();

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
            var lastItem = values?.Count() > 0 ? values.Last() : "";
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

        public async Task<dynamic> FindItemsWhereArrayContainsAsyncWithLocation(string collectionId, string arrayName, string propertyName, string value, Location location)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, arrayName, propertyName);
            string locationFilter = FindLocationWhereArrayContains(location);
            var query = string.Empty;
            var ids = new List<string> { value };

            if (location == null || (string.IsNullOrEmpty(location.State) && string.IsNullOrEmpty(location.County)
                && string.IsNullOrEmpty(location.City) && string.IsNullOrEmpty(location.ZipCode)))
            {
                return "";
            }
            else
            {
                string arrayContainsClause = ArrayContainsWithOrClause(arrayName, propertyName, ids);
                query = $"SELECT * FROM c WHERE {arrayContainsClause}";
                if (!string.IsNullOrEmpty(locationFilter))
                {
                    query = query + " AND " + locationFilter;
                }
            }
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsAllAsync(string collectionId)
        {
            var query = $"SELECT * FROM c";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query);
        }
    }
}