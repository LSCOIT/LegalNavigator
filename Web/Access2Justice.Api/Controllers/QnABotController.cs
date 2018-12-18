using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/qnabot")]
    public class QnABotController : Controller
    {
        private readonly IQnABotBusinessLogic qnABotBusinessLogic;

        public QnABotController(IQnABotBusinessLogic qnABotBusinessLogic)
        {
            this.qnABotBusinessLogic = qnABotBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpGet("{question}")]
        public async Task<IActionResult> GetAsync(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return BadRequest("Question cannot be empty string.");
            }

            var response = await qnABotBusinessLogic.GetAnswersAsync(question);
            return Content(response);
        }
    }
}