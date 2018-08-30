using System;
using System.Threading.Tasks;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models;

namespace Access2Justice.Api.Interfaces
{
    public interface IPersonalizedPlanBusinessLogic
    {
        Task<PersonalizedPlanSteps> GeneratePersonalizedPlan(CuratedExperience curatedExperience, Guid answersDocId);
        Task<PersonalizedActionPlanViewModel> GetPlanDataAsync(string planId);
        Task<PersonalizedPlanSteps> GetPersonalizedPlan(string planId);
        Task<PersonalizedActionPlanViewModel> UpdatePersonalizedPlan(UserPersonalizedPlan userPlan);
        
    }
}