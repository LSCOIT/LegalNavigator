using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Extensions;
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
                luisTopIntents = ParseLuisIntent(luisResponse, luisInput.Location);
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
            if(luisViewModel is null || luisViewModel.GuidedAssistants == null || !luisViewModel.GuidedAssistants.Any())
            {
                return await GetWebResourcesAsync(encodedSentence);
            }

            return JObject.FromObject(luisViewModel).ToString();
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

            List<Topic> topics = await topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);
            List<string> topicIds = topics.Select(x => (string)x.Id).ToList();

            if (!topicIds.Any() || location == null)
            {
                return new LuisViewModel
                {
                    TopIntent = keyword
                };
            }

            ResourceFilter resourceFilter = null;

            IEnumerable<dynamic> guidedAssistantsResult = null;
            IEnumerable<dynamic> curatedExperiences = null;
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

                resourceFilter.ResourceType = Constants.GuidedAssistant;
                PagedResources guidedAssistantResponse = await topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);


                guidedAssistantsResult = guidedAssistantResponse?
                                         .Results.Select(s =>
                                         {
                                             var ga = JsonUtilities.DeserializeDynamicObject<GuidedAssistant>(s);

                                             var child2Topics = topicsResourcesBusinessLogic.GetChild2TopicsAsync(ga.TopicTags[0].TopicTags.ToString());
                                             return (id: ga.CuratedExperienceId, tags: child2Topics);
                                         })
                                         .OfType<dynamic>().ToArray();



                var curatedExperienceIds = guidedAssistantsResult.Select(x => (string)x.Item1).ToList(); ;
                //get curated Experiences

                curatedExperiences = (await topicsResourcesBusinessLogic.GetCuratedExperiences(curatedExperienceIds))
                    .Select(x => new { x.CuratedExperienceId, x.Title, x.IsExternal, x.Url });

                if (guidedAssistantsResult != null)
                {
                    break;
                }
            }

            var relevantTopics = new List<dynamic>();

            foreach (var item in topics)
            {
                //if topic has resources
                if(await topicsResourcesBusinessLogic.ExistsRelation(new TopicInput { Id = item.Id, Location = location }))
                {
                    dynamic dynamicObject = new ExpandoObject();
                    dynamicObject.Id = item.Id;
                    dynamicObject.Name = item.Name;
                    relevantTopics.Add(dynamicObject);
                }

                if(relevantTopics.Count > 2)
                {
                    break;
                }
            }


            var result = new LuisViewModel
            {
                TopIntent = keyword,
                RelevantIntents = relevantIntents != null
                    ? JsonUtilities.DeserializeDynamicObject<dynamic>(relevantTopics)
                    : JsonConvert.DeserializeObject(Constants.EmptyArray),
                GuidedAssistants = guidedAssistantsResult,
                CuratedExperiences = curatedExperiences
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