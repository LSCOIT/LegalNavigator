﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ILuisBusinessLogic
    {
        IEnumerable<string> FilterLuisIntents(IntentWithScore intentWithScore);
        Task<dynamic> GetInternalResources(string query);
        Task<dynamic> GetTopicAsync(string keywords);
        IntentWithScore ParseLuisIntent(string LuisResponse);
    }
}
