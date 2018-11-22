using Access2Justice.Integration.Api.Interfaces;
using Access2Justice.Integration.Interfaces;
using Access2Justice.Shared.Models.Integration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Api.Controllers
{
    /// <summary>
    /// Adapter Controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/adapter")]
    public class AdapterController : Controller
    {        
        private readonly IServiceProviderAdapter serviceProviderAdapter;

        /// <summary>
        /// Service Provider Constructor
        /// </summary>
        public AdapterController(IServiceProviderAdapter serviceProviderAdapter)
        {
            this.serviceProviderAdapter = serviceProviderAdapter;
        }

        /// <summary>
        /// Fetches service provider details from RTM and converts to Service Provider object.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(List<ServiceProvider>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetServiceProviderAsync([FromBody]dynamic serviceProvider)
        {
            var response = await serviceProviderAdapter.GetServiceProviders(serviceProvider);
            return Ok(response);
        }

    }
}