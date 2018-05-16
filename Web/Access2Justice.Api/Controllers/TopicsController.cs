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
using Access2Justice.Shared;
using Newtonsoft.Json;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
   
    public class TopicsController : Controller
    {
        IBackendDatabaseService backendDataBaseService; 
        public TopicsController(IBackendDatabaseService backendDataBaseService)
        {
            this.backendDataBaseService = backendDataBaseService;          
        }

        [HttpGet]
        [Route("api/topics/get")]
        public async Task<IActionResult> Get()
        {
            var topics = await backendDataBaseService.GetItemsAsync<TopicModel>(a => a.Type == "topic" && a.ParentId == "");

            return Ok(topics);                     
        }
        [HttpGet]
        [Route("api/topics/getsubtopics/{id}")]

        public async Task<IActionResult> GetSubTopics(string id)
        {

            var topics = await backendDataBaseService.GetItemsAsync<TopicModel>(a => a.Type == "topic" && a.ParentId == id);
            return Ok(topics);
        }
        [HttpGet]
        [Route("api/topics/getsubtopicdetails/{id}")]
        public async Task<IActionResult> GetSubTopicDetails(string id)
        {
            string[] spParams = { "id", id };
            var response = await backendDataBaseService.ExecuteStoredProcedureAsyncWithParameters<dynamic>("GetResourceById", spParams);
            //List<TopicModel> jsonResponse = JsonConvert.DeserializeObject<List<TopicModel>>(response);
            return Ok(response);
        }
    }
}
