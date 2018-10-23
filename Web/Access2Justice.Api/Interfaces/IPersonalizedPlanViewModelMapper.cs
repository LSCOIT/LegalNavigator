using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IPersonalizedPlanViewModelMapper
    {
        Task<PersonalizedPlanViewModel> MapViewModel(UnprocessedPersonalizedPlan personalizedPlanStepsInScope);
    }
}
