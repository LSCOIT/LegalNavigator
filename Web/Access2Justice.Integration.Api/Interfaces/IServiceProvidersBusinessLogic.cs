using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Api.Interfaces
{
    public interface IServiceProvidersBusinessLogic
    {
        Task<dynamic> GetServiceProviderDocumentAsync(string id);
        Task<IEnumerable<object>> UpsertServiceProviderDocumentAsync(dynamic serviceProvider);
    }
}
