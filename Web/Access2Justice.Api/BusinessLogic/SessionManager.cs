using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace Access2Justice.Api.BusinessLogic
{
    public class SessionManager : ISessionManager
    {
        private readonly ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic;

        public SessionManager(ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic)
        {
            this.curatedExperienceBusinessLogic = curatedExperienceBusinessLogic;
        }

        public CuratedExperience RetrieveCachedCuratedExperience(Guid id, HttpContext context)
        {
            var cuExSession = context.Session.GetString(id.ToString());
            if (string.IsNullOrWhiteSpace(cuExSession))
            {
                var rawCuratedExperience = curatedExperienceBusinessLogic.GetCuratedExperienceAsync(id).Result;
                context.Session.SetObjectAsJson(id.ToString(), rawCuratedExperience);
            }

            return context.Session.GetObjectAsJson<CuratedExperience>(id.ToString());
        }
    }
}
