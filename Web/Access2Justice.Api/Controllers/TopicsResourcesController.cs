using System.IO;
using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
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

        //Added for Topic and Resource Tools API
        #region get all topic Guids when ParentTopicId is mapped to topicTags or ReferenceTags for resources
        [HttpGet]
        [Route("api/topics/gettopicdetails/{name}")]
        public async Task<IActionResult> GetTopicDetails(string name)
        {
            var topics = await topicsResourcesBusinessLogic.GetTopicDetailsAsync(name);
            return Ok(topics);
        }
        #endregion

        #region get topic sample document
        [HttpGet]
        [Route("api/topics/getsampletopicdetails")]
        public async Task<IActionResult> GetSampleTopicDetails()
        {
            string name = "Sample";
            var topics = await topicsResourcesBusinessLogic.GetTopicDetailsAsync(name);
            return Ok(topics);
        }
        #endregion

        #region get resource sample document based on type
        [HttpGet]
        [Route("api/topics/getsampleresourcedetails/{type}")]
        public async Task<IActionResult> GetSampleResourceDetails(string type)
        {
            string name = "Sample";
            var resources = await topicsResourcesBusinessLogic.GetResourceDetailAsync(name, type);
            return Ok(resources);
        }
        #endregion

        #region get topic schema
        [HttpGet]
        [Route("api/topics/getschematopic")]
        public async Task<IActionResult> GetSchemaTopic(Topic topic)
        {
            var topics = new Topic();
            var name = topic.Name;
            await topicsResourcesBusinessLogic.GetTopicDetailsAsync(name);
            return null;
        }
        #endregion

        #region get action plan schema
        [HttpGet]
        [Route("api/topics/getschemaactionplan")]
        public async Task<IActionResult> GetSchemaActionPlan(ActionPlan actionPlan)
        {
            var actionPlans = new ActionPlan();
            var name = actionPlans.Name;
            var type = "Action Plans";
            await topicsResourcesBusinessLogic.GetResourceDetailAsync(name,type);
            return null;
        }
        #endregion

        #region get article schema
        [HttpGet]
        [Route("api/topics/getschemaarticle")]
        public async Task<IActionResult> GetSchemaArticle(Article article)
        {
            var articles = new Article();
            var name = articles.Name;
            var type = "Articles";
            await topicsResourcesBusinessLogic.GetResourceDetailAsync(name, type);
            return null;
        }
        #endregion

        #region get video schema
        [HttpGet]
        [Route("api/topics/getschemavideo")]
        public async Task<IActionResult> GetSchemaVideo(Video video)
        {
            var videos = new Video();
            var name = videos.Name;
            var type = "Videos";
            await topicsResourcesBusinessLogic.GetResourceDetailAsync(name, type);
            return null;
        }
        #endregion

        #region get organizations schema
        [HttpGet]
        [Route("api/topics/getschemaorganization")]
        public async Task<IActionResult> GetSchemaOrganization(Organization form)
        {
            var organizations = new Organization();
            var name = organizations.Name;
            var type = "Organizations";
            await topicsResourcesBusinessLogic.GetResourceDetailAsync(name, type);
            return null;
        }
        #endregion

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
        #region get form schema
        [HttpGet]
        [Route("api/topics/getschemaform")]
        public async Task<IActionResult> GetSchemaForm(Form form)
        {
            var forms = new Form();
            var name = form.Name;
            var type = "Forms";
            await topicsResourcesBusinessLogic.GetResourceDetailAsync(name, type);
            return null;
        }
        #endregion

        #region get essential reading schema
        [HttpGet]
        [Route("api/topics/getschemaessentialreading")]
        public async Task<IActionResult> GetSchemaEssentialReading(EssentialReading essentialReading)
        {
            var essentialReadings = new EssentialReading();
            var name = essentialReading.Name;
            var type = "Essential Readings";
            await topicsResourcesBusinessLogic.GetResourceDetailAsync(name, type);
            return null;
        }
        #endregion

        #region Create Resource Documents using upload
        [HttpPost]
        [Route("api/topics/createresources/upload")]
        public async Task<IActionResult> CreateResources(IFormFile uploadedFile)
        {
            var path = uploadedFile.FileName;
            var resources = await topicsResourcesBusinessLogic.CreateResourcesUploadAsync(path);
            return Ok(resources);
        }
        #endregion

        #region Create Resource Document
        [HttpPost]
        [Route("api/createresourcedocument")]
        public async Task<IActionResult> CreateResourceDocument(dynamic resource)
        {
            using (StreamReader r = new StreamReader("C:\\Users\\v-sobhad\\Desktop\\CreateJSON\\EssentialReadingsData.json"))
            {
                resource = r.ReadToEnd();
            }
            var resources = await topicsResourcesBusinessLogic.CreateResourceDocumentAsync(resource);

            return Ok(resources);
        }
        #endregion

        #region Create Topic Documents using upload 
        [HttpPost]
        [Route("api/topics/createtopics/upload")]
        public async Task<IActionResult> CreateTopics(IFormFile uploadedFile)
        {
            var path = uploadedFile.FileName;
            var topics = await topicsResourcesBusinessLogic.CreateTopicsUploadAsync(path);
            return Ok(topics);
        }
        #endregion

        #region Create Topic Document
        [HttpPost]
        [Route("api/createtopicdocument")]
        public async Task<IActionResult> CreateTopicDocument(dynamic topic)
        {
            using (StreamReader r = new StreamReader("C:\\Users\\v-sobhad\\Desktop\\CreateJSON\\TopicData.json"))
            {
                topic = r.ReadToEnd();
            }
            var topics = await topicsResourcesBusinessLogic.CreateTopicDocumentAsync(topic);

            return Ok(topics);
        }
        #endregion
    }
}