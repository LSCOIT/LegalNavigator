using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Location")]
    public class LocationController : Controller
    {
        private readonly ILocationBusinessLogic locationBusinessLogic;

        public LocationController(ILocationBusinessLogic locationBusinessLogic)
        {
            this.locationBusinessLogic = locationBusinessLogic;
        }

        [HttpGet("get-state-codes")]
        public async Task<IActionResult> GetStateCodes()
        {
            var stateCodes = await locationBusinessLogic.GetStateCodes();
            if (stateCodes == null) return NotFound();

            return Ok(stateCodes);
        }
    }
}