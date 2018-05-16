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
        Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate);
        Task<Document> UpdateItemAsync<T>(string id, T item);

        /// <summary>
        /// Asynchronous method to get documents by the given property name
        /// </summary>
        /// <typeparam name="T">Document details</typeparam>
        /// <param name="storedProcName">Procedure name</param>
        /// <param name="procedureParams">params for procedure</param>
        /// <returns>Returns documents for the given property name</returns>
        Task<T> ExecuteStoredProcedureAsyncWithParameters<T>(string storedProcName, params dynamic[] procedureParams);

    }
}