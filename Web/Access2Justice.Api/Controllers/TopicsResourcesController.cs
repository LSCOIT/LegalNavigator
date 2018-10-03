using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    public class TopicsResourcesController : Controller
    {
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;
        private readonly ILuisBusinessLogic luisBusinessLogic;
        private readonly IUserRoleBusinessLogic userRoleBusinessLogic;

        public TopicsResourcesController(ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic, ILuisBusinessLogic luisBusinessLogic,
            IUserRoleBusinessLogic userRoleBusinessLogic)
        {
            this.topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
            this.luisBusinessLogic = luisBusinessLogic;
            this.userRoleBusinessLogic = userRoleBusinessLogic;
        }

        /// <summary>
        /// Get all topics in the collection
        /// </summary>
        /// <returns></returns>
        [Route("api/topics/gettopics")]
        [HttpPost]
        public async Task<IActionResult> GetTopics([FromBody]Location location)
        {
            var response = await topicsResourcesBusinessLogic.GetTopLevelTopicsAsync(location);
            return Ok(response);
        }

        /// <summary>
        /// Get subtopics by the topic Id
        /// </summary>
        /// <param name="parentTopicId"></param>
        /// <returns></returns> 
        [Route("api/topics/getsubtopics")]
        [HttpPost]
        public async Task<IActionResult> GetSubTopics([FromBody]TopicInput topicInput)
        {
            var topics = await topicsResourcesBusinessLogic.GetSubTopicsAsync(topicInput);
            return Ok(topics);
        }

        /// <summary>
        /// Get resource by resource id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/topics/getresource")]
        public async Task<IActionResult> GetResource([FromBody]TopicInput topicInput)
        {
            var resource = await topicsResourcesBusinessLogic.GetResourceByIdAsync(topicInput);
            return Ok(resource);
        }

        /// <summary>
        /// Get the topic details by the document parent Id
        /// </summary>
        /// <param name="parentTopicId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/topics/getresourcedetails")]
        public async Task<IActionResult> GetResourceDetails([FromBody]TopicInput topicInput)
        {
            var topics = await topicsResourcesBusinessLogic.GetResourceAsync(topicInput);
            return Ok(topics);
        }

        /// <summary>
        /// Get the document details by a document Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/topics/getdocument")]
        public async Task<IActionResult> GetDocumentDataAsync([FromBody]TopicInput topicInput)
        {
            var topics = await topicsResourcesBusinessLogic.GetDocumentAsync(topicInput);
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

        /// <summary>
        /// get topic details based on topic name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/topics/gettopicdetails/{name}")]
        public async Task<IActionResult> GetTopicDetails(string name)
        {
            var topics = await topicsResourcesBusinessLogic.GetTopicDetailsAsync(name);
            return Ok(topics);
        }

        /// <summary>
        /// get resource details based on name and resource type
        /// </summary>
        /// <param name="name", "resourceType"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/topics/getresourcedetails/{name}/{type}")]
        public async Task<IActionResult> GetResourceDetails(string name, string type)
        {
            var resources = await topicsResourcesBusinessLogic.GetResourceDetailAsync(name, type);
            return Ok(resources);
        }

        /// <summary>
        /// Get the organizations by the location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/topics/getorganizationdetails")]
        public async Task<IActionResult> GetOrganizationsWhenParamsValuePassed([FromBody]Location location)
        {
            var organizations = await topicsResourcesBusinessLogic.GetOrganizationsAsync(location);
            return Ok(organizations);
        }

        /// <summary>
        /// get topic schema
        /// </summary>
        [HttpGet]
        [Route("api/topics/getschematopic")]
        public Topic GetSchemaTopic()
        {
            return new Topic();
        }

        /// <summary>
        /// get action plan schema
        /// </summary>
        [HttpGet]
        [Route("api/topics/getschemaactionplan")]
        public ActionPlan GetSchemaActionPlan()
        {
            return new ActionPlan();
        }

        /// <summary>
        /// get article schema
        /// </summary>
        [HttpGet]
        [Route("api/topics/getschemaarticle")]
        public Article GetSchemaArticle()
        {
            return new Article();
        }

        /// <summary>
        /// get video schema
        /// </summary>
        [HttpGet]
        [Route("api/topics/getschemavideo")]
        public Video GetSchemaVideo()
        {
            return new Video();
        }

        /// <summary>
        /// get organizations schema
        /// </summary>
        [HttpGet]
        [Route("api/topics/getschemaorganization")]
        public Organization GetSchemaOrganization()
        {
            return new Organization();
        }

        /// <summary>
        /// get form schema
        /// </summary>
        [HttpGet]
        [Route("api/topics/getschemaform")]
        public Form GetSchemaForm()
        {
            return new Form();
        }

        /// <summary>
        /// get essential reading schema
        /// </summary>
        [HttpGet]
        [Route("api/topics/getschemaessentialreading")]
        public EssentialReading GetSchemaEssentialReading()
        {
            return new EssentialReading();
        }

        /// <summary>
        /// Create Resource Documents using upload
        /// </summary>
        [Permission(PermissionName.createresourcesupload)]
        [HttpPost]
        [Route("api/topics/createresources/upload")]
        public async Task<IActionResult> CreateResources(IFormFile uploadedFile)
        {
            var path = uploadedFile.FileName;
            var resources = await topicsResourcesBusinessLogic.UpsertResourcesUploadAsync(path);
            return Ok(resources);
        }

        /// <summary>
        /// Create Resource Document
        /// </summary>
        [Permission(PermissionName.upsertresourcedocument)]
        [HttpPost]
        [Route("api/upsertresourcedocument")]
        public async Task<IActionResult> UpserResourceDocument([FromBody]dynamic resource)
        {
            var resources = await topicsResourcesBusinessLogic.UpsertResourceDocumentAsync(resource);
            return Ok(resources);
        }

        /// <summary>
        /// Create Topic Documents using upload 
        /// </summary>
        [Permission(PermissionName.createtopicsupload)]
        [HttpPost]
        [Route("api/topics/createtopics/upload")]
        public async Task<IActionResult> CreateTopics(IFormFile uploadedFile)
        {
            var path = uploadedFile.FileName;
            var topics = await topicsResourcesBusinessLogic.UpsertTopicsUploadAsync(path);
            return Ok(topics);
        }

        /// <summary>
        /// Create Topic Document
        /// </summary>
        [Permission(PermissionName.upserttopicdocument)]
        [HttpPost]
        [Route("api/upserttopicdocument")]
        public async Task<IActionResult> UpsertTopicDocument([FromBody]dynamic topic)
        {
            if (HttpContext.User.Claims.FirstOrDefault() != null)
            {
                string oId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                if (await userRoleBusinessLogic.GetOrganizationalUnit(oId, topic.OrganizationalUnit))
                {
                    var topics = await topicsResourcesBusinessLogic.UpsertTopicDocumentAsync(topic);
                    return Ok(topics);
                }
            }
            return StatusCode(403);
        }

        /// <summary>
		/// Get the topic details by the document parent Id
		/// </summary>
		/// <param name="parentTopicId"></param>
		/// <returns></returns>
		[HttpPut]
        [Route("api/personalizedresources")]
        public async Task<IActionResult> GetPersonalizedDataAsync([FromBody]ResourceFilter resourceInput)
        {
            var response = await topicsResourcesBusinessLogic.GetPersonalizedResourcesAsync(resourceInput);
            return Content(response);
        }

        /// Get all topics
        /// </summary>
        /// <param name=""></param>
        /// <returns>all topics from cosmos db</returns>
        [Route("api/topics/getalltopics")]
        [HttpGet]
        public async Task<IActionResult> GetAllTopics()
        {
            var response = await topicsResourcesBusinessLogic.GetAllTopics();
            return Ok(response);
        }
    }
}