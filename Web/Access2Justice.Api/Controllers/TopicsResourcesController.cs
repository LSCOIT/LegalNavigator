using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
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
        #region get all resources when resource type is action plan for a parentTopicId
        [HttpGet]
        [Route("api/topics/getactionplanresourcedetails/{parentTopicId}")]
        public async Task<IActionResult> GetActionPlanResourceDetails(string parentTopicId)
        {
            var filterValue = "Action Plans";
            var resources = await topicsResourcesBusinessLogic.GetResourceActionPlanAsync(parentTopicId, filterValue);
            return Ok(resources);
        }
        #endregion

    }
}