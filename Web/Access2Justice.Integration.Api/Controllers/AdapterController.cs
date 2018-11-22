using Access2Justice.Integration.Api.Interfaces;
using Access2Justice.Integration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Adapter")]
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
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>        
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetServiceProviderAsync([FromBody]dynamic serviceProvider)
        {
            var serviceProvider1 = await serviceProviderAdapter.GetServiceProviders(serviceProvider);
            return Ok(serviceProvider1);
        }

    }
}