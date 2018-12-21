using Access2Justice.Integration.Api.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models.Integration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Api.Controllers
{
    /// <summary>
    /// Service Provider Controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/service-providers")]
    public class ServiceProvidersController : Controller
    {
        private readonly IServiceProvidersBusinessLogic serviceProvidersBusinessLogic;
        private readonly IServiceProvidersOrchestrator serviceProvidersOrchestrator;

        /// <summary>
        /// Service Provider Constructor
        /// </summary>
        public ServiceProvidersController(IServiceProvidersBusinessLogic serviceProvidersBusinessLogic, IServiceProvidersOrchestrator serviceProvidersOrchestrator)
        {
            this.serviceProvidersBusinessLogic = serviceProvidersBusinessLogic;
            this.serviceProvidersOrchestrator = serviceProvidersOrchestrator;
        }

        /// <summary>
        /// Retrieves service provider by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-service-provider/{id}")]
        [ProducesResponseType(typeof(ServiceProvider), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetServiceProviderAsync(string id)
        {
            var serviceProvider = await serviceProvidersBusinessLogic.GetServiceProviderDocumentAsync(id).ConfigureAwait(false);
            return Ok(serviceProvider);
        }

        /// <summary>
        /// Upserts a service provider
        /// </summary>
        /// <param name="serviceProvider"></param>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpsertServiceProviders([FromBody]List<ServiceProvider> serviceProvider)
        {
            string topicName = "Family";
            var response = await serviceProvidersBusinessLogic.UpsertServiceProviderDocumentAsync(serviceProvider, topicName).ConfigureAwait(false);
            return Ok(response);
        }

        /// <summary>
        /// Process service provider data
        /// </summary>
        /// <param name="topicName"></param>
        /// <returns></returns>
        [HttpGet("load-partners-data/{topicName}")]
        public async Task<IActionResult> LoadPartnersData(string topicName)
        {
            var response = await serviceProvidersOrchestrator.LoadServiceProviders(topicName);
            return Ok(response);
        }
    }
}