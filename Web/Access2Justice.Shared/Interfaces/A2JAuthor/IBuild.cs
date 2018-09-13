using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface IPersonalizedPlanEngine
    {
        List<JToken> Build(JObject personalizedPlan, CuratedExperienceAnswers userAnswers);
    }
}