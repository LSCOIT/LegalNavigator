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
        [Route("api/topics/getresourcedetails/{ParentTopicId}")]
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

        //Added for Topic and Resource Tools API
        #region get all topic Guids when parentTopicId is mapped to topicTags or ReferenceTags for resources
        [HttpGet]
        [Route("api/topics/gettopicguid/{name}")]
        public async Task<IActionResult> GetTopicGuid(string name)
        {
            var topics = await topicsResourcesBusinessLogic.GetTopicAsync(name);
            return Ok(topics);
        }
        #endregion

        #region get topic sample schema
        [HttpGet]
        [Route("api/topics/getsampletopicdetails")]
        public async Task<IActionResult> GetSampleTopicDetails()
        {
            string name = "Sample";
            var topics = await topicsResourcesBusinessLogic.GetTopicDetailsAsync(name);
            return Ok(topics);
        }
        #endregion

        #region get resource sample schema based on type
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
            await topicsResourcesBusinessLogic.GetTopicMandatoryDetailsAsync(name);
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
        
        #region Create Topics Document
        [HttpPost]
        [Route("api/topics/createtopic/Upload")]
        public async Task<IActionResult> CreateTopic(IFormFile uploadedFile)
        {
            var path = uploadedFile.FileName;
            var topics = await topicsResourcesBusinessLogic.CreateTopicsAsync(path);
            return Ok(topics);
        }
        #endregion

        #region Create Topic Document
        [HttpPost]
        [Route("api/createtopicdocument")]
        public async Task<IActionResult> CreateTopicDocument(dynamic topic)
        {
            //var topics = await topicsResourcesBusinessLogic.CreateTopicDocumentAsync(topic);
            string json = "";
            using (StreamReader r = new StreamReader("C:\\Users\\v-sobhad\\Desktop\\CreateJSON\\TopicData.json"))
            {
                json = r.ReadToEnd();
            }
            var topics = await topicsResourcesBusinessLogic.CreateResourceDocumentAsync(json);

            return Ok(topics);
        }
        #endregion

        #region Create Resources Document
        [HttpPost]
        [Route("api/topics/createresource/upload")]
        public async Task<IActionResult> CreateResource(IFormFile uploadedFile)
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
            using (StreamReader r = new StreamReader("C:\\Users\\v-sobhad\\Desktop\\CreateJSON\\FormsData.json"))
            {
                resource = r.ReadToEnd();
            }
            var resources = await topicsResourcesBusinessLogic.CreateResourceDocumentAsync(resource);

            return Ok(resources);
        }
        #endregion

    }
}