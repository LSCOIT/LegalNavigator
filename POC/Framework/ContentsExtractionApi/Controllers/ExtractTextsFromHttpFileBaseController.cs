using CrawledContentsBusinessLayer;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

/// <summary>
/// 
/// </summary>
namespace ContentsExtractionApi.Controllers
{


    [EnableCors("*", "*", "*")]
    public class ExtractTextsFromHttpFileBaseController : ApiController
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileBase"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        //[Produces("text/html")]
        //public ExtractedContent Post(MultipartDataMediaFormatter.Infrastructure.FormData fileBase)
        public HttpResponseMessage Post(MultipartDataMediaFormatter.Infrastructure.FormData fileBase)

        {
            var messageCategories = new[] {
                new[] { "vacate", "rent", "tenant", "premises", "landlord", "tenancy" },
                new[] { "marriage", "domestic", "partnership", "legal separation", "spouse", "partner" }
            };



            var responseCategories = new[] { "<html> <body><p style=\"line - height: 18.0pt; background: white; margin: 3.95pt 0in 12.0pt 0in; \"><span style=\"font - size: 12.0pt; font - family: 'Noto Sans'; color: black; \">Your landlord is trying to evict you. <strong>You must respond in writing. Otherwise you must move out without getting a court hearing first.</strong></span></p>" +
         "<ul style = \"margin-top: 0in;\" >" +
          "<li style =\"color: black; margin-top: 3.95pt; margin-bottom: 12.0pt; line-height: 18.0pt; tab-stops: list .5in; background: white;\" ><span style = \"font-size: 12.0pt; font-family: 'Noto Sans';\" > First, try to get help from a lawyer. If you are low-income, call the CLEAR line at 1 - 888 - 201 - 1014.A lawyer at CLEAR may be able to help you over the phone. Or s/ he may be able to refer you to a free or low - cost lawyer who can help you in person.If you are not low - income, try to see a private lawyer.Ask your friends for recommendations for lawyers they have worked with. &nbsp;Or look in the yellow pages under<em>Attorneys.</em></span></li>" +
"<li style = \"color: black; margin-top: 3.95pt; margin-bottom: 12.0pt; line-height: 18.0pt; tab-stops: list .5in; background: white;\" ><span style= \"font-size: 12.0pt; font-family: 'Noto Sans';\" > Next, you must write and deliver a \"Notice of Appearance\" or an \"Answer\".&nbsp; You do not have much time to do this. It is very important to submit these documents on time even if you do not have legal help.</span></li>" +
"</ul></body></html>",
            "<p style=\"line - height: 18.0pt; background: white; margin: 3.95pt 0in 12.0pt 0in; \"><span style=\"font - size: 12.0pt; font - family: 'Noto Sans'; color: black; \">When you are served with legal papers, you must figure out right away how to respond. If you do not respond on time, your spouse will automatically get what s/he wants. <strong><span style=\"font - family: 'Noto Sans'; \">For a motion, you may have as few as four business days after receiving the papers to file your response. </span></strong>It takes time to find legal resources and read through this packet. Start <strong><span style=\"font - family: 'Noto Sans'; \">as soon as you can</span></strong>. If you cannot respond in time, you must file <em><span style=\"font - family: 'Noto Sans'; \">a Notice of Appearance</span></em> and ask for a <em><span style=\"font - family: 'Noto Sans'; \">continuance.</span></em> (See below.)</span></p>"
                     + "<p style = \"line-height: 18.0pt; background: white; margin: 6.0pt 0in 6.0pt 0in;\" ><strong ><span style = \"font-size: 17.0pt; font-family: 'Noto Sans'; color: black;\" > Talk with a lawyer </span ></strong ></p>"
                                 + "<p style = \"line-height: 18.0pt; background: white; margin: 3.95pt 0in 12.0pt 0in;\" ><span style = \"font-size: 12.0pt; font-family: 'Noto Sans'; color: black;\" > Talk to a lawyer familiar with family law before filing anything. Some counties have family law facilitators who can help fill out forms or free legal clinics that give legal advice.</span ></p>"
                                          + "<ul style = \"margin-top: 0in;\" >"
                                           + "<li style = \"color: black; margin-top: 3.95pt; margin-bottom: 12.0pt; line-height: 18.0pt; tab-stops: list .5in; background: white;\" ><strong><span style = \"font-size: 12.0pt; font-family: 'Noto Sans';\" > Do you live in King County? Call 211. </spa ></strong ><span style = \"font-size: 12.0pt; font-family: 'Noto Sans';\" > 211 is open Monday through Friday between 8:00 a.m.and 6:00 p.m.& nbsp; From a pay /public phone, call 1-800-621-4636. 211 will identify and refer you to the appropriate legal aid provider.</span></li>"
+ "<li style = \"color: black; margin-top: 3.95pt; margin-bottom: 12.0pt; line-height: 18.0pt; tab-stops: list .5in; background: white;\" ><strong ><span style=\"font-size: 12.0pt; font-family: 'Noto Sans';\">Apply online with</span></strong><span style = \"font-size: 12.0pt; font-family: 'Noto Sans';\" ><a href=\"https://nwjustice.org/get-legal-help\"><strong><span style = \"font-family: 'Noto Sans'; color: #336e93; text-decoration: none; text-underline: none;\"> CLEAR * Online </ span ></strong ></a ><strong ><span style=\"font-family: 'Noto Sans';\"> -&nbsp; </span></strong><a href = \"https://nwjustice.org/get-legal-help\" > https://nwjustice.org/get-legal-help</a></span></li>"
+ "<li style = \"color: black; margin-top: 3.95pt; margin-bottom: 12.0pt; line-height: 18.0pt; tab-stops: list .5in; background: white;\" ><strong ><span style=\"font-size: 12.0pt; font-family: 'Noto Sans';\">Call the CLEAR Legal Hotline at 1-888-201-1014.</span></strong></li>"
+ "</ul>" };
            var notFoundNotice= "Could not find notice, ask your question in the chat bot";

            var result= TextExtractionModule.ExtractTextContent(fileBase.Files[0].Value.Buffer);
            var response = new HttpResponseMessage();
            if (result != null && !string.IsNullOrEmpty(result.Summary))
            {
                for (int i=0; i< messageCategories.Length; i++)
                {
                    foreach (var item in messageCategories[i])
                    {
                        if (result.Summary.ToLower().Contains(item))
                        {                            
                            
                            response.Content = new StringContent(responseCategories[i]);
                            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                            return response;                           
                        }
                    }
                }               
            }

            response.Content = new StringContent(notFoundNotice);
            response.Content.Headers.ContentType= new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}