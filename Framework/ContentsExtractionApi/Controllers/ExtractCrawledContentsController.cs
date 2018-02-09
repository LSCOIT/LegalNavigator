using ContentsExtractionApi.Models;
using ContentsExtractionApi.Utilities;
using ContentDataAccess.StateBasedContents;
using CrawledContentsBusinessLayer;
using CrawledContentsBusinessLayer.WlhDataExtraction;
using ContentDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
namespace ContentsExtractionApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [EnableCors("*", "*", "*")]
    public class ExtractCrawledContentsController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        private IContentDataRepository crowledContentDataRepository;
        /// <summary>
        /// connection string prefix 
        /// </summary>
        private const string prefix = "ContentsDb";

        /// <summary>
        /// threshold for low confidence intents 
        /// </summary>
        private const decimal treshold = 0.92m;

        /// <param name="crowledContentDataRepository"></param>
        public ExtractCrawledContentsController(IContentDataRepository crowledContentDataRepository)
        {
            this.crowledContentDataRepository = crowledContentDataRepository;
        }
        /// <summary>
        /// Provide entire topics, subtopics, document contents
        /// </summary>
        /// <returns></returns>
        // GET api/ExtractContents      
     
        public IEnumerable<Topic> Get(string state)
        //public string Get(string state)
        {
            //try
            //{
            //    List<Topic> topics = new List<Topic> {
            //    new Topic {
            //    Name = "Topicname",
            //    SubTopics = new List<SubTopic> { new SubTopic { Name="Subtopicname", Docs = new List<Document>() { new Document { Url="url", Title="title", Content="content" }  }, Url="url" } },
            //    Url = "url"
            //    }
            //};

            //    topics.ForEach(topic => crowledContentDataRepository.Save(topic));
            //    return new string[] { "value1", "value2" };
            //}
            //catch(Exception e)
            //{
            //    return new string[] { e.Message, e.StackTrace };
            //}
            try
            {
                var stateShortName = crowledContentDataRepository.GetStateByName(state)?.ShortName;
                // return stateShortName;
                return crowledContentDataRepository.GetTopics(StateToConnectionStringMapper.ToConnectionString(prefix, stateShortName));
            }
            catch(Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the first topic
        /// </summary>
        /// <param name="state"></param>
        /// <param name="id"></param>
        /// <returns></returns>C:\Users\v-gedem\Source\Repos\107465547\TopicsAndContentsCrawler\ContentsExtractionApi\Models\
        // GET api/ExtractContents/5       
        public Topic Get(string state, int id)
        {
            //  return crowledContentDataRepository.GetTopic(id, StateToConnectionStringMapper.ToConnectionString(prefix, state));
            return null;
        }

        /// <summary>
        /// Provides relevant content based on topic and title
        /// </summary>
        /// <param name="contentExtractionRequest"></param>
        /// <returns></returns>
        // POST api/ExtractContents  
        [EnableCors("*", "*", "*")]
        public string Post([FromBody]ContentExtractionRequest contentExtractionRequest)
        {
           // var keyphrases = new []{ "vacate","evict", "rent", "tenant", "premises", "landlord", "tenancy" , "marriage", "domestic", "partnership", "legal separation", "spouse", "partner", "adopt","child", "support","assault","abuse","violence","violent","foreclosure","bankruptcy","theft","debt","naturalization", };
            if (contentExtractionRequest != null)
            {
               // var result = crowledContentDataRepository.GetRelevantContentTopDown(contentExtractionRequest.Topic, contentExtractionRequest.Title);
                
                //If no matching is found
               // if (string.IsNullOrEmpty(result))
               // {
                    var intentWithScore = TextExtractionModule.GetIntentFromLuisApi(contentExtractionRequest.Topic);
                    string topScorongIntent = null;
                    if (intentWithScore != null)
                    {
                      topScorongIntent = intentWithScore.TopScoringIntent;
                      if(intentWithScore.Score < treshold)
                      {
                        //return "intentWithScore.TopTwoIntents" as part of the response, to generated questions to confirm the user Intent.
                        
                      }

                    }
                    var stateShortName = crowledContentDataRepository.GetStateByName(contentExtractionRequest.State)?.ShortName;
                    var result=crowledContentDataRepository.GetRelevantContentTopDown(topScorongIntent, contentExtractionRequest.Title, StateToConnectionStringMapper.ToConnectionString(prefix, stateShortName));
                    if (string.IsNullOrEmpty(result))
                    {
                        // var phrases = StopWordUtilities.RemoveStopWordsFromSentence(contentExtractionRequest.Title?.ToLower());
                        var phrases = TextExtractionModule.ExtractTextUsingLinguisticApi(contentExtractionRequest.Title);
                        StringBuilder sb = new StringBuilder();
                        foreach (var phrase in phrases)
                        {
                            var result_item = crowledContentDataRepository.GetRelevantContentTopDown(topScorongIntent, phrase, StateToConnectionStringMapper.ToConnectionString(prefix, stateShortName));
                            sb.Append(result_item + "<br/>");
                        }
                        result = sb.ToString();

                    }
                //}

                return result;
            }
            return "null is provided as input";
        }
       
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/ExtractContents/5
        public void Delete(int id, string state)
        {
            try
            {
                var statesToPersist =new [] { "AL", "WA" };
                var stateUrls = new[] { @"https://alaskalawhelp.org", @"http://www.washingtonlawhelp.org" };//get state url from DB by state
                var stateurlIndex = 0;
               
                    WebCrawler wc = new WebCrawler();
                    WebCrawler.s_statelhUrlBase = stateUrls[stateurlIndex++];
                    var topics = wc.GetWebPageList();
                var stateShortName = crowledContentDataRepository.GetStateByName(state)?.ShortName;
                topics.ForEach(topic => crowledContentDataRepository.Save(topic, StateToConnectionStringMapper.ToConnectionString(prefix, stateShortName)));



              /*WebCrawler wc = new WebCrawler();
              var topics = wc.GetWebPageList();
              topics.ForEach(topic => crowledContentDataRepository.Save(topic,StateToConnectionStringMapper.ToConnectionString("AL")));

                //    List<Topic> topics = new List<Topic> {
                //    new Topic {
                //    Name = "Topicname",
                //    SubTopics = new List<SubTopic> { new SubTopic { Name="Subtopicname", Docs = new List<Document>() { new Document { Url="url", Title="title", Content="content" }  }, Url="url" } },
                //    Url = "url"
                //    }
                //};

                topics.ForEach(topic => crowledContentDataRepository.Save(topic));*/

            }
            catch (Exception e)
            {
                return;
            }
        }
        /// <summary>
        /// Saves all extracted topics, subtopics ,and documents in the database
        /// </summary>
        /// <returns></returns>       
        //public bool Save()
        //{
        //    try
        //    {

        //        WebCrawler wc = new WebCrawler();
        //        var topics= wc.GetWebPageList();
        //        topics.ForEach(topic => crowledContentDataRepository.Save(topic));

        //        //    List<Topic> topics = new List<Topic> {
        //        //    new Topic {
        //        //    Name = "Topicname",
        //        //    SubTopics = new List<SubTopic> { new SubTopic { Name="Subtopicname", Docs = new List<Document>() { new Document { Url="url", Title="title", Content="content" }  }, Url="url" } },
        //        //    Url = "url"
        //        //    }
        //        //};

        //        // topics.ForEach(topic => crowledContentDataRepository.Save(topic));

        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        
    }

    public class BrowserJsonFormatter : JsonMediaTypeFormatter
    {
        public BrowserJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            this.SerializerSettings.Formatting = Formatting.Indented;
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}
