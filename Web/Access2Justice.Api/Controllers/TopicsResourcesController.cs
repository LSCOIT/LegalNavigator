using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        /// <remarks>
        /// Helps to get all topic details by given location
        /// </remarks>
        /// <param name="location"></param>
        /// <response code="200">Get all topics for given location</response>
        /// <response code="500">Failure</response>

        
        [Route("api/topics/get-topics")]
        [HttpPost]
        public async Task<IActionResult> GetTopics([FromBody]Location location)
        {
            var response = await topicsResourcesBusinessLogic.GetTopLevelTopicsAsync(location);
            return Ok(response);
        }

        /// <summary>
        /// Get subtopics by the topic Id
        /// </summary>
        /// <remarks>
        /// Helps to get all sub topic details by given topic
        /// </remarks>
        /// <param name="topicInput"></param>
        /// <response code="200">Get all sub topics for given topic</response>
        /// <response code="500">Failure</response>
        [Route("api/topics/get-subtopics")]
        [HttpPost]
        public async Task<IActionResult> GetSubTopics([FromBody]TopicInput topicInput)
        {
            var topics = await topicsResourcesBusinessLogic.GetSubTopicsAsync(topicInput);
            return Ok(topics);
        }

        /// <summary>
        /// Get resource by resource id
        /// </summary>
        /// <remarks>
        /// Helps to get all resources by given resource id
        /// </remarks>
        /// <param name="topicInput"></param>
        /// <response code="200">Get all resources for given resource id</response>
        /// <response code="500">Failure</response>
        [HttpPost]
        [Route("api/topics/get-resource")]
        public async Task<IActionResult> GetResource([FromBody]TopicInput topicInput)
        {
            var resource = await topicsResourcesBusinessLogic.GetResourceByIdAsync(topicInput);
            return Ok(resource);
        }

        /// <summary>
        /// Get the topic details by the document parent Id
        /// </summary>
        /// <remarks>
        /// Helps to get all resource details by given topic
        /// </remarks>
        /// <param name="topicInput"></param>
        /// <response code="200">Get all resource details for given topic</response>
        /// <response code="500">Failure</response>
        [HttpPost]
        [Route("api/topics/get-resource-details")]
        public async Task<IActionResult> GetResourceDetails([FromBody]TopicInput topicInput)
        {
            var topics = await topicsResourcesBusinessLogic.GetResourceAsync(topicInput);
            return Ok(topics);
        }

        /// <summary>
        /// Get the document details by a document Id
        /// </summary>
        /// <remarks>
        /// Helps to get all document details by given id
        /// </remarks>
        /// <param name="topicInput"></param>
        /// <response code="200">Get all document for given topic</response>
        /// <response code="500">Failure</response>
        [HttpPost]
        [Route("api/topics/get-document")]
        public async Task<IActionResult> GetDocumentDataAsync([FromBody]TopicInput topicInput)
        {
            var topics = await topicsResourcesBusinessLogic.GetDocumentAsync(topicInput);
            return Ok(topics);
        }

        /// <summary>
        /// Get paged resource data
        /// </summary>
        /// <remarks>
        /// Helps to get all paged resource data
        /// </remarks>
        /// <param name="resourceInput"></param>
        /// <response code="200">Get all paged resource data</response>
        /// <response code="500">Failure</response>
        [HttpPost]
        [Route("api/resources")]
        public async Task<IActionResult> GetPagedDataAsync([FromBody]ResourceFilter resourceInput)
        {
            var response = await topicsResourcesBusinessLogic.GetPagedResourceAsync(resourceInput);
            return Content(response);
        }

        /// <summary>
        /// Get parent topics by a topic id for breadcrumb
        /// </summary>
        /// <remarks>
        /// Helps to get parent topics by a topic id for breadcrumb
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="200">Get parent topics by a topic id for breadcrumb</response>
        /// <response code="500">Failure</response>
        [HttpGet]
        [Route("api/topics/get-breadcrumbs/{id}")]
        public async Task<IActionResult> GetBreadcrumbAsync(string id)
        {
            var topics = await topicsResourcesBusinessLogic.GetBreadcrumbDataAsync(id);
            return Ok(topics);
        }

        /// <summary>
        /// get topic details based on topic name
        /// </summary>
        ///  <remarks>
        /// Helps to get topic details based on topic name
        /// </remarks>
        /// <param name="name"></param>
        /// <response code="200">Get topic details based on topic name</response>
        /// <response code="500">Failure</response>
        [HttpGet]
        [Route("api/topics/get-topic-details/{name}")]
        public async Task<IActionResult> GetTopicDetails(string name)
        {
            var topics = await topicsResourcesBusinessLogic.GetTopicDetailsAsync(name);
            return Ok(topics);
        }

        /// <summary>
        /// get resource details based on name and resource type
        /// </summary>
        ///  <remarks>
        /// Helps to get resource details based on name and resource type
        /// </remarks>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <response code="200">Get resource details based on name and resource type</response>
        /// <response code="500">Failure</response>
        [HttpGet]
        [Route("api/topics/get-resource-details/{name}/{type}")]
        public async Task<IActionResult> GetResourceDetails(string name, string type)
        {
            var resources = await topicsResourcesBusinessLogic.GetResourceDetailAsync(name, type);
            return Ok(resources);
        }

        /// <summary>
        /// Get the organizations by the location
        /// </summary>
        ///  <remarks>
        /// Helps to get the organizations by the location
        /// </remarks>
        /// <param name="location"></param>
        /// <response code="200">Get the organizations by the location</response>
        /// <response code="500">Failure</response>

        [HttpPost]
        [Route("api/topics/get-organization-details")]
        public async Task<IActionResult> GetOrganizationsWhenParamsValuePassed([FromBody]Location location)
        {
            var organizations = await topicsResourcesBusinessLogic.GetOrganizationsAsync(location);
            return Ok(organizations);
        }

        /// <summary>
        /// Get topic schema
        /// </summary>
        ///  <remarks>
        /// Helps to get topic schema
        /// </remarks>
        /// <response code="200">Get the topic schema</response>
        /// <response code="500">Failure</response>
        [HttpGet]
        [Route("api/topics/get-schema-topic")]
        public  Topic GetSchemaTopic()
        {
            return new Topic();
        }

        /// <summary>
        /// get action plan schema
        /// </summary>
        ///  <remarks>
        /// Helps to get action plan schema
        /// </remarks>
        /// <response code="200">Get the action plan schema</response>
        /// <response code="500">Failure</response>
        [HttpGet]
        [Route("api/topics/get-schema-action-plan")]
        public ActionPlan GetSchemaActionPlan()
        {
            return new ActionPlan();
        }

        /// <summary>
        /// get article schema
        /// </summary>
        ///  <remarks>
        /// Helps to get article schema
        /// </remarks>
        /// <response code="200">Get the article schema</response>
        /// <response code="500">Failure</response>
        [HttpGet]
        [Route("api/topics/get-schema-article")]
        public Article GetSchemaArticle()
        {
            return new Article();
        }

        /// <summary>
        /// get video schema
        /// </summary>
        ///  <remarks>
        /// Helps to get video schema
        /// </remarks>
        /// <response code="200">Get the video schema</response>
        /// <response code="500">Failure</response>
        [HttpGet]
        [Route("api/topics/get-schema-video")]
        public Video GetSchemaVideo()
        {
            return new Video();
        }

        /// <summary>
        /// get organizations schema
        /// </summary>
        ///  <remarks>
        /// Helps to get organizations schema
        /// </remarks>
        /// <response code="200">Get the organizations schema</response>
        /// <response code="500">Failure</response>
        [HttpGet]
        [Route("api/topics/get-schema-organization")]
        public Organization GetSchemaOrganization()
        {
            return new Organization();
        }

        /// <summary>
        /// get form schema
        /// </summary>
        ///  <remarks>
        /// Helps to get form schema
        /// </remarks>
        /// <response code="200">Get the form schema</response>
        /// <response code="500">Failure</response>
        [HttpGet]
        [Route("api/topics/get-schema-form")]
        public Form GetSchemaForm()
        {
            return new Form();
        }

        /// <summary>
        /// get essential reading schema
        /// </summary>
        /// <remarks>
        /// Helps to get essential reading content.
        /// </remarks>
        /// <response code="200">Returns essential reading content</response>
        /// <response code="500">Failure</response>
        [HttpGet]
        [Route("api/topics/get-schema-essential-reading")]
        public EssentialReading GetSchemaEssentialReading()
        {
            return new EssentialReading();
        }

        /// <summary>
        /// Create resource documents using upload
        /// </summary>
        /// <remarks>
        /// Helps to get the resource by the uploaded file
        /// </remarks>
        /// <param name="uploadedFile"></param>
        /// <response code="200">Returns created resources </response>
        /// <response code="500">Failure</response>
        [Permission(PermissionName.createresourcesupload)]
        [HttpPost]
        [Route("api/topics/create-resources/upload")]
        public async Task<IActionResult> CreateResources(IFormFile uploadedFile)
        {
            var path = uploadedFile.FileName;
            var resources = await topicsResourcesBusinessLogic.UpsertResourcesUploadAsync(path);
            return Ok(resources);
        }

        /// <summary>
        /// Create Resources Documents - can upsert single or multiple resources
        /// </summary>
        /// <remarks>
        /// Helps to get the resouce created by given input.
        /// </remarks>
        /// <param name="resource"></param>
        /// <response code="200">Returns created resources </response>
        /// <response code="500">Failure</response>
        //[Permission(PermissionName.upsertresourcedocument)]
        [HttpPost]
        [Route("api/upsert-resource-document")]
        public async Task<IActionResult> UpsertResourceDocument([FromBody]dynamic resource)
        {
            var resources = await topicsResourcesBusinessLogic.UpsertResourceDocumentAsync(resource);
            return Ok(resources);
        }

        /// <summary>
        /// Create Topic Documents using upload 
        /// </summary>
        /// <remarks>
        /// Helps to get the topic details by the uploaded file
        /// </remarks>
        /// <param name="uploadedFile"></param>
        /// <response code="200">Returns created topics </response>
        /// <response code="500">Failure</response>
        [Permission(PermissionName.createtopicsupload)]
        [HttpPost]
        [Route("api/topics/create-topics/upload")]
        public async Task<IActionResult> CreateTopics(IFormFile uploadedFile)
        {
            var path = uploadedFile.FileName;
            var topics = await topicsResourcesBusinessLogic.UpsertTopicsUploadAsync(path);
            return Ok(topics);
        }

        /// <summary>
        /// Create topic document
        /// </summary>
        /// /// <remarks>
        /// Helps to create topic document given a topic name.
        /// </remarks>
        /// <param name="topic"></param>
        /// <response code="200">Returns created topic document</response>
        /// <response code="500">Failure</response>
        //[Permission(PermissionName.upserttopicdocument)]
        [HttpPost]
        [Route("api/upsert-topic-document")]
        public async Task<IActionResult> UpsertTopicDocument([FromBody]dynamic topic)
        {
            var topics = await topicsResourcesBusinessLogic.UpsertTopicDocumentAsync(topic);
            return Ok(topics);
        }

        /// <summary>
        /// Create Single Topic Document
        /// </summary>
        [Permission(PermissionName.upserttopic)]
        [HttpPost]
        [Route("api/upsert-topic")]
        public async Task<IActionResult> UpsertSingleTopicDocument([FromBody]dynamic topic)
        {
            List<dynamic> topicsList = new List<dynamic>();
            topicsList.Add(topic);
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(topic.organizationalUnit))
            {
                var topics = await topicsResourcesBusinessLogic.UpsertTopicDocumentAsync(topicsList);
                return Ok(topics);
            }
            return StatusCode(403);
        }

        /// <summary>
        /// Create Single Resource Document
        /// </summary>
        [Permission(PermissionName.upsertresource)]
        [HttpPost]
        [Route("api/upsert-resource")]
        public async Task<IActionResult> UpserSingleResourceDocument([FromBody]dynamic resource)
        {
            List<dynamic> resourcesList = new List<dynamic>();
            resourcesList.Add(resource);
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(resource.organizationalUnit))
            {
                var resources = await topicsResourcesBusinessLogic.UpsertResourceDocumentAsync(resourcesList);
                return Ok(resources);
            }
            return StatusCode(403);
        }
        ///<summary>
        /// Get the topic details by the document parent Id
        /// </summary>
        /// <remarks>
        /// Helps to get the topic details by the document parent Id
        /// </remarks>
        /// <param name="resourceInput"></param>
        /// <response code="200">Get personalized data for given input</response>
        /// <response code="500">Failure</response>
        [HttpPut]
        [Route("api/personalized-resources")]
        public async Task<IActionResult> GetPersonalizedDataAsync([FromBody]ResourceFilter resourceInput)
        {
            var response = await topicsResourcesBusinessLogic.GetPersonalizedResourcesAsync(resourceInput);
            return Content(response);
        }

        ///<summary>
        /// Get all topics
        /// </summary>    
        /// <remarks>Helps to get all topics from Database. </remarks>
        /// <returns>all topics from cosmos db</returns>
        /// <response code="200">Returns all topics from DB</response>
        /// <response code="500">Failure</response>
        [Route("api/topics/get-all-topics")]
        [HttpGet]
        public async Task<IActionResult> GetAllTopics()
        {
            var response = await topicsResourcesBusinessLogic.GetAllTopics();
            return Ok(response);
        }
    }
}