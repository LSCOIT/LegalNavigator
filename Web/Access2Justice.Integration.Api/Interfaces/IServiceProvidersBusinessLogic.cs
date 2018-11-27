using Access2Justice.Shared.Models.Integration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

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
        Task<IEnumerable<Document>> UpsertServiceProviderDocumentAsync(List<ServiceProvider> serviceProvider, string topicName);
    }
}