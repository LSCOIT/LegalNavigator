using ContentsExtractionApi.Models;
using CrawledContentsBusinessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ContentsExtractionApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class ExtractTextFromBytesController : ApiController
    {
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileBase"></param>
        /// <returns></returns>       
        public ExtractedContent Post(byte[] fileContnent)
        {
            var result = new ExtractedContent();
            try
            {
                result = TextExtractionModule.ExtractTextContent(fileContnent);
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return result;
        }     
    }
}