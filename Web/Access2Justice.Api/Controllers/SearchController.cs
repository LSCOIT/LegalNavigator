﻿using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/search")]
    public class SearchController : Controller
    {
        private readonly ILuisBusinessLogic luisBusinessLogic;

        public SearchController(ILuisBusinessLogic luisBusinessLogic)
        {
            this.luisBusinessLogic = luisBusinessLogic;
        }

        [HttpGet("{query}")]
        public async Task<IActionResult> GetAsync(string query)
        {
            LuisInput luisInput = new LuisInput { Sentence = query };
            var resources = await luisBusinessLogic.GetResourceBasedOnThresholdAsync(luisInput);
            return Content(resources);
        }
    }
}