using ContentDataAccess;
using ContentsExtractionApi.Models;
using ContentsExtractionApi.Utilities;
using CrawledContentsBusinessLayer;
using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;


namespace ContentsExtractionApi.Controllers
{
    /// <summary>
    /// ExtractNSMIContentsController , delivers data related to CuratedExperience
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class ExtractCuratedContentsController : ApiController
    {

        private IContentDataRepository contentDataRepository;

        /// <summary>
        /// connection string prefix 
        /// </summary>
        private const string prefix = "ContentsDb";

        /// <summary>
        /// threshold for low confidence intents 
        /// </summary>
        private const decimal treshold = 0.92m;

        /// <summary>
        /// 
        /// </summary>
        private string[] separators = { ",", ".", "!", "?", ";", ":", " " };


        /// <summary>
        /// 
        /// </summary>
        private string dbSourceConnectionStringName = ConfigurationManager.AppSettings["SourceConnectionString"];


        /// <summary>
        /// ExtractCuratedContentsController
        /// </summary>
        /// <param name="crowledContentDataRepository"></param>
        public ExtractCuratedContentsController(IContentDataRepository crowledContentDataRepository)
        {
            this.contentDataRepository = crowledContentDataRepository;
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
        /// <param name="trnaslateTo"></param>       
        /// <returns></returns>
        // GET api/ExtractNSMIContents/5
        public CrawledContentDataAccess.CuratedContentForAScenario Get(int scenarioId,string state, string trnaslateTo="en" )
        {
            var CuratedResult = new CrawledContentDataAccess.CuratedContentForAScenario();
            CuratedResult.SelectedState = state;
           // try
           // {
                  var stateShortName = contentDataRepository.GetStateByName(state)?.ShortName;

                 
                CuratedResult = contentDataRepository.GetCuratedContent(scenarioId, StateToConnectionStringMapper.ToConnectionString(prefix,stateShortName, trnaslateTo != "en"?trnaslateTo:null),trnaslateTo);
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
                if (!string.IsNullOrEmpty(nsmiInput?.TranslateFrom?.Trim()) && nsmiInput?.TranslateFrom != "en")
                {
                    
                       nsmiInput.Sentence = TextExtractionModule.TextTranslate(nsmiInput.Sentence, nsmiInput.TranslateFrom, "en");
                      var length = nsmiInput.Sentence.Length;
                    
                       var currentTranslationUsage = contentDataRepository.GetCurrentTranslationUsage(dbSourceConnectionStringName);
                       currentTranslationUsage.UsedTillNow = currentTranslationUsage.UsedTillNow + length;
                       currentTranslationUsage.LastUpdated = length;
                       currentTranslationUsage.LastRunTime = DateTime.Now;
                       contentDataRepository.Update(currentTranslationUsage, dbSourceConnectionStringName);
                }
                var intentWithScore = TextExtractionModule.GetIntentFromLuisApi(nsmiInput?.Sentence);
                string topScorongIntent = null;
                if (intentWithScore != null  && intentWithScore.IsSuccessful)
                {
                    topScorongIntent = intentWithScore.TopScoringIntent;   

                }

                if (string.IsNullOrEmpty(nsmiInput?.TranslateTo?.Trim()) && !string.IsNullOrEmpty(nsmiInput?.TranslateFrom?.Trim()))
                {
                    nsmiInput.TranslateTo = nsmiInput.TranslateFrom;
                }

                    var stateShortName = contentDataRepository.GetStateByName(nsmiInput.State)?.ShortName;
                CuratedResult = contentDataRepository.GetCuratedContent(topScorongIntent, StateToConnectionStringMapper.ToConnectionString(prefix, stateShortName, nsmiInput.TranslateTo != "en" ? nsmiInput.TranslateTo : null), nsmiInput.TranslateTo);
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