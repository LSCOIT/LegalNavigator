using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    // Todo:@Alaa this class has many methods that will become obsolete soon, remove them after refactoring is done
    public class PersonalizedPlanBusinessLogic : IPersonalizedPlanBusinessLogic
    {
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly IDynamicQueries dynamicQueries;
        private readonly IUserProfileBusinessLogic userProfileBusinessLogic;
        private readonly IPersonalizedPlanEngine personalizedPlanEngine;
        private readonly IPersonalizedPlanViewModelMapper personalizedPlanViewModelMapper;

        public PersonalizedPlanBusinessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService,
            IDynamicQueries dynamicQueries, IUserProfileBusinessLogic userProfileBusinessLogic, IPersonalizedPlanEngine personalizedPlanEngine, IPersonalizedPlanViewModelMapper personalizedPlanViewModelMapper)
        {
            this.cosmosDbSettings = cosmosDbSettings;
            this.backendDatabaseService = backendDatabaseService;
            this.dynamicQueries = dynamicQueries;
            this.userProfileBusinessLogic = userProfileBusinessLogic;
            this.personalizedPlanEngine = personalizedPlanEngine;
            this.personalizedPlanViewModelMapper = personalizedPlanViewModelMapper;
        }

        public async Task<PersonalizedPlanViewModel> GeneratePersonalizedPlanAsync(CuratedExperience curatedExperience, Guid answersDocId)
        {
            // Todo:@Alaa do all quries return a list? create a new one maybe?
            var a2jPersonalizedPlan = await dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.A2JAuthorTemplatesCollectionId, "id",
                curatedExperience.A2jPersonalizedPlanId.ToString());
            var userAnswers = await backendDatabaseService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(),
                cosmosDbSettings.CuratedExperienceAnswersCollectionId);

            var unprocessedPlan = personalizedPlanEngine.Build((JObject)a2jPersonalizedPlan[0], userAnswers);
            return await personalizedPlanViewModelMapper.MapViewModel(unprocessedPlan);
        }

        public List<PersonalizedPlanStep> GetPlanSteps(Guid topic, List<PersonalizedPlanStep> personalizedPlanSteps)
        {
            List<PersonalizedPlanStep> PlanSteps = new List<PersonalizedPlanStep>();
            int stepOrder = 1;
            foreach (var personalizedPlanStep in personalizedPlanSteps)
            {
                var topicId = personalizedPlanStep.TopicIds?.Where(x => x == personalizedPlanStep.TopicIds.FirstOrDefault()).FirstOrDefault(); //To do: multiple topics cannot be mapped to same step
                if (topicId == topic)
                {
                    PlanSteps.Add(new PersonalizedPlanStep
                    {
                        StepId = personalizedPlanStep.StepId,
                        Title = personalizedPlanStep.Title,
                        Description = personalizedPlanStep.Description,
                        Order = stepOrder++,
                        IsComplete = personalizedPlanStep.IsComplete,
                        Resources = personalizedPlanStep.Resources
                    });
                }
            }

            return PlanSteps;
        }

        public PersonalizedPlanSteps ConvertPersonalizedPlanSteps(dynamic convObj)
        {
            List<PersonalizedPlanSteps> listPlanSteps = JsonUtilities.DeserializeDynamicObject<List<PersonalizedPlanSteps>>(convObj);
            PersonalizedPlanSteps personalizedPlanSteps = new PersonalizedPlanSteps();
            foreach (PersonalizedPlanSteps planSteps in listPlanSteps)
            {
                personalizedPlanSteps.PersonalizedPlanId = planSteps.PersonalizedPlanId;
                personalizedPlanSteps.Topics = planSteps.Topics;
            }
            return personalizedPlanSteps;
        }

        public string GetByTopicId(Guid topicId, List<TopicDetails> topicDetails, bool isTopicName)
        {
            string topicNameOrIcon = string.Empty;
            foreach (var topic in topicDetails)
            {
                if (topic.TopicId == topicId)
                {
                    if (isTopicName)
                    {
                        topicNameOrIcon = topic.TopicName;
                    }
                    else
                    {
                        topicNameOrIcon = topic.Icon;
                    }
                }
            }
            return topicNameOrIcon;
        }

        public List<PlanQuickLink> GetQuickLinksForTopic(Guid topicId, List<TopicDetails> topicDetails)
        {
            var quicklinks = new List<PlanQuickLink>();

            foreach (var topic in topicDetails)
            {
                if (topic.TopicId == topicId && topic.QuickLinks.Count > 0)
                {
                    foreach (var quickLink in topic.QuickLinks)
                    {
                        quicklinks.Add(new PlanQuickLink
                        {
                            Text = quickLink.Text,
                            Url = new Uri(quickLink.Url.ToString())
                        });
                    }
                }
            }
            return quicklinks;
        }

        public List<PlanStep> ConvertToPlanSteps(List<PersonalizedPlanStep> personalizedPlanSteps, List<Resource> resourceDetails)
        {
            List<PlanStep> planSteps = new List<PlanStep>();
            foreach (var personalizedPlanStep in personalizedPlanSteps)
            {
                planSteps.Add(new PlanStep
                {
                    StepId = personalizedPlanStep.StepId,
                    Type = "steps",
                    Title = personalizedPlanStep.Title,
                    Description = personalizedPlanStep.Description,
                    Order = personalizedPlanStep.Order,
                    IsComplete = personalizedPlanStep.IsComplete,
                    Resources = GetResources(personalizedPlanStep.Resources, resourceDetails)
                });
            }
            return planSteps;
        }

        public List<Resource> GetResources(List<Guid> resourceIds, List<Resource> resourceDetails)
        {
            List<Resource> resources = new List<Resource>();
            foreach (var resourceId in resourceIds)
            {
                foreach (var resource in resourceDetails)
                {
                    if (resource.ResourceId == resourceId.ToString())
                    {
                        resources.Add(resource);
                    }
                }
            }
            return resources;
        }

        public async Task<PersonalizedPlanSteps> GetPersonalizedPlan(string planId)
        {
            dynamic personalizedPlan = null;
            var planDetails = await dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.UserResourceCollectionId, Constants.Id, planId);
            if (planDetails != null)
            {
                personalizedPlan = JsonUtilities.DeserializeDynamicObject<List<PersonalizedPlanSteps>>(planDetails);
            }
            return personalizedPlan?[0];
        }

        public async Task<PersonalizedPlanViewModel> UpdatePersonalizedPlan(UserPersonalizedPlan userPlan)
        {
            var userPersonalizedPlan = new UserPersonalizedPlan();
            userPersonalizedPlan = JsonUtilities.DeserializeDynamicObject<UserPersonalizedPlan>(userPlan);
            string oId = userPersonalizedPlan.OId;
            string planId = string.Empty;
            if (oId != null)
            {
                planId = await UpdatePostLogInPersonalizedPlan(userPlan);
            }
            else
            {
                planId = await UpdatePreLogInPersonalizedPlan(userPlan.PersonalizedPlan);
            }
            // Todo:@Alaa reconsider using the plan generator here, it is too expensive,
            // this is the old code: return await GetPlanDataAsync(planId);
            // return GeneratePersonalizedPlanAsync()
            return null;
        }

        public async Task<string> UpdatePostLogInPersonalizedPlan(UserPersonalizedPlan userPlan)
        {
            string oId = userPlan.OId;
            string planId = string.Empty;
            dynamic userPlanDBData = null;
            var userProfile = await userProfileBusinessLogic.GetUserProfileDataAsync(oId);
            if (userProfile?.PersonalizedActionPlanId != null && userProfile?.PersonalizedActionPlanId != Guid.Empty)
            {
                if (userPlan.PersonalizedPlan.PersonalizedPlanId.ToString() == (userProfile?.PersonalizedActionPlanId.ToString()))
                {
                    userPlanDBData = await dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.UserResourceCollectionId, Constants.Id, Convert.ToString(userProfile.PersonalizedActionPlanId, CultureInfo.InvariantCulture));
                }
                else
                {
                    planId = await UpdatePreLogInPersonalizedPlan(userPlan.PersonalizedPlan);
                }
            }
            if (userPlanDBData == null || userPlanDBData?.Count == 0)
            {
                userProfile.PersonalizedActionPlanId = userPlan.PersonalizedPlan.PersonalizedPlanId;
                await backendDatabaseService.UpdateItemAsync(userProfile.Id, userProfile, cosmosDbSettings.UserProfileCollectionId);
                planId = userProfile.PersonalizedActionPlanId.ToString();
            }
            else
            {
                await backendDatabaseService.UpdateItemAsync(userPlan.PersonalizedPlan.PersonalizedPlanId.ToString(), userPlan.PersonalizedPlan, cosmosDbSettings.UserResourceCollectionId);
                planId = userProfile.PersonalizedActionPlanId.ToString();
            }
            return planId;
        }

        public async Task<string> UpdatePreLogInPersonalizedPlan(PersonalizedPlanSteps personalizedPlan)
        {
            string planId = string.Empty;
            dynamic userPlanDBData = null;
            if (personalizedPlan?.PersonalizedPlanId != null && personalizedPlan?.PersonalizedPlanId != Guid.Empty)
            {
                userPlanDBData = await dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.UserResourceCollectionId, Constants.Id, Convert.ToString(personalizedPlan.PersonalizedPlanId, CultureInfo.InvariantCulture));
            }
            if (userPlanDBData == null || userPlanDBData?.Count == 0)
            {
                await backendDatabaseService.CreateItemAsync((personalizedPlan), cosmosDbSettings.UserResourceCollectionId);
                planId = personalizedPlan.PersonalizedPlanId.ToString();
            }
            else
            {

                await backendDatabaseService.UpdateItemAsync(personalizedPlan.PersonalizedPlanId.ToString(), personalizedPlan, cosmosDbSettings.UserResourceCollectionId);
                planId = personalizedPlan.PersonalizedPlanId.ToString();
            }
            return planId;
        }

        // Todo:@Alaa remove this region after new implementation is done
        #region Old PersonalizedPlan code
        //public async Task<PersonalizedPlanSteps> GeneratePersonalizedPlan(CuratedExperience curatedExperience, Guid answersDocId)
        //{
        //    var userAnswers = await dbService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(), dbSettings.CuratedExperienceAnswersCollectionId);
        //    CuratedExperienceAnswers curatedExperienceAnswers = new CuratedExperienceAnswers();
        //    curatedExperienceAnswers = userAnswers;
        //    var answerButtons = curatedExperienceAnswers.Answers.Select(x => x.AnswerButtonId.ToString()).ToList().Distinct();
        //    var planSteps = new List<PersonalizedPlanStep>();
        //    foreach (var component in curatedExperience.Components)
        //    {
        //        var answerButton = component.Buttons?.Where(x => x.Id == component.Buttons[0].Id).FirstOrDefault();
        //        if (answerButton != null && answerButtons.Contains(answerButton.Id.ToString()))
        //        {
        //            List<Guid> relevantResources = new List<Guid>();
        //            List<Guid> relevantTopics = new List<Guid>();
        //            if (answerButton.ResourceIds.Any())
        //            {
        //                relevantResources.AddRange(answerButton.ResourceIds);
        //            }
        //            if (answerButton.TopicIds.Any())
        //            {
        //                relevantTopics.AddRange(answerButton.TopicIds);
        //            }
        //            int stepOrder = 1;
        //            if (relevantResources.Count > 0)
        //            {
        //                planSteps.Add(new PersonalizedPlanStep()
        //                {
        //                    StepId = Guid.NewGuid(),
        //                    Title = answerButton.StepTitle,
        //                    Description = answerButton.StepDescription,
        //                    Order = stepOrder++,
        //                    IsComplete = false,
        //                    Resources = (relevantResources.Count > 0) ? relevantResources : new List<Guid>(),
        //                    TopicIds = (relevantTopics.Count > 0) ? relevantTopics : new List<Guid>()
        //                });
        //            }
        //        }
        //    }
        //    var personalizedPlan = new PersonalizedPlanSteps();
        //    personalizedPlan = BuildPersonalizedPlan(planSteps);
        //    personalizedPlan = JsonUtilities.DeserializeDynamicObject<PersonalizedPlanSteps>(personalizedPlan);
        //    var res = await dbService.CreateItemAsync((personalizedPlan), dbSettings.UserResourceCollectionId);
        //    return personalizedPlan;
        //}

        //public async Task<PersonalizedActionPlanViewModel> GetPlanDataAsync(string planId)
        //{
        //    PersonalizedActionPlanViewModel personalizedPlan = new PersonalizedActionPlanViewModel();
        //    List<dynamic> procedureParams = new List<dynamic>() { planId };
        //    var planDetails = await dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.UserResourceCollectionId, Constants.Id, planId);
        //    PersonalizedPlanSteps personalizedPlanSteps = new PersonalizedPlanSteps();
        //    personalizedPlanSteps = ConvertPersonalizedPlanSteps(planDetails);
        //    if (personalizedPlanSteps.Topics.Count > 0)
        //    {
        //        var topicsList = personalizedPlanSteps.Topics.Select(x => x.TopicId).ToList().Distinct();
        //        var resourcesList = personalizedPlanSteps.Topics.Select(x => x.PlanSteps).SelectMany(v => v.Select(c => c.Resources).SelectMany(r => r)).ToList().Distinct();
        //        List<string> topicValues = topicsList.Select(x => x.ToString()).ToList();
        //        List<string> resourceValues = resourcesList.Select(x => x.ToString()).ToList();
        //        var topicsData = await dynamicQueries.FindItemsWhereInClauseAsync(cosmosDbSettings.TopicCollectionId, Constants.Id, topicValues);
        //        List<TopicDetails> topicDetails = JsonUtilities.DeserializeDynamicObject<List<TopicDetails>>(topicsData);
        //        var resourceData = await dynamicQueries.FindItemsWhereInClauseAsync(cosmosDbSettings.ResourceCollectionId, Constants.Id, resourceValues);
        //        List<Resource> resourceDetails = JsonUtilities.DeserializeDynamicObject<List<Resource>>(resourceData);

        //        List<PlanTopic> planTopics = new List<PlanTopic>();
        //        foreach (var item in personalizedPlanSteps.Topics)
        //        {
        //            planTopics.Add(new PlanTopic
        //            {
        //                TopicId = item.TopicId,
        //                TopicName = GetByTopicId(item.TopicId, topicDetails, true),
        //                Steps = ConvertToPlanSteps(item.PlanSteps, resourceDetails),
        //                QuickLinks = GetQuickLinksForTopic(item.TopicId, topicDetails),
        //                Icon = GetByTopicId(item.TopicId, topicDetails, false)
        //            });
        //        }
        //        personalizedPlan.Topics = planTopics;
        //    }

        //    personalizedPlan.PersonalizedPlanId = personalizedPlanSteps.PersonalizedPlanId;
        //    personalizedPlan.IsShared = personalizedPlanSteps.IsShared;
        //    return personalizedPlan;
        //}

        //public PersonalizedPlanSteps BuildPersonalizedPlan(List<PersonalizedPlanStep> planSteps)
        //{
        //    var personalizedPlanSteps = new PersonalizedPlanTopic
        //    {
        //        PlanSteps = planSteps
        //    };
        //    List<Guid> topics = new List<Guid>();
        //    foreach (var topic in personalizedPlanSteps.PlanSteps)
        //    {
        //        if (topic.TopicIds.Any() && !((topics.ToList()).Contains(topic.TopicIds[0]))) //To do: multiple topics cannot be mapped to same step
        //        {
        //            topics.AddRange(topic.TopicIds);
        //        }
        //    }

        //    // construct a plan
        //    List<PersonalizedPlanTopic> planTopics = new List<PersonalizedPlanTopic>();
        //    foreach (var topic in topics)
        //    {
        //        List<PersonalizedPlanStep> PlanSteps = new List<PersonalizedPlanStep>();
        //        PlanSteps = GetPlanSteps(topic, personalizedPlanSteps.PlanSteps);
        //        planTopics.Add(new PersonalizedPlanTopic
        //        {
        //            TopicId = topic,
        //            PlanSteps = PlanSteps,
        //        });
        //    }

        //    var personalizedPlan = new PersonalizedPlanSteps
        //    {
        //        PersonalizedPlanId = Guid.NewGuid(),
        //        IsShared = false,
        //        Topics = planTopics
        //    };
        //    return personalizedPlan;
        //}
        #endregion
    }
}