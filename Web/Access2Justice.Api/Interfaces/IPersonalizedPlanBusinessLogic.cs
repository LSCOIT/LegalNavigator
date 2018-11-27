using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using System;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IPersonalizedPlanBusinessLogic
    {
        Task<PersonalizedPlanViewModel> GeneratePersonalizedPlanAsync(CuratedExperience curatedExperience, Guid answersDocId);
        Task<PersonalizedPlanViewModel> GetPersonalizedPlanAsync(Guid personalizedPlanId);
        Task<Document> UpsertPersonalizedPlanAsync(UserPlan userPlan);
    }
}