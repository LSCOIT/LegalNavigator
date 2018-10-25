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

		public PersonalizedPlanViewModelMapper(ICosmosDbSettings cosmosDbSettings, IDynamicQueries dynamicQueries)
		{
			this.cosmosDbSettings = cosmosDbSettings;
			this.dynamicQueries = dynamicQueries;
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

                var topic = await GetTopic(unprocessedTopic.Name);
				actionPlan.Topics.Add(new PlanTopic
				{
                    TopicId = Guid.Parse(topic.Id),
                    TopicName = topic.Name,
                    Icon = topic.Icon,
                    //QuickLinks = GetQuickLinks(topic.QuickLinks), //Topic schema has changed, quick links should be displayed based on 'Essential Readings' resourceType.
                    Steps = GetSteps(unprocessedTopic.UnprocessedSteps, resourceDetails)
				});
			}

			actionPlan.PersonalizedPlanId = Guid.NewGuid();
			actionPlan.IsShared = false;
			return actionPlan;
		}

        private async Task<Topic> GetTopic(string topicName)
        {
            try
            {
                List<dynamic> topics = null;
                topics = await dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.TopicsCollectionId, Constants.Name, topicName);
                if (topics == null || !topics.Any())
                {
                    topics = await dynamicQueries.FindItemsWhereContainsAsync(cosmosDbSettings.TopicsCollectionId, Constants.Name, topicName);
                }
                if (!topics.Any())
                {
                    throw new Exception($"No topic found with this name: {topicName}");
                }

                return JsonUtilities.DeserializeDynamicObject<Topic>(topics.FirstOrDefault()); // Todo:@Alaa we shouldn't return multiple topics, maybe return the latest one if many exist
            }
            catch
            {
                throw;
            }
        }

        private List<PlanQuickLink> GetQuickLinks(IEnumerable<QuickLinks> quickLinks)
        {
            var planQuickLinks = new List<PlanQuickLink>();
            foreach (var link in quickLinks)
            {
                Uri.TryCreate(link.Urls, UriKind.RelativeOrAbsolute, out var url);
                planQuickLinks.Add(new PlanQuickLink
                {
                    Text = link.Text,
                    Url = url
                });
            }

            return planQuickLinks;
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
					Type = "steps",
					Title = unprocessedStep.Title,
					Description = unprocessedStep.Description,
					Order = orderIndex++,
                    IsComplete = false,  // Todo:@Alaa check if step complete
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
