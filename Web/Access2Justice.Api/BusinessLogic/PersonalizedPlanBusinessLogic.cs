using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
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
            var personalizedPlanSteps = new PersonalizedPlanTopic();
            var steps = new List<PlanStep>();
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

            personalizedPlanSteps.PlanSteps = planSteps;
            List<Guid> topics = new List<Guid>();
            foreach (var topic in personalizedPlanSteps.PlanSteps)
            {
                if (topic.TopicIds.Any() && !((topics.ToList()).Contains(topic.TopicIds[0]))) //To do: multiple topics cannot be mapped to same step
                {
                    topics.AddRange(topic.TopicIds);
                }
            }
            // todo: use the dyanmic queries (or maybe the methods in the TpoicsResourcesBusinessLogic) to 
            // get resources (based on relevantResources and relevantTopics lists) then add them to the plan

            // construct a plan
            List<PersonalizedPlanTopic> planTopics = new List<PersonalizedPlanTopic>();
            var quicklinks = new List<PlanQuickLink>();
            quicklinks.Add(new PlanQuickLink
            {
                Title = "Quick link to topic1",
                Url = new Uri("http://localhost/topic1")
            });
            foreach (var topic in topics)
            {
                List<PersonalizedPlanStep> PlanSteps = new List<PersonalizedPlanStep>();
                PlanSteps = GetPlanSteps(topic, personalizedPlanSteps.PlanSteps);
                planTopics.Add(new PersonalizedPlanTopic
                {
                    TopicId = topic,
                    //QuickLinks = quicklinks,
                    PlanSteps = PlanSteps,
                });
            }

            var personalizedPlan = new PersonalizedPlanSteps();
            personalizedPlan.PersonalizedPlanId = Guid.NewGuid();
            personalizedPlan.Topics = planTopics;
            
           // var sanitizedObject = personalizedPlan.Topics.Select(x => x.PlanSteps.Select(v => v.TopicIds.Count() > 0));
            
            // save the newly generated plan in the Personalized plan (PersonalizedActionPlan collection)
            personalizedPlan = JsonConvert.DeserializeObject<PersonalizedPlanSteps>(JsonConvert.SerializeObject(personalizedPlan));
            var res = await dbService.CreateItemAsync((personalizedPlan), dbSettings.PersonalizedActionPlanCollectionId);
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

        private PersonalizedPlanSteps ConvertPersonalizedPlanSteps(dynamic convObj)
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
            List<dynamic> procedureParams = new List<dynamic>() { planId };
            //var result = await dbService.ExecuteStoredProcedureAsync(dbSettings.ResourceCollectionId, Constants.PlanStoredProcedureName, procedureParams);
            var planDetails = await dynamicQueries.FindItemsWhereAsync(dbSettings.PersonalizedActionPlanCollectionId, Constants.Id, planId);
            //var planDetails = result.Response;
            //planDetails = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(planDetails));
            PersonalizedPlanSteps personalizedPlanSteps = new PersonalizedPlanSteps();
            personalizedPlanSteps = ConvertPersonalizedPlanSteps(planDetails);
            var topicsList = personalizedPlanSteps.Topics.Select(x => x.TopicId).ToList().Distinct();
            var resourcesList = personalizedPlanSteps.Topics.Select(x => x.PlanSteps).SelectMany(v => v.Select(c => c.Resources).SelectMany(r => r)).ToList().Distinct();
            List<string> topicValues = topicsList.Select(x => x.ToString()).ToList();
            List<string> resourceValues = resourcesList.Select(x => x.ToString()).ToList();
            var topicsData = await dynamicQueries.FindItemsWhereInClauseAsync(dbSettings.TopicCollectionId, Constants.Id, topicValues);
            List<TopicDetails> topicDetails = JsonConvert.DeserializeObject<List<TopicDetails>>(JsonConvert.SerializeObject(topicsData));
            var resourceData = await dynamicQueries.FindItemsWhereInClauseAsync(dbSettings.ResourceCollectionId, Constants.Id, resourceValues);
            //resourceData = JsonConvert.DeserializeObject<List<Resource>>(JsonConvert.SerializeObject(resourceData));
            List<Resource> resourceDetails = JsonConvert.DeserializeObject<List<Resource>>(JsonConvert.SerializeObject(resourceData));
            PersonalizedActionPlanViewModel personalizedPlan = new PersonalizedActionPlanViewModel();
            personalizedPlan.PersonalizedPlanId = personalizedPlanSteps.PersonalizedPlanId;

            List<PlanTopic> planTopics = new List<PlanTopic>();
            var quicklinks = new List<PlanQuickLink>();
            quicklinks.Add(new PlanQuickLink
            {
                Title = "Quick link to topic1",
                Url = new Uri("http://localhost/topic1")
            });
            foreach (var item in personalizedPlanSteps.Topics)
            {
                planTopics.Add(new PlanTopic
                {
                    TopicId = item.TopicId,
                    TopicName = GetByTopicId(item.TopicId, topicDetails, true),
                    Steps = ConvertToPlanSteps(item.PlanSteps, resourceDetails),
                    QuickLinks = quicklinks,
                    Icon = GetByTopicId(item.TopicId, topicDetails, false)
                });
            }
            personalizedPlan.Topics = planTopics;
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
    }
}