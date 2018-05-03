using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Access2Justice.CosmosDbService;
using Access2Justice.CosmosDbService.Models;
namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Topics")]
    public class TopicsController : Controller
    {
        Class1 cls = new Class1();
        public List<TopicModel> Get()
        {
            return cls.GetTopicsList();
        }
    }
}