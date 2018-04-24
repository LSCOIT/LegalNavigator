using Access2Justice.Api.Models;
using Access2Justice.CosmosDb;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Route("api/[controller]")]
    public class CuratedExperienceController : Controller
    {
        //private readonly IConfigurationManager _configurationManager;
        //private readonly IDocumentClient _documentClient;
        private readonly IBackendDatabaseService<CuratedExperience> _backendDatabaseService;

        public CuratedExperienceController(IBackendDatabaseService<CuratedExperience> backendDatabaseService)
        {
            //_configurationManager = configurationManager;
            //_documentClient = documentClient;
            _backendDatabaseService = backendDatabaseService;
        }

        [HttpGet]
        public async Task<IEnumerable<CuratedExperience>> Get()
        {
            var items = await _backendDatabaseService.GetItemsAsync(x => x.id == "46350752-a33d-3454-2e8d-e1045d554d41");
            return items;
        }

    }
}
