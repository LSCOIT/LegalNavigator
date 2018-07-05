using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Models.CuratedExperience;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Api.BusinessLogic
{
    public class CuratedExperienceBuisnessLogic
    {
        public Component GetQuestion(Guid buttonId)
        {
            var curatedExperience = GetCuratedExperience(buttonId);

            throw new NotImplementedException();
        }

        public CuratedExperience GetCuratedExperience(Guid id)
        {

            throw new NotImplementedException();
        }
    }
}
