using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Access2Justice.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get() => new string[] { "value1", "value2" };
    }
}