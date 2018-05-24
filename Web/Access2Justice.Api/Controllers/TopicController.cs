using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
   
    public class TopicController : Controller
    {
        IBackendDatabaseService backendDataBaseService;
        public TopicController(IBackendDatabaseService backendDataBaseService)
        {
            this.backendDataBaseService = backendDataBaseService;
        }

        [HttpGet]
        [Route("api/topic/get")]
        public async Task<IActionResult> Get()
        {
            // var topics = await backendDataBaseService.GetItemAsync<dynamic>("SELECT * FROM c where c.parentTopicID ="+"");
            //return Ok(topics);
            var response = await backendDataBaseService.ExecuteStoredProcedureAsyncWithoutParameters<dynamic>();
            return Ok(response);
        }
        [HttpGet]
        [Route("api/topic/getsubtopics/{id}")]

        public async Task<IActionResult> GetSubTopics(string id)
        {

            var topics = await backendDataBaseService.GetItemsAsync<TopicModel>(a => a.Type == "Sub-Topic" && a.ParentTopicID == id);
            return Ok(topics);
        }
        [HttpGet]
        [Route("api/topic/getsubtopicdetails/{id}")]
        public async Task<IActionResult> GetSubTopicDetails(string id)
        {
            string[] spParams = { "id", id };
            var response = await backendDataBaseService.ExecuteStoredProcedureAsyncWithParameters<dynamic>("GetResourceById", spParams);         
            return Ok(response);
        }
    }
}