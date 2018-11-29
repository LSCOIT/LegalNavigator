using Access2Justice.Api.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface ILocationBusinessLogic
    {
        Task<List<StateCode>> GetStateCodes();
    }
}
