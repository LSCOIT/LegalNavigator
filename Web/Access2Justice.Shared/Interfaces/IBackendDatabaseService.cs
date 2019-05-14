using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public class IncomingSharedResourceRetrieveParam
    {
        /// <summary>
        /// Resource to find
        /// </summary>
        public IEnumerable<Guid> ResourceIds { get; set; }

        /// <summary>
        /// Collection id where to search. If empty - search in all incoming collections
        /// </summary>
        public Guid IncomingShareId { get; set; }

        /// <summary>
        /// Search resources only from specific share. If empty - ignored
        /// </summary>
        public Guid SharedFromResourcesId { get; set; }
    }

    public interface IBackendDatabaseService
    {
        Task<Document> CreateItemAsync<T>(T item, string collectionId);
        Task DeleteItemAsync(string id, string collectionId);
        Task<T> GetItemAsync<T>(string id, string collectionId);
        Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate,string collectionId);
        Task<dynamic> QueryItemsAsync(string collectionId, string query, Dictionary<string, object> sqlParams = null);
        Task<ICollection<T>> QueryItemsAsync<T>(string collectionId, params Expression<Func<T, bool>>[] whereCondition);
        Task<dynamic> ExecuteStoredProcedureAsync(string collectionId, string storedProcName, string partitionKey, params dynamic[] procedureParams);
        Task<Document> UpdateItemAsync<T>(string id, T item, string collectionId);
        Task<dynamic> QueryItemsPaginationAsync(string collectionId, string query, FeedOptions feedOptions);
        Task<dynamic> GetFirstPageResourceAsync(string query,bool isInitialPage);
        Task<dynamic> GetNextPageResourcesAsync(string query, string continuationToken);
        Task<dynamic> QueryPagedResourcesAsync(string query, string continuationToken);
        Task<dynamic> QueryResourcesCountAsync(string query);
        /// <summary>
        /// Retrieve collection of incoming resources containing specific resource
        /// </summary>
        Task<ICollection<UserIncomingResources>> FindIncomingSharedResource(IncomingSharedResourceRetrieveParam incomingSharedResourceRetrieveParam);
    }
}