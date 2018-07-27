using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
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
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        private readonly IDynamicQueries dynamicQueries;

        public PersonalizedPlanBusinessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService, IDynamicQueries dynamicQueries)
        {
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            this.dynamicQueries = dynamicQueries;
        }

        public async Task<PersonalizedActionPlanViewModel> GeneratePersonalizedPlan(CuratedExperience curatedExperience, Guid answersDocId)
        {
            var userAnswers = await dbService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(), dbSettings.CuratedExperienceAnswersCollectionId);
            var personalizedPlanSteps = new PersonalizedPlanSteps();
            var steps = new List<PlanStep>();
            int stepOrder = 1;

            var planSteps = new List<PersonalizedPlanStep>();
            foreach (var component in curatedExperience.Components)
            {
                var answerButton = component.Buttons?.Where(x => x.Id == component.Buttons[0].Id).FirstOrDefault();
                if (answerButton != null)
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
                    if (relevantResources.Count > 0)
                    {
                        planSteps.Add(new PersonalizedPlanStep()
                        {
                            StepId = Guid.NewGuid(),
                            Title = answerButton.Destination,
                            Description = answerButton.Destination,
                            Order = stepOrder++,
                            IsComplete = false,
                            Resources = (relevantResources.Count > 0) ? relevantResources : new List<Guid>(),
                            Topics = (relevantTopics.Count > 0) ? relevantTopics : new List<Guid>()
                        });
                    }
                }
            }

            personalizedPlanSteps.PlanSteps = planSteps;
            List<Guid> topics = new List<Guid>();
            foreach (var topic in personalizedPlanSteps.PlanSteps)
            {
                if (topic.Topics.Any() && !((topics.ToList()).Contains(topic.Topics[0]))) //To do: multiple topics cannot be mapped to same step
                {
                    topics.AddRange(topic.Topics);
                }
            }
            // todo: use the dyanmic queries (or maybe the methods in the TpoicsResourcesBusinessLogic) to 
            // get resources (based on relevantResources and relevantTopics lists) then add them to the plan

            // construct a plan
            List<PlanTopic> planTopics = new List<PlanTopic>();
            var quicklinks = new List<PlanQuickLink>();
            quicklinks.Add(new PlanQuickLink
            {
                Title = "Quick link to topic1",
                Url = new Uri("http://localhost/topic1")
            });
            foreach (var topic in topics)
            {
                List<PlanStep> PlanSteps = new List<PlanStep>();
                PlanSteps = GetPlanSteps(topic, personalizedPlanSteps.PlanSteps);
                planTopics.Add(new PlanTopic
                {
                    TopicId = topic,
                    QuickLinks = quicklinks,
                    Steps = PlanSteps,
                });
            }

            var personalizedPlan = new PersonalizedActionPlanViewModel();
            personalizedPlan.PersonalizedPlanId = Guid.NewGuid();
            personalizedPlan.Topics = planTopics;

           // save the newly generated plan in the Personalized plan (PersonalizedActionPlan collection)
            personalizedPlan = JsonConvert.DeserializeObject<PersonalizedActionPlanViewModel>(JsonConvert.SerializeObject(personalizedPlan));
            var res = await dbService.CreateItemAsync((personalizedPlan), dbSettings.PersonalizedActionPlanCollectionId);
            return personalizedPlan;
        }

        public List<PlanStep> GetPlanSteps(Guid topic, List<PersonalizedPlanStep> personalizedPlanSteps)
        {
            List<PlanStep> PlanSteps = new List<PlanStep>();
            foreach (var personalizedPlanStep in personalizedPlanSteps)
            {
                var topicId = personalizedPlanStep.Topics?.Where(x => x == personalizedPlanStep.Topics.FirstOrDefault()).FirstOrDefault(); //To do: multiple topics cannot be mapped to same step
                if (topicId == topic)
                {
                    PlanSteps.Add(new PlanStep {
                        StepId=personalizedPlanStep.StepId,
                        Type="Steps",
                        Title=personalizedPlanStep.Title,
                        Description=personalizedPlanStep.Description,
                        Order=personalizedPlanStep.Order,
                        IsComplete=personalizedPlanStep.IsComplete,
                        Resources=personalizedPlanStep.Resources
                    });
                }
            }

            return PlanSteps;
        }
    }
}