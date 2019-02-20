using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Access2Justice.Api
{
    public class LuisBusinessLogic : ILuisBusinessLogic
    {
        private readonly ILuisProxy luisProxy;
        private readonly ILuisSettings luisSettings;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;
        private readonly IWebSearchBusinessLogic webSearchBusinessLogic;
        private readonly IBingSettings bingSettings;
        private dynamic luisTopIntents = null;

        public LuisBusinessLogic(ILuisProxy luisProxy, ILuisSettings luisSettings, ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic, IWebSearchBusinessLogic webSearchBusinessLogic, IBingSettings bingSettings)
        {
            this.luisSettings = luisSettings;
            this.luisProxy = luisProxy;
            this.topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
            this.webSearchBusinessLogic = webSearchBusinessLogic;
            this.bingSettings = bingSettings;
        }

        public async Task<dynamic> GetResourceBasedOnThresholdAsync(LuisInput luisInput)
        {
            var encodedSentence = HttpUtility.UrlEncode(luisInput.Sentence);
            if (string.IsNullOrEmpty(luisInput.LuisTopScoringIntent))
            {
                dynamic luisResponse = await luisProxy.GetIntents(encodedSentence);
                luisTopIntents = ParseLuisIntent(luisResponse);
            }
            LuisViewModel luisViewModel = null;
            if ((luisTopIntents != null && IsIntentAccurate(luisTopIntents)) || !string.IsNullOrEmpty(luisInput.LuisTopScoringIntent))
            {
                luisViewModel = await GetInternalResourcesAsync(
                   luisTopIntents?.TopScoringIntent ?? luisInput.LuisTopScoringIntent,
                   luisInput,
                   luisTopIntents != null && luisTopIntents.TopNIntents != null ? luisTopIntents.TopNIntents : null);
            }
            //Will fetch web links only when there are no mapping LUIS Intent or no mapping resources to specific LUIS Intent
            return ((luisViewModel != null && luisViewModel.Resources != null &&
                ((JContainer)(luisViewModel.Resources)).Count > 0))
                || !string.IsNullOrEmpty(luisInput.LuisTopScoringIntent) ?
                JObject.FromObject(luisViewModel).ToString() :
            await GetWebResourcesAsync(encodedSentence);
        }

        public IntentWithScore ParseLuisIntent(string LuisResponse)
        {
            LuisIntent luisIntent = JsonConvert.DeserializeObject<LuisIntent>(LuisResponse);

            return new IntentWithScore
            {
                IsSuccessful = true,
                TopScoringIntent = luisIntent?.TopScoringIntent?.Intent,
                Score = luisIntent?.TopScoringIntent?.Score ?? 0,
                TopNIntents = luisIntent?.Intents.Skip(1).Take(luisSettings.TopIntentsCount).Select(x => x.Intent).ToList()
            };
        }

        public bool IsIntentAccurate(IntentWithScore intentWithScore)
        {
            return intentWithScore.Score >= luisSettings.IntentAccuracyThreshold && intentWithScore.TopScoringIntent.ToUpperInvariant() != "NONE";
        }

        public async Task<dynamic> GetInternalResourcesAsync(string keyword, LuisInput luisInput, IEnumerable<string> relevantIntents)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            Location location = luisInput.Location;

            var topics = await topicsResourcesBusinessLogic.GetTopicsAsync(textInfo.ToTitleCase(keyword), location);

            List<string> topicIds = new List<string>();
            foreach (var item in topics)
            {
                string topicId = item.Id;
                topicIds.Add(topicId);
            }

            if (topicIds.Count == 0 || location == null)
            {
                return new LuisViewModel
                {
                    TopIntent = keyword
                };
            }

            ResourceFilter resourceFilter = new ResourceFilter { TopicIds = topicIds, PageNumber = 0, ResourceType = Constants.All, Location = location };
            var GetResourcesTask = topicsResourcesBusinessLogic.GetResourcesCountAsync(resourceFilter);
            ResourceFilter sortResourceFilter = resourceFilter;
            sortResourceFilter.IsOrder = true;
            sortResourceFilter.OrderByField = luisInput.OrderByField;
            sortResourceFilter.OrderBy = luisInput.OrderBy;
            var ApplyPaginationTask = topicsResourcesBusinessLogic.ApplyPaginationAsync(sortResourceFilter);
            //To get guided assistant id
            resourceFilter.ResourceType = Constants.GuidedAssistant;
            var GetGuidedAssistantId = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
            await Task.WhenAll(GetResourcesTask, ApplyPaginationTask, GetGuidedAssistantId);

            var groupedResourceType = GetResourcesTask.Result;
            PagedResources resources = ApplyPaginationTask.Result;
            PagedResources guidedAssistantResponse = GetGuidedAssistantId.Result;
            var guidedAssistantResult = guidedAssistantResponse != null ? JsonUtilities.DeserializeDynamicObject<GuidedAssistant>(guidedAssistantResponse.Results.FirstOrDefault()) : null;

            dynamic searchFilter = new JObject();
            searchFilter.OrderByField = resourceFilter.OrderByField;
            searchFilter.OrderBy = resourceFilter.OrderBy;
            return new LuisViewModel
            {
                TopIntent = keyword,
                RelevantIntents = relevantIntents != null ? JsonUtilities.DeserializeDynamicObject<dynamic>(relevantIntents) : JsonConvert.DeserializeObject(Constants.EmptyArray),
                Topics = topics != null ? JsonUtilities.DeserializeDynamicObject<dynamic>(topics) : JsonConvert.DeserializeObject(Constants.EmptyArray),
                Resources = resources != null ? JsonUtilities.DeserializeDynamicObject<dynamic>(resources.Results) : JsonConvert.DeserializeObject(Constants.EmptyArray),
                ContinuationToken = resources != null && resources.ContinuationToken != null ? JsonConvert.DeserializeObject(resources.ContinuationToken) : JsonConvert.DeserializeObject(Constants.EmptyArray),
                TopicIds = topicIds != null ? JsonUtilities.DeserializeDynamicObject<dynamic>(topicIds) : JsonConvert.DeserializeObject(Constants.EmptyArray),
                ResourceTypeFilter = groupedResourceType != null ? JsonUtilities.DeserializeDynamicObject<dynamic>(groupedResourceType) : JsonConvert.DeserializeObject(Constants.EmptyArray),
                GuidedAssistantId = guidedAssistantResult != null ? guidedAssistantResult.CuratedExperienceId : string.Empty,
                SearchFilter = searchFilter
            };
        }

        public async Task<dynamic> GetWebResourcesAsync(string searchTerm)
        {
            var uri = string.Format(CultureInfo.InvariantCulture, bingSettings.BingSearchUrl.OriginalString, searchTerm, bingSettings.CustomConfigId, bingSettings.PageResultsCount, bingSettings.PageOffsetValue);
            var response = await webSearchBusinessLogic.SearchWebResourcesAsync(new Uri(uri));
            JObject webResources = new JObject
            {
                { "webResources" , JsonConvert.DeserializeObject(response) }
            };
            return webResources.ToString();
        }
    }
}