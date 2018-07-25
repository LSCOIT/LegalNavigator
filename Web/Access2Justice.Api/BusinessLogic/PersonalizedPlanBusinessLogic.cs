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
            var personalizedPlan = new PersonalizedActionPlanViewModel();
            personalizedPlan.PersonalizedPlanId = Guid.NewGuid();

            var relevantResources = new List<Guid>();
            var relevantTopics = new List<Guid>();
            var steps = new List<PlanStep>();
            var quicklinks = new List<PlanQuickLink>();
            int stepOrder = 0;
            foreach (var answer in userAnswers.Answers)
            {
                foreach (var component in curatedExperience.Components)
                {
                    var answerButton = component.Buttons?.Where(x => x.Id == answer.AnswerButtonId).FirstOrDefault();
                    if (answerButton != null)
                    {
                        if (answerButton.ResourceIds.Any())
                        {
                            relevantResources.AddRange(answerButton.ResourceIds);
                        }
                        if (answerButton.TopicIds.Any())
                        {
                            relevantTopics.AddRange(answerButton.TopicIds);
                        }
                    }

                    var answerField = component.Fields?.Where(x => x.Id == answer.AnswerButtonId).FirstOrDefault();
                    if (answerField != null)
                    {
                        if (answerField.ResourceIds.Any())
                        {
                            relevantResources.AddRange(answerField.ResourceIds);
                        }
                        if (answerField.TopicIds.Any())
                        {
                            relevantTopics.AddRange(answerField.TopicIds);
                        }
                    }
                    
                    steps.Add(new PlanStep
                    {
                        StepId = Guid.NewGuid(),
                        Title = component.Name,
                        Description = component.Text,
                        Order = stepOrder++,
                        IsComplete = false,
                        //Resources = relevantResources,
                        Type = "PersonalizedPlan"
                    });
                }
                quicklinks.Add(new PlanQuickLink
                {
                    Title = "Quick link to topic1",
                    Url = new Uri("http://localhost/topic1")
                });
                var planTopic = new PlanTopic();
                planTopic.TopicId = relevantTopics[0];
                planTopic.Steps = steps;
                planTopic.QuickLinks = quicklinks;
                personalizedPlan.Topics.Add(planTopic);
            }

            // todo: use the dyanmic queries (or maybe the methods in the TpoicsResourcesBusinessLogic) to 
            // get resources (based on relevantResources and relevantTopics lists) then add them to the plan

            // construct a plan
            //var personalizedPlan = new PersonalizedActionPlanViewModel();
            //personalizedPlan.PersonalizedPlanId = Guid.NewGuid();
            //foreach(var topic in relevantTopics)
            //{
            //    PlanTopic planTopic = new PlanTopic();
            //    planTopic.TopicId = topic;
            //    personalizedPlan.Topics.Add(planTopic);
            //}
            personalizedPlan = JsonConvert.DeserializeObject<PersonalizedActionPlanViewModel>(JsonConvert.SerializeObject(personalizedPlan));
            var res = await dbService.CreateItemAsync((personalizedPlan), dbSettings.PersonalizedActionPlanCollectionId);
            // save the newly generated plan in the Personalized plan (PersonalizedActionPlan collection)

            return personalizedPlan;
        }
    }
}