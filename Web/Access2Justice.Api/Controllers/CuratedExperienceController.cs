using Access2Justice.Api.Models.CuratedExperience;
using Access2Justice.Api.ViewModels;
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
        private readonly IBackendDatabaseService<CuratedExperience> _backendDatabaseService;

        public CuratedExperienceController(IBackendDatabaseService<CuratedExperience> backendDatabaseService)
        {
            _backendDatabaseService = backendDatabaseService;
        }

        [HttpGet]
        public async Task<CuratedExperienceChoiceSet> Get([FromQuery] string id)
        {
            var choiceSet = new CuratedExperienceChoiceSet();
            var choice = new List<Choice>();

            var children = new List<string>();
            children.Add("ffa0141d-ff2a-4f39-90eb-fbbb2f71e482");

            var curatedExperience = new CuratedExperience();
            curatedExperience.id = "8bafc1c9-c451-4f72-84f9-2383791fa713";
            curatedExperience.parentId = "self";
            curatedExperience.description = "are you a male or female?";
            curatedExperience.childern = children;

            await _backendDatabaseService.CreateItemAsync(curatedExperience);


            var retrivedCuratedExperience = await _backendDatabaseService.GetItemAsync("8bafc1c9-c451-4f72-84f9-2383791fa713");

            return choiceSet;
        }
    }
}