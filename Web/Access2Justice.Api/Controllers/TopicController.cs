using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Topic")]
    public class TopicController : Controller
    {
        public IActionResult Get()
        {
            return null;
        }
    }
}