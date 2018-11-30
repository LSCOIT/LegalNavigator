using Access2Justice.Api.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IStateProvinceBusinessLogic
    {
        Task<List<StateCode>> GetStateCodes();

        Task<string> GetStateCodeForState(string stateName);
    }
}
