using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
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

		public PersonalizedPlanViewModelMapper(ICosmosDbSettings cosmosDbSettings, IDynamicQueries dynamicQueries)
		{
			this.cosmosDbSettings = cosmosDbSettings;
			this.dynamicQueries = dynamicQueries;
		}

		public async Task<PersonalizedPlanViewModel> MapViewModel(UnprocessedPersonalizedPlan personalizedPlanStepsInScope)
		{
            // https://github.com/Microsoft/Access2Justice/issues/567

            PersonalizedPlanViewModel actionPlan = new PersonalizedPlanViewModel();
			
			foreach (var topic in personalizedPlanStepsInScope.UnprocessedTopics)
			{
                var resourceDetails = new List<Resource>();
                var resourceIDs = topic.UnprocessedSteps.SelectMany(x => x.ResourceIds);
                if (resourceIDs != null || resourceIDs.Any())
                {
                    var resourceValues = resourceIDs.Select(x => x.ToString()).Distinct().ToList();
                    var resourceData = await dynamicQueries.FindItemsWhereInClauseAsync(cosmosDbSettings.ResourceCollectionId, Constants.Id, resourceValues);
                    resourceDetails = JsonUtilities.DeserializeDynamicObject<List<Resource>>(resourceData);
                }
				actionPlan.Topics.Add(new PlanTopic
				{
					TopicName = topic.Name,
					TopicId = topic.Id,
					Steps = GetPlanSteps(topic.UnprocessedSteps, resourceDetails)
				});
			}
			actionPlan.PersonalizedPlanId = Guid.NewGuid(); //personalizedPlanStepsInScope.Id;
			actionPlan.IsShared = false;
			return actionPlan;
		}

		public List<PlanStep> GetPlanSteps(List<UnprocessedStep> unprocessedSteps, List<Resource> resourceDetails)
		{
			int orderIndex = 1;
			List<PlanStep> planSteps = new List<PlanStep>();
			foreach (var unprocessedStep in unprocessedSteps)
			{
				planSteps.Add(new PlanStep
				{
					StepId = unprocessedStep.Id,
					Type = "steps",
					Title = unprocessedStep.Title,
					Description = unprocessedStep.Description,
					Order = orderIndex++,
					IsComplete = false, //when plan is loaded for first time, by default setting it to false
					Resources = GetResources(unprocessedStep.ResourceIds, resourceDetails)
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
