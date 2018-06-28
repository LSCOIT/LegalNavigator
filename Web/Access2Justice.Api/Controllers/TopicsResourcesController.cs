using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    public class TopicsResourcesController : Controller
    {
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;
        private readonly ILuisBusinessLogic luisBusinessLogic;

        public TopicsResourcesController(ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic, ILuisBusinessLogic luisBusinessLogic)
        {
            this.topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
            this.luisBusinessLogic = luisBusinessLogic;
        }

        /// <summary>
        /// Get all topics in the collection
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/topics/gettopics")]
        public async Task<IActionResult> GetTopics()
        {
            var response = await topicsResourcesBusinessLogic.GetTopLevelTopicsAsync();
            return Ok(response);
        }

        
        /// <summary>
        /// Get subtopics by the topic Id
        /// </summary>
        /// <param name="parentTopicId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/topics/getsubtopics/{parentTopicId}")]
        public async Task<IActionResult> GetSubTopics(string parentTopicId)
        {

            var topics = await topicsResourcesBusinessLogic.GetSubTopicsAsync(parentTopicId);
            return Ok(topics);
        }


        /// <summary>
        /// Get the topic details by the document parent Id
        /// </summary>
        /// <param name="parentTopicId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/topics/getresourcedetails/{ParentTopicId}")]
        public async Task<IActionResult> GetResourceDetails(string parentTopicId)
        {
            var topics = await topicsResourcesBusinessLogic.GetResourceAsync(parentTopicId);
            return Ok(topics);
        }


        /// <summary>
        /// Get the document details by a document Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/topics/getdocument/{id}")]
        public async Task<IActionResult> GetDocumentDataAsync(string id)
        {

            var topics = await topicsResourcesBusinessLogic.GetDocumentAsync(id);
            return Ok(topics);
        }

        [HttpPost]
        [Route("api/resources")]
        public async Task<IActionResult> GetPagedDataAsync([FromBody]ResourceFilter resourceInput)
        {
            var response = await topicsResourcesBusinessLogic.GetPagedResourceAsync(resourceInput);
            
            return Content(response);            
        }

        
        /// <summary>
        /// Get the parent topics by a topic id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/topics/getbreadcrumbs/{id}")]
        public async Task<IActionResult> GetBreadcrumbAsync(string id)
        {
            var topics = await topicsResourcesBusinessLogic.GetBreadcrumbDataAsync(id);
            return Ok(topics);
        }
    }
}