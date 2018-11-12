using Access2Justice.Shared.Models;
using Access2Justice.Shared.Models.Integration;
using Microsoft.AspNetCore.Mvc;
using Access2Justice.Integration.Api;
using Access2Justice.Integration.Adapters;

namespace Access2Justice.Integration.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/ServiceProviders")]
    public class ServiceProvidersController : Controller
    {
        /// <summary>
        /// Retrieves service provider by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(ServiceProvider), 200)]
        [ProducesResponseType(404)]
        public IActionResult Get(int id)
        {
            var serviceProvider = new ServiceProvider();
            return Ok(serviceProvider);
        }

        /// <summary>
        /// Upserts a service provider
        /// </summary>
        /// <param name="serviceProvider"></param>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public void Post([FromBody]ServiceProvider serviceProvider)
        {
            return;
        }

        /// <summary>
        /// Retrieves service provider by Id
        /// </summary>
        /// <param name="organizationalUnit"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(ServiceProvider), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetServiceProviders(string organizationalUnit, Topic topic)
        {
            ServiceProviderAdaptee serviceProviderAdaptee = new ServiceProviderAdaptee();
            var serviceProvider = serviceProviderAdaptee.GetServiceProviders(organizationalUnit, topic);
            return Ok(serviceProvider);
        }
    }
}