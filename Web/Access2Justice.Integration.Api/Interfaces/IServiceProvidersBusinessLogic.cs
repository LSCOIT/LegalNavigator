using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Api.Interfaces
{
    /// <summary>
    /// Service provider interface
    /// </summary>
    public interface IServiceProvidersBusinessLogic
    {
        /// <summary>
        /// returns service provider based on id
        /// </summary>
        Task<dynamic> GetServiceProviderDocumentAsync(string id);
        /// <summary>
        /// upserts service provider
        /// </summary>
        Task<IEnumerable<object>> UpsertServiceProviderDocumentAsync(dynamic serviceProviderJson, dynamic providerDetailJson, dynamic topicName);
    }
}
