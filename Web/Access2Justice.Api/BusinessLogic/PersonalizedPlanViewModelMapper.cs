using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
	public class PersonalizedPlanViewModelMapper : IPersonalizedPlanViewModelMapper
	{
		private readonly ICosmosDbSettings cosmosDbSettings;
		private readonly IDynamicQueries dynamicQueries;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;
        private readonly IBackendDatabaseService backendDatabaseService;

        public PersonalizedPlanViewModelMapper(ICosmosDbSettings cosmosDbSettings, IDynamicQueries dynamicQueries, 
            ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic, IBackendDatabaseService backendDatabaseService)
		{
			this.cosmosDbSettings = cosmosDbSettings;
			this.dynamicQueries = dynamicQueries;
            this.topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
            this.backendDatabaseService = backendDatabaseService;
        }

        public async Task<PersonalizedPlanViewModel> MapViewModel(UnprocessedPersonalizedPlan personalizedPlanStepsInScope)
		{
            var actionPlan = new PersonalizedPlanViewModel();
            
			foreach (var unprocessedTopic in personalizedPlanStepsInScope.UnprocessedTopics)
			{
                var resourceDetails = new List<Resource>();
                var resourceIDs = unprocessedTopic.UnprocessedSteps.SelectMany(x => x.ResourceIds);
                if (resourceIDs != null || resourceIDs.Any())
                {
                    var resourceValues = resourceIDs.Select(x => x.ToString()).Distinct().ToList();
                    var resourceData = await dynamicQueries.FindItemsWhereInClauseAsync(cosmosDbSettings.ResourcesCollectionId, Constants.Id, resourceValues);
                    resourceDetails = JsonUtilities.DeserializeDynamicObject<List<Resource>>(resourceData);
                }

                var topic = await topicsResourcesBusinessLogic.GetTopic(unprocessedTopic.Name);
                actionPlan.Topics.Add(new PlanTopic
				{
                    TopicId = Guid.Parse(topic.Id),
                    TopicName = topic.Name,
                    Icon = topic.Icon,
                    AdditionalReadings = await GetAdditionalReadings(topic.Id),
                    Steps = GetSteps(unprocessedTopic.UnprocessedSteps, resourceDetails)
				});
			}
            actionPlan.PersonalizedPlanId = Guid.NewGuid();
			actionPlan.IsShared = false;
			return actionPlan;
		}

        private async Task<List<AdditionalReadings>> GetAdditionalReadings(string topicId)
        {
            var additionalReadings = await backendDatabaseService.GetItemsAsync<AdditionalReading>(x => x.ResourceType == Constants.ReasourceTypes.AdditionalReadings &&
            x.TopicTags.Contains(new TopicTag { TopicTags = topicId }), cosmosDbSettings.ResourcesCollectionId);

            var additionalReadingsUrls = new List<AdditionalReadings>();
            foreach (var item in additionalReadings)
            {
                additionalReadingsUrls.Add(new AdditionalReadings
                {
                    Text = item.Name,
                    Url = item.Url,
                });
            }

            return additionalReadingsUrls;
        }

        private List<PlanStep> GetSteps(List<UnprocessedStep> unprocessedSteps, List<Resource> resourceDetails)
		{
			int orderIndex = 1;
			List<PlanStep> planSteps = new List<PlanStep>();
			foreach (var unprocessedStep in unprocessedSteps)
			{
				planSteps.Add(new PlanStep
				{
					StepId = unprocessedStep.Id,
                    Title = unprocessedStep.Title,
					Description = unprocessedStep.Description,
					Order = orderIndex++,
                    IsComplete = false,
                    Resources = GetResources(unprocessedStep.ResourceIds, resourceDetails)
				});
			}
			return planSteps;
		}

		private List<Resource> GetResources(List<Guid> resourceIds, List<Resource> resourceDetails)
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
