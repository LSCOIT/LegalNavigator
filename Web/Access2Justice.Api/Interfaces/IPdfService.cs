using System.Threading.Tasks;
using Access2Justice.Api.ViewModels;

namespace Access2Justice.Api.Interfaces
{
    public interface IPdfService
    {
        Task<byte[]> PrintPlan(PersonalizedPlanViewModel personalizedPlan);
    }
}