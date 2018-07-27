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
            var personalizedPlan = new PersonalizedActionPlanViewModel();
            var personalizedPlanSteps = new PersonalizedPlanSteps();

            personalizedPlan.PersonalizedPlanId = Guid.NewGuid();
            List<Guid> topics = new List<Guid>();
            var steps = new List<PlanStep>();
            var quicklinks = new List<PlanQuickLink>();
            int stepOrder = 1;
            quicklinks.Add(new PlanQuickLink
            {
                Title = "Quick link to topic1",
                Url = new Uri("http://localhost/topic1")
            });

            var planSteps = new List<PersonalizedPlanStep>();
            //foreach (var answer in userAnswers.Answers)
            //{
                var planTopic = new PlanTopic();
                foreach (var component in curatedExperience.Components)
                {
                //var answerButton = component.Buttons?.Where(x => x.Id == answer.AnswerButtonId).FirstOrDefault();
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
                        if(relevantResources.Count > 0)
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


                    //var answerField = component.Fields?.Where(x => x.Id == answer.AnswerButtonId).FirstOrDefault();
                    //if (answerField != null)
                    //{
                    //    if (answerField.ResourceIds.Any())
                    //    {
                    //        relevantResources.AddRange(answerField.ResourceIds);
                    //    }
                    //    if (answerField.TopicIds.Any())
                    //    {
                    //        relevantTopics.AddRange(answerField.TopicIds);
                    //    }
                    //}
                }
                //planTopic.Steps = steps;
                //planTopic.QuickLinks = quicklinks;
                //personalizedPlan.Topics.Add(planTopic);

                personalizedPlanSteps.PlanSteps = planSteps;
            //}

            personalizedPlanSteps.PersonalizedPlanId = Guid.NewGuid();
            personalizedPlanSteps = JsonConvert.DeserializeObject<PersonalizedPlanSteps>(JsonConvert.SerializeObject(personalizedPlanSteps));
            var res = await dbService.CreateItemAsync((personalizedPlanSteps), dbSettings.PersonalizedActionPlanCollectionId);

            // todo: use the dyanmic queries (or maybe the methods in the TpoicsResourcesBusinessLogic) to 
            // get resources (based on relevantResources and relevantTopics lists) then add them to the plan

            // construct a plan
            //var personalizedPlan = new PersonalizedActionPlanViewModel();
            //personalizedPlan.PersonalizedPlanId = Guid.NewGuid();

            //PlanStep planStep = new PlanStep()
            //{
            //    StepId = Guid.NewGuid(),
            //    Title = component.Name,
            //    Description = component.Text,
            //    Order = stepOrder++,
            //    IsComplete = false,
            //    //Resources = relevantResources,
            //    Type = "PersonalizedPlan"
            //};
            //foreach(var topic in relevantTopics)
            //{
            //    PlanTopic planTopic = new PlanTopic();
            //    planTopic.TopicId = topic;
            //    personalizedPlan.Topics.Add(planTopic);
            //}
            //personalizedPlan = JsonConvert.DeserializeObject<PersonalizedActionPlanViewModel>(JsonConvert.SerializeObject(personalizedPlan));
            //var res = await dbService.CreateItemAsync((personalizedPlan), dbSettings.PersonalizedActionPlanCollectionId);
            // save the newly generated plan in the Personalized plan (PersonalizedActionPlan collection)

            return personalizedPlan;
        }
    }
}