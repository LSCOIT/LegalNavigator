using Access2Justice.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IDynamicQueries
    {
        Task<dynamic> FindItemsWhere(string collectionId, string propertyName, string value);
        Task<dynamic> FindItemsWhereContains(string collectionId, string propertyName, string value);
        Task<dynamic> FindItemsWhereArrayContains(string collectionId, string arrayName, string propertyName, string value);
        Task<dynamic> FindItemsWhereArrayContains(string collectionId, string arrayName, string propertyName, IEnumerable<string> values);

        dynamic FindLocationWhereArrayContains(Location location);
        dynamic FindOrganizationsWhereArrayContains(string collectionId, Location location);

        
    }
}
