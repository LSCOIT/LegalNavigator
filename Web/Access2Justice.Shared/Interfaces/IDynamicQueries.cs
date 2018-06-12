using Access2Justice.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IDynamicQueries
    {
        Task<dynamic> QueryItemsAsync(string collectionId, string query);
        Task<dynamic> FindItemsWhere(string collectionId, string propertyName, string value);
        Task<dynamic> FindItemsWhereContainsWithLocation(string collectionId, string propertyName, string value,Location location);
        Task<dynamic> FindItemsWhereArrayContains(string collectionId, string arrayName, string propertyName, string value);
        Task<dynamic> FindItemsWhereArrayContains(string collectionId, string arrayName, string propertyName, IEnumerable<string> values);
        dynamic FindItemsWhereArrayContainsWithAndClause(string arrayName, string propertyName, string andPropertyName, string andPropertyValue, IEnumerable<string> values,Location location);
        Task<dynamic> QueryPagedResourcesAsync(string query, string continuationToken);
        Task<dynamic> QueryResourcesCountAsync(string query);
    }
}
