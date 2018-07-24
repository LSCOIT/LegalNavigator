using System;
using System.Threading.Tasks;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models.CuratedExperience;

namespace Access2Justice.Api.Interfaces
{
    public interface IPersonalizedPlanBusinessLogic
    {
        Task<PersonalizedActionPlanViewModel> GeneratePersonalizedPlan(CuratedExperienceTree curatedExperience, Guid answersDocId);
    }
}