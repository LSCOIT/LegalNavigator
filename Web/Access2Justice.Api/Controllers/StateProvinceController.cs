using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    [Produces("application/json")]
    [Route("api/StateProvince")]
    public class StateProvinceController : Controller
    {
        private readonly IStateProvinceBusinessLogic locationBusinessLogic;

        public StateProvinceController(IStateProvinceBusinessLogic locationBusinessLogic)
        {
            this.locationBusinessLogic = locationBusinessLogic;
        }

        [HttpGet("state-codes")]
        public async Task<IActionResult> GetStateCodes()
        {
            var stateCodes = await locationBusinessLogic.GetStateCodes();
            
            return Ok(stateCodes);
        }

        [HttpGet("state-code")]
        public async Task<IActionResult> GetStateCodeForState(string stateName)
        {
            var stateCodes = await locationBusinessLogic.GetStateCodeForState(stateName);
            
            return Ok(stateCodes);
        }

        [HttpGet("state-name")]
        public async Task<IActionResult> GetStateNameForStateCode(string stateCode)
        {
            var stateName = await locationBusinessLogic.GetStateNameForStateCode(stateCode);

            return Ok(stateName);
        }
    }
}