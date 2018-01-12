using ContentDataAccess;
using ContentsExtractionApi.Models;
using ContentsExtractionApi.Utilities;
using CrawledContentDataAccess.StateBasedContents;
using CrawledContentsBusinessLayer;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;

namespace ContentsExtractionApi.Controllers
{
    /// <summary>
    /// ExtractNSMIContentsController , delivers data related to CuratedExperience
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class ExtractCuratedContentsController : ApiController
    {

        private IContentDataRepository crowledContentDataRepository;

        /// <summary>
        /// connection string prefix 
        /// </summary>
        private const string prefix = "ContentsDb";

        /// <summary>
        /// threshold for low confidence intents 
        /// </summary>
        private const decimal treshold = 0.92m;

        /// <summary>
        /// ExtractCuratedContentsController
        /// </summary>
        /// <param name="crowledContentDataRepository"></param>
        public ExtractCuratedContentsController(IContentDataRepository crowledContentDataRepository)
        {
            this.crowledContentDataRepository = crowledContentDataRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET api/ExtractNSMIContents
       /* public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scenarioId"></param>
        /// <param name="state"></param>       
        /// <returns></returns>
        // GET api/ExtractNSMIContents/5
        public CrawledContentDataAccess.CuratedContent Get(int scenarioId,string state )
        {
            var CuratedResult = new CrawledContentDataAccess.CuratedContent();
            try
            {
                CuratedResult = crowledContentDataRepository.GetCuratedContent(scenarioId, StateToConnectionStringMapper.ToConnectionString(prefix,state));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return CuratedResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nsmiInput"></param>
        /// <returns></returns>
        // POST api/ExtractNSMIContents
        public CrawledContentDataAccess.CuratedContent Post([FromBody]NSMIInput nsmiInput)
        {
            var  CuratedResult = new CrawledContentDataAccess.CuratedContent();
            try
            {
                //var intent = TextExtractionModule.GetIntentFromLuisApi(nsmiInput.Sentence);

                var intentWithScore = TextExtractionModule.GetIntentFromLuisApi(nsmiInput.Sentence);
                string topScorongIntent = null;
                if (intentWithScore != null  && intentWithScore.IsSuccessful)
                {
                    topScorongIntent = intentWithScore.TopScoringIntent;
                   

                }
                else
                {

                }

                CuratedResult = crowledContentDataRepository.GetCuratedContent(topScorongIntent, StateToConnectionStringMapper.ToConnectionString(prefix, nsmiInput.State));
                if(CuratedResult== null)
                {
                    CuratedResult = new CrawledContentDataAccess.CuratedContent();
                }
                if (intentWithScore != null && intentWithScore.IsSuccessful)
                {
                    if (intentWithScore.Score < treshold)
                    {
                        //return "intentWithScore.TopTwoIntents" as part of the response, to generated questions to confirm the user Intent.
                        CuratedResult.TopTwoIntentsForLowConfidenceIntents = intentWithScore.TopTwoIntents;
                    }
                }
                else if(intentWithScore != null)
                {
                    ModelState.AddModelError(string.Empty, intentWithScore.ErrorMessage);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex?.Message);
            }
            return CuratedResult;
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