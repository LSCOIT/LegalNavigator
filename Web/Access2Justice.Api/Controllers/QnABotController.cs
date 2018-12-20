﻿using System.Threading.Tasks;
using Access2Justice.Api.Interfaces;
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
        /// This makes Luis and QnA API call to get the answers for the user questions in chat window.
        /// </summary>
        /// <param name="question">input from chat window</param>
        /// <param name="isLuisCallRequired"></param>
        /// <returns></returns>
        [HttpGet("{question}/{isluiscallrequired}")]
        public async Task<IActionResult> GetAsync(string question,bool isLuisCallRequired)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return BadRequest("Question cannot be empty string.");
            }

            var response = await qnABotBusinessLogic.GetAnswersAsync(question, isLuisCallRequired);
            return Content(response);
        }
    }
}