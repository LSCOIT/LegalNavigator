﻿using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    public class TopicsResourcesController : Controller
    {
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;
        public TopicsResourcesController(ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic)
        {
            this.topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
        }

        #region  get all topics when parentTopicId is empty
        [HttpGet]
        [Route("api/topics/gettopics")]
        public async Task<IActionResult> GetTopics()
        {
            var response = await topicsResourcesBusinessLogic.GetTopLevelTopicsAsync();
            return Ok(response);
        }
        #endregion

        #region get all topics when parentTopicId is guid value
        [HttpGet] 
        [Route("api/topics/getsubtopics/{parentTopicId}")]
        public async Task<IActionResult> GetSubTopics(string parentTopicId)
        {
            var topics = await topicsResourcesBusinessLogic.GetSubTopicsAsync(parentTopicId);
            return Ok(topics);
        }
        #endregion

        #region get all resources when parentTopicId is mapped to topicTags
        [HttpGet]
        [Route("api/topics/getresourcedetails/{parentTopicId}")]
        public async Task<IActionResult> GetResourceDetails(string parentTopicId)  
        {
            var topics = await topicsResourcesBusinessLogic.GetResourceAsync(parentTopicId);
            return Ok(topics);
        }
        #endregion

        #region get Spectific document data 
        [HttpGet]
        [Route("api/topics/getdocument/{id}")]
        public async Task<IActionResult> GetDocumentDataWithGuid(string id)  
        {

            var topics = await topicsResourcesBusinessLogic.GetDocumentAsync(id);
            return Ok(topics);
        }
        #endregion

        #region get Orgationzation details when location value passed
        [HttpGet]
        [Route("api/topics/getorganizationdetails/{location}")]
        public async Task<IActionResult> GetOrganizationsWhenParamsValuePassed(Location location)
        {
            var organizations = await topicsResourcesBusinessLogic.GetOrganizationsAsync(location); 
            return Ok(organizations);
        }
        #endregion

    }
}