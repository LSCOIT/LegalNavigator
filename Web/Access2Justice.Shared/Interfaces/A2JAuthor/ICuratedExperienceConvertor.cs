using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface ICuratedExperienceConvertor
    {
        CuratedExperience ConvertA2JAuthorToCuratedExperience(JObject a2jSchema,bool isFromAdminImport = false, Guid a2jPersonalizedPlanId = default(Guid));
    }
}
