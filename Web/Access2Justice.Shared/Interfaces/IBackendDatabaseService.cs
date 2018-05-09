using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace Access2Justice.Shared.Interfaces
{
    public interface IBackendDatabaseService
    {
        Task<Document> CreateItemAsync<T>(T item);
        Task DeleteItemAsync(string id);
        Task<T> GetItemAsync<T>(string id);
        Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate);
        Task<Document> UpdateItemAsync<T>(string id, T item);


        Task<IEnumerable<TopicModel>> GetTopicsFromCollectionAsync();
        Task<TopicModel> GetTopicsFromCollectionAsync(string id);
    }
}