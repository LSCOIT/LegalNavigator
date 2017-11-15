using ContentDataAccess;
using ContentsExtractionApi.Models;
using ContentsExtractionApi.Utilities;
using CrawledContentDataAccess.StateBasedContents;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ContentsExtractionApi.Controllers
{
    /// <summary>
    /// ExtractNSMIContentsController , delivers data related to CuratedExperience
    /// </summary>
    ///  [EnableCors("*", "*", "*")]
    public class ExtractNSMIContentsController : ApiController
    {

        private ICrowledContentDataRepository crowledContentDataRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crowledContentDataRepository"></param>
        public ExtractNSMIContentsController(ICrowledContentDataRepository crowledContentDataRepository)
        {
            this.crowledContentDataRepository = crowledContentDataRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET api/ExtractNSMIContents
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/ExtractNSMIContents/5
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nsmiInput"></param>
        /// <returns></returns>
        // POST api/ExtractNSMIContents
        public NSMIContent Post([FromBody]NSMIInput nsmiInput)
        {
            var result = new NSMIContent ();
            try
            {
                result = crowledContentDataRepository.GetNSMIContent(nsmiInput.NsmiCode, StateToConnectionStringMapper.ToConnectionString(nsmiInput.State));

            }
            catch(Exception ex)
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