using System;
using System.Globalization;
using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/websearch")]
    public class WebSearchController : Controller
    {
        private readonly IWebSearchBusinessLogic webSearchBusinessLogic;
        private IBingSettings bingSettings;

        public WebSearchController(IWebSearchBusinessLogic webSearchBusinessLogic, IBingSettings bingSettings)
        {
            this.webSearchBusinessLogic = webSearchBusinessLogic;
            this.bingSettings = bingSettings;
        }

        [HttpGet("{searchTerm}/{offset}")]
        public async Task<IActionResult> GetAsync(string searchTerm, Int16 offset)
        {
            var uri = string.Format(CultureInfo.InvariantCulture, bingSettings.BingSearchUrl.OriginalString, searchTerm, bingSettings.CustomConfigId, bingSettings.PageResultsCount, offset);
            var response = await webSearchBusinessLogic.SearchWebResourcesAsync(new Uri(uri));

            JObject webResources = new JObject
            {
                { "webResources" , JsonConvert.DeserializeObject(response) }
            };
            var resources = webResources.ToString();
            return Content(resources);
        }
    }
}