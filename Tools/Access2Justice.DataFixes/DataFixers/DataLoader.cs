using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.DataFixes.DataAccess;
using Access2Justice.DataFixes.Helpers;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;

namespace Access2Justice.DataFixes.DataFixers
{
	public class DataLoader : IssueFixerBase, IDataFixer
	{
		private readonly string _loadOption;
		protected override string IssueId => "#__getData";

		public DataLoader(string loadOption)
		{
			_loadOption = loadOption;
		}

		public async Task ApplyFixAsync(
			CosmosDbSettings cosmosDbSettings,
			CosmosDbService cosmosDbService)
		{
			//List<Dictionary<string, object>> resources =
			//	JsonHelper.Deserialize<List<Dictionary<string,object>>>(
			//		// await cosmosDbService.FindAllItemsAsync(cosmosDbSettings.ResourcesCollectionId)
			//		new List<Resource>
			//		{
			//			new Article{ResourceType = "article"},
			//			new Video {ResourceType = "video"},
			//			new Form {ResourceType = "form"},
			//			new AdditionalReading {ResourceType = "additionalReading"},
			//			new Organization {ResourceType = "organization"},
			//			new RelatedLink {ResourceType = "relatedLink"},
			//			new GuidedAssistant {ResourceType = "guidedAssistant"}
			//		}
			//);

			//var propertiesByType = new Dictionary<string, HashSet<string>>();
			//foreach (var resource in resources)
			//{
			//	resource.TryGetValue("resourceType", out var type);
			//	var key = type?.ToString() ?? string.Empty;
			//	if (!propertiesByType.TryGetValue(key, out var propertiesSet))
			//	{
			//		propertiesByType[key] = propertiesSet = new HashSet<string>();
			//	}

			//	foreach (var property in resource.Keys)
			//	{
			//		propertiesSet.Add(property);
			//	}
			//}

			//LogEntry(JsonConvert.SerializeObject(propertiesByType.SelectMany(x => x.Value.Select(y => new { resouceType = x.Key, property = y }))));
			//return;

			var objects = new List<DataViewBase>();
			switch (_loadOption)
			{
				case "ce":
					objects.AddRange(await GetCuratedExperiences(cosmosDbSettings, cosmosDbService));
					break;
				case "tr":
					objects.AddRange(await GetTopicsAndResourses(cosmosDbSettings, cosmosDbService));
					break;
			}

			LogEntry(JsonConvert.SerializeObject(objects));
		}

		async Task<List<DataViewBase>> GetTopicsAndResourses(CosmosDbSettings cosmosDbSettings,
			CosmosDbService cosmosDbService)
		{
			List<Topic> topics =
				JsonHelper.Deserialize<List<Topic>>(
					await cosmosDbService.FindAllItemsAsync(cosmosDbSettings.TopicsCollectionId));
			List<Resource> resources =
				JsonHelper.Deserialize<List<Resource>>(
					await cosmosDbService.FindAllItemsAsync(cosmosDbSettings.ResourcesCollectionId));
			return topics.Select(x => (DataViewBase)new TopicsAndResourcesDataView(x))
				.Union(resources.Select(x => (DataViewBase)new TopicsAndResourcesDataView(x))).ToList();
		}

		async Task<List<DataViewBase>> GetCuratedExperiences(CosmosDbSettings cosmosDbSettings,
			CosmosDbService cosmosDbService)
		{
			List<CuratedExperience> curatedExperiences =
				JsonHelper.Deserialize<List<CuratedExperience>>(
					await cosmosDbService.FindAllItemsAsync(cosmosDbSettings.CuratedExperiencesCollectionId));
			List<GuidedAssistant> resources =
				JsonHelper.Deserialize<List<GuidedAssistant>>(
					await cosmosDbService.QueryItemsAsync(cosmosDbSettings.ResourcesCollectionId,
						$"SELECT * FROM c WHERE c.curatedExperienceId in ({string.Join(",", curatedExperiences.Select(x => $"'{x.CuratedExperienceId.ToString()}'"))})"));
			var gaDictionary = new Dictionary<string, GuidedAssistant>(StringComparer.InvariantCultureIgnoreCase);
			foreach (var curatedExperience in resources)
			{
				gaDictionary[curatedExperience.CuratedExperienceId] = curatedExperience;
			}

			return curatedExperiences.Select(x =>
					(DataViewBase) new CuratedExperienceView(x,
						gaDictionary.TryGetValue(x.CuratedExperienceId.ToString(), out var ga) ? ga : null))
				.ToList();
		}

		private class DataViewBase
		{
			public string Id { get; set; }
			public string Name { get; set; }
			public string State { get; set; }
		}

		private class CuratedExperienceView : DataViewBase
		{
			public CuratedExperienceView(CuratedExperience curatedExperience, Resource guidedAssistant = null)
			{
				Id = curatedExperience.CuratedExperienceId.ToString();
				Name = curatedExperience.Title;
				State = guidedAssistant?.OrganizationalUnit
					?? guidedAssistant?.Location?.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.State))?.State;
			}
		}

		private class TopicsAndResourcesDataView: DataViewBase
		{
			public string NsmiCode { get; set; }
			public string Type { get; set; }
			public string ResourceType { get; set; }
			public string Keywords { get; set; }
			public string Description { get; set; }
			public string Url { get; set; }

			public TopicsAndResourcesDataView(Resource resource)
			{
				Id = resource.ResourceId;
				Name = resource.Name;
				State = resource.OrganizationalUnit;
				NsmiCode = resource.NsmiCode;
				Type = "resource";
				ResourceType = resource.ResourceType;
				Keywords = string.Empty;
				Description = resource.Description;
				Url = resource.Url;
			}

			public TopicsAndResourcesDataView(Topic topic)
			{
				Id = topic.Id;
				Name = topic.Name;
				State = topic.OrganizationalUnit;
				NsmiCode = topic.NsmiCode;
				Type = "topic";
				ResourceType = topic.ResourceType;
				Keywords = topic.Keywords;
				Description = string.Empty;
				Url = string.Empty;
			}
		}
	}
}
