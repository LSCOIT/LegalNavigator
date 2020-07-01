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
        private const string _parameterPrefix = "param";
        private static readonly Func<int, string> _parameterName = x => $"{_parameterPrefix}{x}";
        private static string _singleParameterName => _parameterName(0);
        private static readonly Func<object, Dictionary<string, object>> _wrapSingleParam = x => new Dictionary<string, object>{{ $"{_singleParameterName}", x } };

        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly ICosmosDbSettings dbSettings;

        public CosmosDbDynamicQueries(IBackendDatabaseService backendDatabaseService, ICosmosDbSettings dbSettings)
        {
            this.backendDatabaseService = backendDatabaseService;
            this.dbSettings = dbSettings;
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

            string query;
            Dictionary<string, object> sqlParams = null;
            if (string.IsNullOrWhiteSpace(value))
            {
                query = $"SELECT * FROM c WHERE c.{propertyName}=[]";
            }
            else
            {
                value = value.ToLower();
                query = $"SELECT * FROM c WHERE LOWER(c.{propertyName})=@valueToSearch";
                sqlParams = new Dictionary<string, object> { { "valueToSearch", value } };
            }

            return await backendDatabaseService.QueryItemsAsync(collectionId, query, sqlParams);
        }

        public async Task<dynamic> FindItemsWhereAsync(string collectionId, List<string> propertyNames, List<string> values)
        {
            var parameterDictionary = new Dictionary<string, object>();
            var query = "SELECT * FROM c WHERE ";
            for (var iterator = 0; iterator < propertyNames.Count; iterator++)
            {
                EnsureParametersAreNotNullOrEmpty(collectionId, propertyNames[iterator]);

                var paramName = _parameterName(iterator);
                parameterDictionary[paramName] = values[iterator];

                query += $"c.{propertyNames[iterator]}=@{paramName}";
                if (iterator != propertyNames.Count - 1)
                {
                    query += " AND ";
                }
            }

            return await backendDatabaseService.QueryItemsAsync(collectionId, query, parameterDictionary);
        }

        public async Task<dynamic> FindItemsWhereContainsAsync(string collectionId, string propertyName, string value)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);
            
            var query = $"SELECT * FROM c WHERE CONTAINS(c.{propertyName}, @{_singleParameterName})";

            return await backendDatabaseService.QueryItemsAsync(collectionId, query, _wrapSingleParam(value.ToUpperInvariant()));
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
                return await backendDatabaseService.QueryItemsAsync(collectionId, $"SELECT * FROM c WHERE ARRAY_CONTAINS(c.{arrayName}, {{ '{propertyName}' : @{_singleParameterName} }}, true)", _wrapSingleParam(value));
            }

            return await FindItemsWhereArrayContainsAsync(collectionId, arrayName, propertyName, value);
        }

        public async Task<dynamic> FindItemsWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, IEnumerable<string> values)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, arrayName, propertyName);

            var parameters = new Dictionary<string, object>();
            string arrayContainsClause = ArrayContainsWithOrClause(arrayName, propertyName, values as IList<string> ?? values.ToList(), parameters);
            var query = $"SELECT * FROM c WHERE {arrayContainsClause} AND c.resourceType != 'Guided Assistant' AND c.display = 'Yes'";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, parameters);
        }

        public async Task<dynamic> FindItemsWhereContainsWithLocationAsync(string collectionId, string propertyName, string value, Location location, bool ignoreCase = false)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);
            string locationFilter = FindLocationWhereArrayContains(location);
            var searchProperty = $"c.{propertyName}";

            if (ignoreCase)
            {
                searchProperty = $"LOWER({searchProperty})";
            }
            var query = $"SELECT * FROM c WHERE CONTAINS({searchProperty}, @{_singleParameterName})";
            if (!string.IsNullOrEmpty(locationFilter))
            {
                query = query + " AND " + locationFilter;
            }
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, _wrapSingleParam(value));
        }

        public async Task<dynamic> FindResourcesWithLocationAsync(string collectionId, Location location)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId);
            string locationFilter = FindLocationWhereArrayContains(location);
            var query = string.Empty;
            query = $"SELECT * FROM c";

            if (!string.IsNullOrEmpty(locationFilter))
            {
                query += " WHERE " + locationFilter;
            }

            query = query + " AND c.display='Yes' AND c.resourceType != 'Guided Assistant'"; 
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, null);
        }

        public async Task<dynamic> FindResourcesWithCuratedExperiences(string collectionId, Location location)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId);
            string locationFilter = FindLocationWhereArrayContains(location);
            var query = string.Empty;
            query = $"SELECT * FROM c WHERE c.curatedExperienceId != null AND c.isActive = true";

            if (!string.IsNullOrEmpty(locationFilter))
            {
                query += " AND " + locationFilter;
            }
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, null);
        }

        public async Task<dynamic> FindItemsWhereWithLocationAsync(string collectionId, string propertyName, string value, Location location)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);
            string locationFilter = FindLocationWhereArrayContains(location);
            var query = string.Empty;
            if (string.IsNullOrEmpty(value) && (location != null))
            {
                query = $"SELECT * FROM c WHERE (c.{propertyName}=[] OR c.{propertyName}=null)";
            }
            else
            {
                query = $"SELECT * FROM c WHERE c.{propertyName}=@{_singleParameterName}";
            }

            if (!string.IsNullOrEmpty(locationFilter))
            {
                query = query + " AND " + locationFilter;
            }
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, _wrapSingleParam(value));
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
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, null);
        }

        public string BuildQueryWhereArrayContainsWithAndClauseAsync(string arrayName, 
            string propertyName, 
            string andPropertyName, 
            ResourceFilter resourceFilter, Dictionary<string, object> parameters, bool isResourceCountCall = false)
        {
            EnsureParametersAreNotNullOrEmpty(arrayName, propertyName, andPropertyName, resourceFilter.ResourceType);

            var arrayContainsWithAndClause = ArrayContainsWithOrClause(arrayName, propertyName, 
                resourceFilter.TopicIds as IList<string> ?? resourceFilter.TopicIds.ToList(), parameters);
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

            if (isResourceCountCall)
            {
                var query = $"SELECT c.resourceType FROM c WHERE {arrayContainsWithAndClause}";
                return query;
            }
            else
            {
                var query = $"SELECT * FROM c WHERE {arrayContainsWithAndClause}";
                if (resourceFilter.IsOrder)
                {
                    if (resourceFilter.OrderByField == "date")
                    {
                        resourceFilter.OrderByField = "modifiedTimeStamp";
                    }
                    var orderByField = resourceFilter.OrderByField ?? "name";
                    query = $"SELECT * FROM c WHERE {arrayContainsWithAndClause} order by c.{orderByField} {resourceFilter.OrderBy}";
                }

                return query;
            }
        }

        public string BuildQueryWhereArrayContainsWithAndClauseLocation(string arrayName,
            string propertyName,
            string andPropertyName,
            ResourceFilter resourceFilter, Dictionary<string, object> parameters, Location location, bool isResourceCountCall = false)
        {
            EnsureParametersAreNotNullOrEmpty(arrayName, propertyName, andPropertyName, resourceFilter.ResourceType);

            var arrayContainsWithAndClause = ArrayContainsWithOrClause(arrayName, propertyName,
                resourceFilter.TopicIds as IList<string> ?? resourceFilter.TopicIds.ToList(), parameters);
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
            string locationFilter = FindLocationWhereArrayContains(location);
            if (!string.IsNullOrEmpty(locationFilter))
            {
                arrayContainsWithAndClause = string.IsNullOrEmpty(arrayContainsWithAndClause) ? locationFilter
                                          : arrayContainsWithAndClause + " AND " + locationFilter;
            }

            if (isResourceCountCall)
            {
                var query = $"SELECT c.resourceType FROM c WHERE {arrayContainsWithAndClause}" + " AND c.display != 'No'";
                return query;
            }
            else
            {
                var query = $"SELECT * FROM c WHERE {arrayContainsWithAndClause} order by c.name ASC";

                return query;
            }
        }

        public string BuildQueryWhereArrayContainsWithAndClauseLocationSpecificRanking(string arrayName,
            string propertyName,
            string andPropertyName,
            ResourceFilter resourceFilter, Dictionary<string, object> parameters, Location location, string topicName, bool isResourceCountCall = false)
        {
            EnsureParametersAreNotNullOrEmpty(arrayName, propertyName, andPropertyName, resourceFilter.ResourceType);

            var arrayContainsWithAndClause = ArrayContainsWithOrClause(arrayName, propertyName,
                resourceFilter.TopicIds as IList<string> ?? resourceFilter.TopicIds.ToList(), parameters);
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
            string locationFilter = FindLocationWhereArrayContains(location);
            if (!string.IsNullOrEmpty(locationFilter))
            {
                arrayContainsWithAndClause = string.IsNullOrEmpty(arrayContainsWithAndClause) ? locationFilter
                                          : arrayContainsWithAndClause + " AND " + locationFilter;
            }

            if (isResourceCountCall)
            {
                var query = $"SELECT c.resourceType FROM c WHERE {arrayContainsWithAndClause}" + " AND c.display != 'No'";
                return query;
            }
            else
            {
                var query = $"SELECT * FROM c WHERE {arrayContainsWithAndClause} AND c.display = 'Yes' order by c.ranking['{topicName}'] ASC";

                return query;
            }
        }

        public async Task<dynamic> FindItemsWhereArrayContainsWithAndClauseAsync(string arrayName, string propertyName, string andPropertyName, ResourceFilter resourceFilter, bool isResourceCountCall = false)
        {
            var parameters = new Dictionary<string, object>();
            var query = BuildQueryWhereArrayContainsWithAndClauseAsync(arrayName, propertyName, andPropertyName,
                resourceFilter, parameters, isResourceCountCall);
            PagedResources pagedResources;
            if (isResourceCountCall)
            {
                pagedResources = await backendDatabaseService.QueryResourcesCountAsync(query, parameters);
            }
            else
            {
                if (resourceFilter.PageNumber == 0)
                {
                    pagedResources = await backendDatabaseService.QueryPagedResourcesAsync(query, parameters, "");
                    pagedResources.TopicIds = resourceFilter.TopicIds;
                }
                else
                {
                    pagedResources = await backendDatabaseService.QueryPagedResourcesAsync(query, parameters, resourceFilter.ContinuationToken);
                    pagedResources.TopicIds = resourceFilter.TopicIds;
                }
            }
            return pagedResources;
        }

        public async Task<dynamic> FindItemsWhereArrayContainsWithAndClauseLocationAsync(string arrayName, string propertyName, string andPropertyName, ResourceFilter resourceFilter, Location location, bool isResourceCountCall = false)
        {
            //Get topic info, and check the ranking value before assigning the query
            string topicId = string.Empty;
            var parameters = new Dictionary<string, object>();
            string query = string.Empty;
            var ssf = resourceFilter.TopicIds.Count();
            if (ssf == 1)
            {
                topicId = resourceFilter.TopicIds.ElementAt(0);
                Topic topicInfo = await backendDatabaseService.GetItemAsync<Topic>(topicId, dbSettings.TopicsCollectionId);
                query = BuildQueryWhereArrayContainsWithAndClauseLocationSpecificRanking(arrayName, propertyName, andPropertyName,
                    resourceFilter, parameters, location, topicInfo.Name, isResourceCountCall);
            }
            else
            {
                query = BuildQueryWhereArrayContainsWithAndClauseLocation(arrayName, propertyName, andPropertyName,
                    resourceFilter, parameters, location, isResourceCountCall);
            }

            PagedResources pagedResources;
            if (isResourceCountCall)
            {
                pagedResources = await backendDatabaseService.QueryResourcesCountAsync(query, parameters);
            }
            else
            {
                if (resourceFilter.PageNumber == 0)
                {
                    pagedResources = await backendDatabaseService.QueryPagedResourcesAsync(query, parameters, string.Empty);
                    pagedResources.TopicIds = resourceFilter.TopicIds;
                }
                else
                {
                    pagedResources = await backendDatabaseService.QueryPagedResourcesAsync(query, parameters, resourceFilter.ContinuationToken);
                    pagedResources.TopicIds = resourceFilter.TopicIds;
                }
            }

            return pagedResources;
        }

        public async Task<dynamic> FindItemsWhereInClauseAsync(string collectionId, string propertyName, IEnumerable<string> values)
        {
            var valuesCollection = values?.ToList();
            if (values == null || valuesCollection.Count == 0)
            {
                return Constants.EmptyArray;
            }

            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);
            var parameters = new Dictionary<string, object>();
            var inClause = InClause(valuesCollection, parameters);
            var query = $"SELECT * FROM c WHERE c.{propertyName} IN ({inClause})";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, parameters);
        }

        public async Task<dynamic> FindItemsWhereInClauseAsync(string collectionId, string propertyName, IEnumerable<string> values, Location location)
        {
            var valuesCollection = values?.ToList();
            if (values == null || valuesCollection.Count == 0)
            {
                return Constants.EmptyArray;
            }

            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);
            var parameters = new Dictionary<string, object>();
            var inClause = InClause(valuesCollection, parameters);
            string locationFilter = FindLocationWhereArrayContains(location);
            var query = $"SELECT * FROM c WHERE c.{propertyName} IN ({inClause})";

            if (!string.IsNullOrEmpty(locationFilter))
            {
                query = query + " AND " + locationFilter;
            }
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, parameters);
        }

        private string InClause(IEnumerable<string> values, Dictionary<string, object> parametersCollection)
        {
            var counter = 0;
            foreach (var value in values)
            {
                parametersCollection[_parameterName(counter)] = value;
                counter++;
            }
            return string.Join(',', parametersCollection.Keys.Select(v => $"@{v}"));
        }

        public async Task<dynamic> FindFieldWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, string value, string dateProperty)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);

            var query = $"SELECT c.id, f.location, f.url FROM c JOIN f in c.{arrayName} WHERE CONTAINS(f.{propertyName}, @{_singleParameterName}) AND f.{dateProperty} > '{DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture)}'";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, _wrapSingleParam(value));
        }

        public async Task<dynamic> FindFieldWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, string value)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);

            var query = $"SELECT f.name, f.code FROM c JOIN f in c.{arrayName} WHERE CONTAINS(f.{propertyName}, @{_singleParameterName})";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, _wrapSingleParam(value));
        }
        
        public async Task<dynamic> FindAllChildTopicsAsync(string value)
        {
            var query = "SELECT * FROM c WHERE ARRAY_CONTAINS(c.parentTopicId, {\"id\":\"#GUID#\"}, true)".Replace("#GUID#", value);
            return await backendDatabaseService.QueryItemsAsync("Topics", query, _wrapSingleParam(value));
        }

        public async Task<dynamic> FindFieldWhereArrayContainsAsync(string collectionId, string propertyName, string value)
        {
            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);

            var query = $"SELECT c.name, c.firstName, c.lastName, c.oId FROM c WHERE c.{propertyName} = @{_singleParameterName}";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, _wrapSingleParam(value));
        }

        private string FindItemsWhereResourceIsActive(string resourceType)
        {
            string resourceIsActiveFilter = string.Empty;
            if (resourceType.ToUpperInvariant() == Constants.GuidedAssistant.ToUpperInvariant())
            {
                resourceIsActiveFilter = $" c.display = 'Yes'";
            }
            else if (resourceType.ToUpperInvariant() == Constants.All)
            {
                resourceIsActiveFilter = $" c.display = 'Yes'";
            }
            return resourceIsActiveFilter;
        }

        /// <summary>
        /// For now, forms with Json.SerializeObject + string.Format
        /// TODO: change to sqlParameters
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private dynamic FindLocationWhereArrayContains(Location location)
        {
            if (location == null || (string.IsNullOrEmpty(location.State) && string.IsNullOrEmpty(location.County)
                && string.IsNullOrEmpty(location.City) && string.IsNullOrEmpty(location.ZipCode)))
            {
                return "";
            }
            return ArrayContainsWithMultipleProperties("location", location, exactForDefaults: true);
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

        private string ArrayContainsWithMultipleProperties(
            string propertyName,
            dynamic input,
            bool exactForDefaults = false)
        {
            var jsonSettings = JsonUtilities.JSONSanitizer();

            if (exactForDefaults)
            {
                jsonSettings.DefaultValueHandling = DefaultValueHandling.Include;
            }

            string arrayContainsClause = JsonConvert.SerializeObject(input, jsonSettings);
            string query = $" (ARRAY_CONTAINS(c.{propertyName},{{0}}))";
            if (!string.IsNullOrEmpty(arrayContainsClause))
            {
                arrayContainsClause = string.Format(CultureInfo.InvariantCulture, query, arrayContainsClause + ",true");
            }
            return arrayContainsClause;
        }

        private string ArrayContainsWithOrClause(string arrayName, string propertyName, ICollection<string> values, Dictionary<string, object> parameters)
        {
            var arrayContainsWithOrClause = string.Empty;
            var lastItem = values.Count > 0 ? values.Last() : "";
            var index = 0;
            foreach (var value in values)
            {
                var parameterName = _parameterName(index);
                index++;
                parameters[parameterName] = value;
                arrayContainsWithOrClause += $" ARRAY_CONTAINS(c.{arrayName}, {{ '{propertyName}' : @{parameterName} }},true)";
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
            var ids = new List<string> { value };

            if (location == null || (string.IsNullOrEmpty(location.State) && string.IsNullOrEmpty(location.County)
                && string.IsNullOrEmpty(location.City) && string.IsNullOrEmpty(location.ZipCode)))
            {
                return "";
            }

            var parameters = new Dictionary<string, object>();
            string arrayContainsClause = ArrayContainsWithOrClause(arrayName, propertyName, ids, parameters);
            var query = $"SELECT * FROM c WHERE {arrayContainsClause}";
            if (!string.IsNullOrEmpty(locationFilter))
            {
                query = query + " AND " + locationFilter;
            }
            query = query + " AND c.display = 'Yes' AND c.resourceType != 'Guided Assistant' ";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, parameters);
        }

        public async Task<dynamic> FindItemsAllAsync(string collectionId)
        {
            var query = "SELECT * FROM c";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, null);
        }

        public async Task<dynamic> FindTopicsByTopicIds(string collectionId, string propertyName, IEnumerable<string> topicsIds)
        {
            var topicsCollection = topicsIds?.Distinct().ToList();
            if (topicsIds == null || topicsCollection.Count == 0)
            {
                return Constants.EmptyArray;
            }

            EnsureParametersAreNotNullOrEmpty(collectionId, propertyName);
            var parameters = new Dictionary<string, object>();
            var inClause = InClause(topicsCollection, parameters);
            var query = $"SELECT * FROM c WHERE c.{propertyName} IN ({inClause})";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, parameters);
        }

        public async Task<dynamic> FindCuratedExperiences(string collectionId, IEnumerable<string> curatedExperienceIds)
        {
            var curatedExpCollection = curatedExperienceIds?.Distinct().ToList();
            if (curatedExperienceIds == null || curatedExpCollection.Count == 0)
            {
                return Constants.EmptyArray;
            }

            var parameters = new Dictionary<string, object>();
            var inClause = InClause(curatedExpCollection, parameters);
            var query = $"SELECT * FROM c WHERE c.id IN ({inClause})";
            return await backendDatabaseService.QueryItemsAsync(collectionId, query, parameters);
        }

        public async Task DeleteResource(string collectionId, string id, string partitionKey)
        {
            await backendDatabaseService.DeleteResourceAsync(id, partitionKey, collectionId);
        }
    }
}