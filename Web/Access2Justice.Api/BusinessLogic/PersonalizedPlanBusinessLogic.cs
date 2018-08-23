using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class PersonalizedPlanBusinessLogic : IPersonalizedPlanBusinessLogic
    {
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        private readonly IDynamicQueries dynamicQueries;

        public PersonalizedPlanBusinessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService, IDynamicQueries dynamicQueries)
        {
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            this.dynamicQueries = dynamicQueries;
        }

        public async Task<PersonalizedPlanSteps> GeneratePersonalizedPlan(CuratedExperience curatedExperience, Guid answersDocId)
        {
            var userAnswers = await dbService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(), dbSettings.CuratedExperienceAnswersCollectionId);
            CuratedExperienceAnswers curatedExperienceAnswers = new CuratedExperienceAnswers();
            curatedExperienceAnswers = userAnswers;
            var answerButtons = curatedExperienceAnswers.Answers.Select(x => x.AnswerButtonId.ToString()).ToList().Distinct();
            var planSteps = new List<PersonalizedPlanStep>();
            foreach (var component in curatedExperience.Components)
            {
                var answerButton = component.Buttons?.Where(x => x.Id == component.Buttons[0].Id).FirstOrDefault();
                if (answerButton != null && answerButtons.Contains(answerButton.Id.ToString()))
                {
                    List<Guid> relevantResources = new List<Guid>();
                    List<Guid> relevantTopics = new List<Guid>();
                    if (answerButton.ResourceIds.Any())
                    {
                        relevantResources.AddRange(answerButton.ResourceIds);
                    }
                    if (answerButton.TopicIds.Any())
                    {
                        relevantTopics.AddRange(answerButton.TopicIds);
                    }
                    int stepOrder = 1;
                    if (relevantResources.Count > 0)
                    {
                        planSteps.Add(new PersonalizedPlanStep()
                        {
                            StepId = Guid.NewGuid(),
                            Title = answerButton.StepTitle,
                            Description = answerButton.StepDescription,
                            Order = stepOrder++,
                            IsComplete = false,
                            Resources = (relevantResources.Count > 0) ? relevantResources : new List<Guid>(),
                            TopicIds = (relevantTopics.Count > 0) ? relevantTopics : new List<Guid>()
                        });
                    }
                }
            }
            var personalizedPlan = new PersonalizedPlanSteps();
            personalizedPlan = BuildPersonalizedPlan(planSteps);
            personalizedPlan = JsonUtilities.DeserializeDynamicObject<PersonalizedPlanSteps>(personalizedPlan);
            var res = await dbService.CreateItemAsync((personalizedPlan), dbSettings.PersonalizedActionPlanCollectionId);
            return personalizedPlan;
        }

        public PersonalizedPlanSteps BuildPersonalizedPlan(List<PersonalizedPlanStep> planSteps)
        {
            var personalizedPlanSteps = new PersonalizedPlanTopic
            {
                PlanSteps = planSteps
            };
            List<Guid> topics = new List<Guid>();
            foreach (var topic in personalizedPlanSteps.PlanSteps)
            {
                if (topic.TopicIds.Any() && !((topics.ToList()).Contains(topic.TopicIds[0]))) //To do: multiple topics cannot be mapped to same step
                {
                    topics.AddRange(topic.TopicIds);
                }
            }

            // construct a plan
            List<PersonalizedPlanTopic> planTopics = new List<PersonalizedPlanTopic>();
            foreach (var topic in topics)
            {
                List<PersonalizedPlanStep> PlanSteps = new List<PersonalizedPlanStep>();
                PlanSteps = GetPlanSteps(topic, personalizedPlanSteps.PlanSteps);
                planTopics.Add(new PersonalizedPlanTopic
                {
                    TopicId = topic,
                    PlanSteps = PlanSteps,
                });
            }

            var personalizedPlan = new PersonalizedPlanSteps
            {
                PersonalizedPlanId = Guid.NewGuid(),
                IsShared = false,
                Topics = planTopics
            };
            return personalizedPlan;
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
            var serializedResult = JsonConvert.SerializeObject(convObj);
            List<PersonalizedPlanSteps> listPlanSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanSteps>>(serializedResult);
            PersonalizedPlanSteps personalizedPlanSteps = new PersonalizedPlanSteps();
            foreach (PersonalizedPlanSteps planSteps in listPlanSteps)
            {
                personalizedPlanSteps.PersonalizedPlanId = planSteps.PersonalizedPlanId;
                personalizedPlanSteps.Topics = planSteps.Topics;
            }
            return personalizedPlanSteps;
        }

        public async Task<PersonalizedActionPlanViewModel> GetPlanDataAsync(string planId)
        {
            PersonalizedActionPlanViewModel personalizedPlan = new PersonalizedActionPlanViewModel();
            List<dynamic> procedureParams = new List<dynamic>() { planId };
            var planDetails = await dynamicQueries.FindItemsWhereAsync(dbSettings.PersonalizedActionPlanCollectionId, Constants.Id, planId);
            PersonalizedPlanSteps personalizedPlanSteps = new PersonalizedPlanSteps();
            personalizedPlanSteps = ConvertPersonalizedPlanSteps(planDetails);
            if (personalizedPlanSteps.Topics.Count>0)
            {
                var topicsList = personalizedPlanSteps.Topics.Select(x => x.TopicId).ToList().Distinct();
                var resourcesList = personalizedPlanSteps.Topics.Select(x => x.PlanSteps).SelectMany(v => v.Select(c => c.Resources).SelectMany(r => r)).ToList().Distinct();
                List<string> topicValues = topicsList.Select(x => x.ToString()).ToList();
                List<string> resourceValues = resourcesList.Select(x => x.ToString()).ToList();
                var topicsData = await dynamicQueries.FindItemsWhereInClauseAsync(dbSettings.TopicCollectionId, Constants.Id, topicValues);
                List<TopicDetails> topicDetails = JsonConvert.DeserializeObject<List<TopicDetails>>(JsonConvert.SerializeObject(topicsData));
                var resourceData = await dynamicQueries.FindItemsWhereInClauseAsync(dbSettings.ResourceCollectionId, Constants.Id, resourceValues);
                List<Resource> resourceDetails = JsonConvert.DeserializeObject<List<Resource>>(JsonConvert.SerializeObject(resourceData));

                List<PlanTopic> planTopics = new List<PlanTopic>();
                foreach (var item in personalizedPlanSteps.Topics)
                {
                    planTopics.Add(new PlanTopic
                    {
                        TopicId = item.TopicId,
                        TopicName = GetByTopicId(item.TopicId, topicDetails, true),
                        Steps = ConvertToPlanSteps(item.PlanSteps, resourceDetails),
                        QuickLinks = GetQuickLinksForTopic(item.TopicId, topicDetails),
                        Icon = GetByTopicId(item.TopicId, topicDetails, false)
                    });
                }
                personalizedPlan.Topics = planTopics;
            }

            personalizedPlan.PersonalizedPlanId = personalizedPlanSteps.PersonalizedPlanId;
            personalizedPlan.IsShared = personalizedPlanSteps.IsShared;
            return personalizedPlan;
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
            var planDetails = await dynamicQueries.FindItemsWhereAsync(dbSettings.PersonalizedActionPlanCollectionId, Constants.Id, planId);
            var personalizedPlan = JsonConvert.DeserializeObject<List<PersonalizedPlanSteps>>(JsonConvert.SerializeObject(planDetails));
            return personalizedPlan[0];
        }

        public async Task<PersonalizedActionPlanViewModel> UpdatePersonalizedPlan(PersonalizedPlanSteps plan)
        {
            var personalizedPlanSteps = JsonConvert.DeserializeObject<object>(JsonConvert.SerializeObject(plan));
            var result = await dbService.UpdateItemAsync(plan.PersonalizedPlanId.ToString(), personalizedPlanSteps, dbSettings.PersonalizedActionPlanCollectionId);
            var planId = JsonConvert.DeserializeObject<PersonalizedPlanSteps>(JsonConvert.SerializeObject(result)).PersonalizedPlanId;
            return await GetPlanDataAsync(planId.ToString());
        }
    }
}