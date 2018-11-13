using Access2Justice.Shared.Models.Integration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Access2Justice.Integration.Api.Interfaces;
using System;

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
        /// <summary>
        /// Constructor call
        /// </summary>
        public ServiceProvidersController(IServiceProvidersBusinessLogic serviceProvidersBusinessLogic)
        {
            this.serviceProvidersBusinessLogic = serviceProvidersBusinessLogic;
        }

        /// <summary>
        /// Retrieves service provider by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
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
        [HttpPost("upsert")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpsertServiceProviders([FromBody]dynamic serviceProvider)
        {
            var response = await serviceProvidersBusinessLogic.UpsertServiceProviderDocumentAsync(serviceProvider).ConfigureAwait(false);
            return Ok(response);
        }
    }
}