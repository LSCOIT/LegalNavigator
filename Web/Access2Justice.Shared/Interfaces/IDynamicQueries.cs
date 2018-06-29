using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IDynamicQueries
    {
        Task<dynamic> FindItemsWhereAsync(string collectionId, string propertyName, string value);
        Task<dynamic> FindItemsWhereContainsAsync(string collectionId, string propertyName, string value);
        Task<dynamic> FindItemsWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, string value);
        Task<dynamic> FindItemsWhereArrayContainsAsync(string collectionId, string arrayName, string propertyName, IEnumerable<string> values);
        //Task<dynamic> FindItemsWhereArrayContainsFilterAsync(string collectionId, string arrayName, string propertyName, string value, string filterName, string filterValue);
        //Task<dynamic> FindItemsWhereArrayContainsFilterAsync(string collectionId, string arrayName, string propertyName, IEnumerable<string> values, string filterName, string filterValue);
        Task<dynamic> FindItemsWhereInClauseAsync(string collectionId, string propertyName, IEnumerable<string> values);
    }
}
