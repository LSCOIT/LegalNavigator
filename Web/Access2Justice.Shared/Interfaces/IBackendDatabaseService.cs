using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
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
        Task<dynamic> QueryItemsAsync(string collectionId, string query);
        Task<Document> UpdateItemAsync<T>(string id, T item);        
        Task<T> ExecuteStoredProcedureAsyncWithParameters<T>(string storedProcName, params dynamic[] procedureParams);        
        Task<dynamic> QueryItemsPaginationAsync(string collectionId, string query, FeedOptions feedOptions);
        Task<dynamic> GetFirstPageResourceAsync(string query);
        Task<dynamic> GetNextPageResourcesAsync(string query, string continuationToken);
    }
}