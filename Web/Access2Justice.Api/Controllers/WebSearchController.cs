﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pomelo.AntiXSS;

namespace Access2Justice.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    [Produces("application/json")]
    [Route("api/web-search")]
    public class WebSearchController : Controller
    {
        private readonly IWebSearchBusinessLogic webSearchBusinessLogic;
        private IBingSettings bingSettings;

        public WebSearchController(IWebSearchBusinessLogic webSearchBusinessLogic, IBingSettings bingSettings)
        {
            this.webSearchBusinessLogic = webSearchBusinessLogic;
            this.bingSettings = bingSettings;
        }

        /// <summary>
        /// Get web resource based on given search term
        /// </summary>
        /// <remarks>
        /// Helps to get web resource based on given search term
        /// </remarks>
        /// <param name="searchTerm"></param>
        /// <param name="offset"></param>
        /// <response code="200">Get web resource based on given search term</response>
        /// <response code="500">Failure</response>
        [HttpGet("{searchTerm}/{offset}")]
        public async Task<IActionResult> GetAsync(string searchTerm, Int16 offset)
        {
            searchTerm = Instance.Sanitize(searchTerm);
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