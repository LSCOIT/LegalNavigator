using Microsoft.AspNetCore.Mvc;
using System;

namespace Access2Justice.Web.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        [HttpGet("[action]")]
        public string WeatherForecasts()
        {
            return "Hello World!";
        }
    }
}
