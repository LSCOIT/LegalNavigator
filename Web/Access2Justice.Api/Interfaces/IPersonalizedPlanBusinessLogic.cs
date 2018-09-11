using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models;
using System;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IPersonalizedPlanBusinessLogic
    {
        Task<PersonalizedActionPlanViewModel> GeneratePersonalizedPlanAsync(CuratedExperience curatedExperience, Guid answersDocId);
        Task<PersonalizedPlanSteps> GetPersonalizedPlan(string planId);
        Task<PersonalizedActionPlanViewModel> UpdatePersonalizedPlan(UserPersonalizedPlan userPlan);
    }
}