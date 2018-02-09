﻿using ContentDataAccess;
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

        private ILogger logger;

        /// <summary>
        /// ExtractCuratedContentsController
        /// </summary>
        /// <param name="crowledContentDataRepository"></param>
        public ExtractCuratedContentsController(IContentDataRepository crowledContentDataRepository)
        {
            this.crowledContentDataRepository = crowledContentDataRepository;
            logger = new LoggerConfiguration()
                     .WriteTo.Console()
                     .CreateLogger();


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
        public CrawledContentDataAccess.CuratedContentForAScenario Get(int scenarioId,string state )
        {
            var CuratedResult = new CrawledContentDataAccess.CuratedContentForAScenario();
            CuratedResult.SelectedState = state;
           // try
           // {
                logger.Information("scenario id = {0} , state = {1}", scenarioId, state);
                var stateShortName = crowledContentDataRepository.GetStateByName(state)?.ShortName;

                logger.Information("short name from Db = {0} ", stateShortName);
                var lowConfidenceIntents=
                CuratedResult = crowledContentDataRepository.GetCuratedContent(scenarioId, StateToConnectionStringMapper.ToConnectionString(prefix,stateShortName));
                CuratedResult.SelectedState = stateShortName;
                if (CuratedResult != null && CuratedResult.CurrentIntent != null) {
                    var intentWithScore = TextExtractionModule.GetIntentFromLuisApi(CuratedResult.CurrentIntent);
                    if (intentWithScore != null && intentWithScore.IsSuccessful)
                    {
                        //if (intentWithScore.Score < treshold)
                       // {
                            //return "intentWithScore.TopTwoIntents" as part of the response, to generated questions to confirm the user Intent.
                            CuratedResult.TopTwoIntentsForLowConfidenceIntents = intentWithScore.TopSixIntents;
                       // }
                    }

                }
          //  }
          //  catch (Exception ex)
           // {
            //    ModelState.AddModelError(string.Empty, ex.Message);

              //  throw;
           // }
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
               
                var stateShortName = crowledContentDataRepository.GetStateByName(nsmiInput.State)?.ShortName;
                CuratedResult = crowledContentDataRepository.GetCuratedContent(topScorongIntent, StateToConnectionStringMapper.ToConnectionString(prefix, stateShortName));
                if(CuratedResult== null)
                {
                    CuratedResult = new CrawledContentDataAccess.CuratedContent();
                }
                if (intentWithScore != null && intentWithScore.IsSuccessful)
                {
                    if (intentWithScore.Score < treshold)
                    {
                        //return "intentWithScore.TopTwoIntents" as part of the response, to generated questions to confirm the user Intent.
                        CuratedResult.TopSixIntentsForLowConfidenceIntents = intentWithScore.TopSixIntents;
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