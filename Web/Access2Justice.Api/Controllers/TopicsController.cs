using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Access2Justice.CosmosDb;
using Access2Justice.Shared.Interfaces;
using Microsoft.Azure.Documents;
using System.Linq.Expressions;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
   
    public class TopicsController : Controller
    {
        IBackendDatabaseService csdbsrvs;
     
        public TopicsController(IBackendDatabaseService bds)
        {
            csdbsrvs = bds;          
        }       
        [HttpGet]
        [Route("api/Topics/Get")]
        public IActionResult Get()
        {
            var topics = csdbsrvs.GetTopicsFromCollectionAsync();
            return Ok(topics);
                      
        }
        [HttpGet]
        [Route("api/Topics/GetSubTopics/{id}")]
        public IActionResult Get(string id)
        {
            var topics = csdbsrvs.GetTopicsFromCollectionAsync(id).Result;
            return Ok(topics);
        }
    }
}
