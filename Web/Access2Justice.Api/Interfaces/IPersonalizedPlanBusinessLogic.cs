using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IPersonalizedPlanBusinessLogic
    {
        Task<PersonalizedPlanViewModel> GeneratePersonalizedPlanAsync(CuratedExperience curatedExperience, Guid answersDocId, string userId = null);
        Task<PersonalizedPlanViewModel> GetPersonalizedPlanAsync(Guid personalizedPlanId);
        Task<Document> UpsertPersonalizedPlanAsync(UserPlan userPlan);
        Task UpsertExternalPersonalizedPlanAsync(PersonalizedPlanViewModel personalizedPlanView, string oId);
        Task<List<PlanTopic>> GetPlanTopicsAsync(List<string> planIds);
    }
}