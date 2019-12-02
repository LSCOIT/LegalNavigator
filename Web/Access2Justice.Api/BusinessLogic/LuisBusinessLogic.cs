using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
                luisTopIntents = await ParseLuisIntent(luisResponse, luisInput.Location);
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

        public IntentWithScore ParseLuisIntent(string LuisResponse, Location location = null)
        {
            LuisIntent luisIntent = JsonConvert.DeserializeObject<LuisIntent>(LuisResponse);

            var topIntents = luisIntent?.Intents.Skip(1).Take(luisSettings.TopIntentsCount).Select(x => x.Intent).ToList();

            return new IntentWithScore
            {
                IsSuccessful = true,
                TopScoringIntent = luisIntent?.TopScoringIntent?.Intent,
                Score = luisIntent?.TopScoringIntent?.Score ?? 0,
                TopNIntents = topIntents
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

            var topics = await topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);

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

            ResourceFilter resourceFilter = null;
            PagedResources resources = null;
            dynamic groupedResourceType = null;
            IEnumerable<dynamic> guidedAssistantsResult = null;

            foreach (var searchLocation in LocationUtilities.GetSearchLocations(location))
            {
                const bool isNeedAllGuidAssistance = true;
                resourceFilter = new ResourceFilter
                {
                    TopicIds = topicIds,
                    PageNumber = 0,
                    ResourceType = Constants.All,
                    Location = searchLocation,
                    IsNeedAllGuideAssistance = isNeedAllGuidAssistance
                };

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

                groupedResourceType = GetResourcesTask.Result;
                resources = ApplyPaginationTask.Result;
                PagedResources guidedAssistantResponse = GetGuidedAssistantId.Result;

                guidedAssistantsResult = guidedAssistantResponse?
                                         .Results.Select(s =>
                                         {
                                             var ga = JsonUtilities.DeserializeDynamicObject<GuidedAssistant>(s);

                                             var child2Topics = topicsResourcesBusinessLogic.GetChild2TopicsAsync(ga.TopicTags[0].TopicTags.ToString());
                                             return (id: ga.CuratedExperienceId, tags: child2Topics);

                                         })
                                         .OfType<dynamic>().ToArray();
                guidedAssistantsResult = CleanGuidedAssistantsResult(guidedAssistantsResult);

                if (resources != null &&
                    resources.Results != null &&
                    resources.Results.Count() > 0)
                {
                    break;
                }
            }

            dynamic searchFilter = new JObject();
            searchFilter.OrderByField = resourceFilter.OrderByField;
            searchFilter.OrderBy = resourceFilter.OrderBy;

            var relevantTopics = new List<dynamic>();
            foreach (var intent in relevantIntents)
            {
                List<Topic> topic = await topicsResourcesBusinessLogic.GetTopicsAsync(intent, location);
                foreach (var item in topic)
                {
                    if(relevantTopics.Count > 2)
                    {
                        break;
                    }

                    List<Topic> ttt = await topicsResourcesBusinessLogic.GetResourceAsync(new TopicInput { Id = item.Id, Location = location });
                    if (ttt.Any())
                    {
                        dynamic dynamicObject = new ExpandoObject();
                        dynamicObject.Id = item.Id;
                        dynamicObject.Name = item.Name;
                        relevantTopics.Add(dynamicObject);
                    }
                }

            }
            var result = new LuisViewModel
            {
                TopIntent = keyword,
                RelevantIntents = relevantIntents != null
                    ? JsonUtilities.DeserializeDynamicObject<dynamic>(relevantTopics)
                    : JsonConvert.DeserializeObject(Constants.EmptyArray),
                Topics = JsonUtilities.DeserializeDynamicObject<dynamic>(topics),
                Resources = resources != null
                    ? JsonUtilities.DeserializeDynamicObject<dynamic>(resources.Results)
                    : JsonConvert.DeserializeObject(Constants.EmptyArray),
                ContinuationToken = resources != null && resources.ContinuationToken != null
                    ? JsonConvert.DeserializeObject(resources.ContinuationToken)
                    : JsonConvert.DeserializeObject(Constants.EmptyArray),
                TopicIds = JsonUtilities.DeserializeDynamicObject<dynamic>(topicIds),
                ResourceTypeFilter = groupedResourceType != null
                    ? JsonUtilities.DeserializeDynamicObject<dynamic>(groupedResourceType)
                    : JsonConvert.DeserializeObject(Constants.EmptyArray),
                GuidedAssistants = guidedAssistantsResult,
                SearchFilter = searchFilter
            };


            return result;

        }

        private IEnumerable<dynamic> CleanGuidedAssistantsResult(IEnumerable<object> guidedAssistantsResult)
        {
            var result = guidedAssistantsResult.Cast<dynamic>().Where(ga => ga.Item1 != null).ToList();

            return result.Count == 0 ? null : result;
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