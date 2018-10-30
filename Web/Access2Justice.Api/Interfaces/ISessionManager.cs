using System;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;

namespace Access2Justice.Api.Interfaces
{
    public interface ISessionManager
    {
        CuratedExperience RetrieveCachedCuratedExperience(Guid id, HttpContext context);
    }
}