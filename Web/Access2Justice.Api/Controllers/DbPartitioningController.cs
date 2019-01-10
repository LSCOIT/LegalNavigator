using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;
using Pomelo.AntiXSS;
using System.Diagnostics.CodeAnalysis;
using Access2Justice.Shared;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/db-partitioning")]
    public class DbPartitioningController : Controller
    {
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings cosmosDbSettings;

        public DbPartitioningController(ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic, IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings)
        {
            this.topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
            this.dynamicQueries = dynamicQueries;
            this.cosmosDbSettings = cosmosDbSettings;
        }

        [Route("demo")]
        [HttpPost]
        public async Task<IActionResult> GetTopics([FromBody]Location location)
        {
            // var response = await dynamicQueries.FindItemsAllAsync(cosmosDbSettings.TopicsCollectionId);

            // var response = await dynamicQueries.FindItemsWhereWithLocationAsync(cosmosDbSettings.TopicsCollectionId, "name", "Divorce", location);

            // var response = await dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.RolesCollectionId, "organizationalUnit", "Alaska");

            var response = await dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.RolesCollectionId, "type", "Role");

            if (response == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(response);
        }
    }
}