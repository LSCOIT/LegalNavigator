using ContentsExtractionApi.Models;
using ContentsExtractionApi.Utilities;
using ContentDataAccess.StateBasedContents;
using CrawledContentsBusinessLayer;
using ContentDataAccess;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ContentsExtractionApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class ExtractSubTopicsController : ApiController
    {
        private IContentDataRepository crowledContentDataRepository;

        /// <summary>
        /// connection string prefix 
        /// </summary>
        private const string prefix = "ContentsDb";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="crowledContentDataRepository"></param>
        public ExtractSubTopicsController(IContentDataRepository crowledContentDataRepository)
        {
            this.crowledContentDataRepository = crowledContentDataRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentenceInput"></param>      
        /// <returns></returns>
        // POST api/<controller>
        public List<RelevantTopic> Post([FromBody]SentenceInput sentenceInput)
        {
            var result = new List<RelevantTopic>();
            try
            {
                var stateShortName = crowledContentDataRepository.GetStateByName(sentenceInput.State)?.ShortName;
                result = crowledContentDataRepository.GetRelevantTopicsSentenceAsPivot(sentenceInput.Sentence, StateToConnectionStringMapper.ToConnectionString(prefix, stateShortName));
                if(result == null)
                {
                    var extractedInput=TextExtractionModule.ExtractTextUsingLinguisticApi(sentenceInput.Sentence);
                    if (extractedInput != null)
                    {
                         for (int i = 0; i < extractedInput.Capacity; i++)
                        {

                            result = crowledContentDataRepository.GetRelevantTopicsDataAsPivot(extractedInput[i], StateToConnectionStringMapper.ToConnectionString(prefix, stateShortName));
                            if(result!= null)
                            {
                                return result;
                            }
                        }
                    }

                }
                return result;
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
    
}