using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Chatbot")]
    public class ChatbotController : Controller
    {
        private const decimal threshold = 0.92m;
        private readonly Helper _helper;

        public ChatbotController(Helper helper)
        {
            _helper = helper;
        }

        [HttpGet]
        public string Get(string input)
        {
            var intentWithScore = Helper.GetIntentFromLuisApi(input);
            string topScorongIntent = null;
            if (intentWithScore != null)
            {
                topScorongIntent = intentWithScore.TopScoringIntent;
                return _helper.GetChatResponse(topScorongIntent);
            }
            return "Glad to talk to you. Could you describe your problem in detail?";
        }

    }
}