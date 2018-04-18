using ContentDataAccess;
using ContentsExtractionApi.Utilities;
using CrawledContentDataAccess.StateBasedContents;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ContentsExtractionApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class QuestionsAndAnswersController : ApiController
    {

        private IContentDataRepository contentDataRepository;

        /// <summary>
        /// connection string prefix 
        /// </summary>
        private const string prefix = "ContentsDb";


        /// <summary>
        /// ExtractCuratedContentsController
        /// </summary>
        /// <param name="crowledContentDataRepository"></param>
        public QuestionsAndAnswersController(IContentDataRepository crowledContentDataRepository)
        {
            this.contentDataRepository = crowledContentDataRepository;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<CrawledContentDataAccess.QuestionsAndAnswers> Get(string intent, string state)
        {
            var stateShortName = contentDataRepository.GetStateByName(state)?.ShortName;

            return contentDataRepository.GetQuestionsAndAnswers(intent, StateToConnectionStringMapper.ToConnectionString(prefix, stateShortName));

        }



    }
}