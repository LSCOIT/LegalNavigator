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


        public dynamic FindLocationWhereArrayContains(Location location)
        {
            if (location == null || (string.IsNullOrEmpty(location.State) && string.IsNullOrEmpty(location.County)
                && string.IsNullOrEmpty(location.City) && string.IsNullOrEmpty(location.ZipCode)))
            {
                return "";
            }
            var jsonSettings = UtilityHelper.JSONSanitizer();

            string locationQuery = JsonConvert.SerializeObject(location, jsonSettings);
            string query = " ARRAY_CONTAINS(c.location,{0})";
            if (!string.IsNullOrEmpty(locationQuery))
            {
                locationQuery = string.Format(CultureInfo.InvariantCulture, query, locationQuery + ",true");
            }
            return locationQuery;
        }

        public async Task<dynamic> FindOrganizationsWhereArrayContains(string collectionId, Location location)
        {
            string result = FindLocationWhereArrayContains(location);
            if (!string.IsNullOrEmpty(result)) {
                result = " AND " + result;
            }
            var query = $"SELECT * FROM c WHERE c.resourceType='Organizations' {result}";
            return await  backendDatabaseService.QueryItemsAsync(collectionId, query);
        }



    }
}