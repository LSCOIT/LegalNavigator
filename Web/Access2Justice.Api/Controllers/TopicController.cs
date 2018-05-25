using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    public class TopicController : Controller
    {
        private readonly ITopicBusinessLogic _topicBusinessLogic;
        public TopicController(ITopicBusinessLogic topicBusinessLogic)
        {
            _topicBusinessLogic = topicBusinessLogic;
        }

        #region  GetTopics
        [HttpGet]
        [Route("api/topics/get")]
        public async Task<IActionResult> Get()
        {
            var response = await _topicBusinessLogic.GetTopicsAsync<dynamic>();
            return Ok(response);
        }
        #endregion

        #region GetSubTopics
        [HttpGet]
        [Route("api/topics/getsubtopics/{id}")]
        public async Task<IActionResult> GetSubTopics(string id)
        {

            var topics = await _topicBusinessLogic.GetSubTopicsAsync(id);
            return Ok(topics);
        }
        #endregion

        #region GetSubTopicDetails
        [HttpGet]
        [Route("api/topics/getsubtopicdetails/{id}")]
        public async Task<IActionResult> GetSubTopicDetails(string id)
        {
            var topics = await _topicBusinessLogic.GetSubTopicDetailAsync(id);
            return Ok(topics);
        }
        #endregion

        
    }
}