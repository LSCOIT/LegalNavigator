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
using Access2Justice.CosmosDb.Interfaces;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
   
    public class TopicsController : Controller
    {
        private readonly IBackendDatabaseService _backendDataBaseService;
        private readonly ICosmosDbSettings _cosmosDbSettings;
        public TopicsController(IBackendDatabaseService backendDataBaseService, ICosmosDbSettings cosmosDbSettings)
        {
            _backendDataBaseService = backendDataBaseService;
            _cosmosDbSettings = cosmosDbSettings;
        }

        [HttpGet]
        [Route("api/topics/get")]
        public async Task<IActionResult> Get()
        {
            var topics = await _backendDataBaseService.GetItemsAsync<TopicModel>(a => a.Type == "topic" && a.ParentId == "", _cosmosDbSettings.TopicCollectionId);

            return Ok(topics);                     
        }
        [HttpGet]
        [Route("api/topics/getsubtopics/{id}")]

        public async Task<IActionResult> GetSubTopics(string id)
        {

            var topics = await _backendDataBaseService.GetItemsAsync<TopicModel>(a => a.Type == "topic" && a.ParentId == id, _cosmosDbSettings.TopicCollectionId);
            return Ok(topics);
        }
        [HttpGet]
        [Route("api/topics/getsubtopicdetails/{id}")]
        public async Task<IActionResult> GetSubTopicDetails(string id)
        {
            string[] spParams = { "id", id };
            var response = await _backendDataBaseService.ExecuteStoredProcedureAsyncWithParameters<dynamic>("GetResourceById", spParams);            
            return Ok(response);
        }
    }
}
