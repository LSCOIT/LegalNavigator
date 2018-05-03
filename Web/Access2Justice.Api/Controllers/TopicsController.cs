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
   
    public class TopicsController : Controller
    {
        Class1 cls = new Class1();
        [Route("api/Topics/Get")]
        [HttpGet]
        public List<TopicModel> Get()
        {
            return cls.GetTopicsList();
        }

        [Route("api/Topics/GetContent")]
        [HttpGet]
        public SubjectModel GetContent(string name)
        {
            return cls.GetContentsList(name);
        }
        //public Overview GetContent(int id)
        //{
        //    return bal.GetContentsList(id);
        //}

    }
}