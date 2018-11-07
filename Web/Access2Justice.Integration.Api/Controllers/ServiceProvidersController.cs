using Access2Justice.Shared.Models.Integration;
using Microsoft.AspNetCore.Mvc;

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
    }
}