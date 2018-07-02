using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IBackendDatabaseService
    {
        Task<Document> CreateItemAsync<T>(T item);
        Task DeleteItemAsync(string id);
        Task<T> GetItemAsync<T>(string id);
        Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate,string collectionId);
        Task<Document> UpdateItemAsync<T>(string id, T item);
        Task<dynamic> QueryItemsAsync(string collectionId, string query);
        Task<dynamic> ExecuteStoredProcedureAsync(string collectionId, string storedProcName, params dynamic[] procedureParams);
        Task<Document> CreateItemAsync<T>(T item, string collectionId);
        Task<Document> UpdateItemAsync<T>(string id, T item, string collectionId);
    }
}