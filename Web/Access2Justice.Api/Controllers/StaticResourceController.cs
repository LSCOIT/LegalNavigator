using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    public class StaticResourceController : Controller
    {
        private readonly IStaticResourceBusinessLogic StaticResourceBusinessLogic;

        public StaticResourceController(IStaticResourceBusinessLogic StaticResourceBusinessLogic)
        {
            this.StaticResourceBusinessLogic = StaticResourceBusinessLogic;
        }

        /// <summary>
        /// Get StaticResource by page name
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/staticresource/getstaticresource/{name}")]
        public async Task<IActionResult> GetStaticResourceDataAsync(string name)
        {
            var users = await StaticResourceBusinessLogic.GetPageStaticResourceDataAsync(name);
            return Ok(users);
        }
    }   
}