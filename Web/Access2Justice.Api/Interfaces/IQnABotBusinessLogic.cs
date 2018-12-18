using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IQnABotBusinessLogic
    {
        Task<dynamic> GetAnswersAsync(string question);
    }
}
