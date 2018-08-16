using Access2Justice.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IDynamicQueries
    {
        Task<dynamic> FindItemsWhereAsync(string collectionId, string propertyName, string value);
        Task<dynamic> FindItemsWhereAsync(string collectionId, List<string> propertyNames, List<string> values);
        Task<dynamic> FindItemsWhereContainsAsync(string collectionId, string propertyName, string value);
        Task<dynamic> FindItemsWhereContainsWithLocationAsync(string collectionId, string propertyName, string value,Location location);
        Task<dynamic> FindItemsWhereWithLocationAsync(string collectionId, string propertyName, string value, Location location);
        Task<dynamic> FindItemsWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, string value);
        Task<dynamic> FindItemsWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, IEnumerable<string> values);
        Task<dynamic> FindItemsWhereArrayContainsWithAndClauseAsync(string arrayName, string propertyName, string andPropertyName, ResourceFilter resourceFilter,bool isResourceCountCall= false);
        Task<dynamic> FindItemsWhereInClauseAsync(string collectionId, string propertyName, IEnumerable<string> values);
        Task<dynamic> FindItemsWhereArrayContainsAsyncWithLocation(string collectionId, string arrayName, string propertyName, string value, Location location); 
    }
}
