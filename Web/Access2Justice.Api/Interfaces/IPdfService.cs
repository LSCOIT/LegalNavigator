using System.Collections.Generic;
using System.Threading.Tasks;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models;

namespace Access2Justice.Api.Interfaces
{
    public interface IPdfService
    {
        Task<byte[]> PrintPlan(PersonalizedPlanViewModel personalizedPlan);
        Task<byte[]> PrintTopic(TopicView personalizedPlan, IEnumerable<dynamic> resources);
        Task<byte[]> PrintResource(dynamic personalizedPlan);
    }
}