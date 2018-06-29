using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Access2Justice.Api.BusinessLogic
{
    public class TopicsResourcesBusinessLogic : ITopicsResourcesBusinessLogic
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        public TopicsResourcesBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        public async Task<dynamic> GetResourcesAsync(dynamic topics)
        {
            var ids = new List<string>();
            foreach (var topic in topics)
            {
                string topicId = topic.id;
                ids.Add(topicId);
            }

            return await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourceCollectionId, Constants.TopicTags, Constants.Id, ids);
        }

        public async Task<dynamic> GetTopicsAsync(string keyword,Location location)
        {
            return await dbClient.FindItemsWhereContainsWithLocationAsync(dbSettings.TopicCollectionId, "keywords", keyword, location);
        }

        public async Task<dynamic> GetTopLevelTopicsAsync()
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, Constants.ParentTopicId, "");
        }

        public async Task<dynamic> GetSubTopicsAsync(string ParentTopicId)
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, Constants.ParentTopicId, ParentTopicId);
        }

        public async Task<dynamic> GetResourceAsync(string ParentTopicId)
        {
            return await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourceCollectionId, Constants.TopicTags, Constants.Id, ParentTopicId);
        }

        public async Task<dynamic> GetDocumentAsync(string id)
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, Constants.Id, id);
        }

        public async Task<dynamic> GetBreadcrumbDataAsync(string id)
        {
            List<dynamic> procedureParams = new List<dynamic>() { id };
            return await dbService.ExecuteStoredProcedureAsync(dbSettings.TopicCollectionId, Constants.BreadcrumbStoredProcedureName, procedureParams);
        }

        public async Task<dynamic> GetTopicDetailsAsync(string topicName)
        {
            var result = await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, Constants.Name, topicName);
            return result;
        }
                
        public async Task<dynamic> GetResourceDetailAsync(string resourceName, string resourceType)
        {
            var result = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, Constants.Name, resourceName, Constants.ResourceType, resourceType);
            return result;
        }
                
        public dynamic GetReferences(dynamic resourceObject)
        {
            List<ReferenceTag> referenceTags = new List<ReferenceTag>();
            List<Location> locations = new List<Location>();
            List<Conditions> conditions = new List<Conditions>();
            List<ParentTopicId> ParentTopicIds = new List<ParentTopicId>();
            List<dynamic> references = new List<dynamic>();
            foreach (JProperty field in resourceObject)
            {
                if (field.Name == "referenceTags")
                {
                    referenceTags = GetReferenceTags(field.Value);
                }

                else if (field.Name == "location")
                {
                    locations = GetLocations(field.Value);
                }

                else if (field.Name == "conditions")
                {
                    conditions = GetConditions(field.Value);
                }

                else if (field.Name == "parentTopicId")
                {
                    ParentTopicIds = GetParentTopicIds(field.Value);
                }
            }

            references.Add(referenceTags);
            references.Add(locations);
            references.Add(conditions);
            references.Add(ParentTopicIds);

            return references;
        }

        public dynamic GetReferenceTags(dynamic tagValues)
        {
            List<ReferenceTag> referenceTags = new List<ReferenceTag>();
            foreach (var referenceTag in tagValues)
            {
                string id = string.Empty;
                foreach (JProperty tags in referenceTag)
                {
                    if (tags.Name == "id")
                    {
                        id = tags.Value.ToString();
                    }
                }
                referenceTags.Add(new ReferenceTag { ReferenceTags = id });
            }
            return referenceTags;
        }

        public dynamic GetLocations(dynamic locationValues)
        {
            List<Location> locations = new List<Location>();
            foreach (var loc in locationValues)
            {
                string state = string.Empty, county = string.Empty, city = string.Empty, zipCode = string.Empty;
                foreach (JProperty locs in loc)
                {
                    if (locs.Name == "state")
                    {
                        state = locs.Value.ToString();
                    }
                    else if (locs.Name == "county")
                    {
                        county = locs.Value.ToString();
                    }
                    else if (locs.Name == "city")
                    {
                        city = locs.Value.ToString();
                    }
                    else if (locs.Name == "zipCode")
                    {
                        zipCode = locs.Value.ToString();
                    }
                }
                locations.Add(new Location { State = state, County = county, City = city, ZipCode = zipCode });                
            }
            return locations;
        }

        public dynamic GetConditions(dynamic conditionsValues)
        {
            List<Conditions> conditions = new List<Conditions>();
            foreach (var conditon in conditionsValues)
            {
                List<Condition> conditionData = new List<Condition>();
                string title = string.Empty, description = string.Empty;
                foreach (JProperty conditionJson in conditon)
                {
                    if (conditionJson.Name == "condition")
                    {
                        var conditionDetails = conditionJson.Value;
                        foreach (JProperty conditionDetail in conditionDetails)
                        {
                            if (conditionDetail.Name == "title")
                            {
                                title=conditionDetail.Value.ToString();
                            }
                            else if (conditionDetail.Name == "description")
                            {
                                description = conditionDetail.Value.ToString();
                            }
                        }
                        conditionData.Add(new Condition {Title=title, ConditionDescription = description });
                    }
                }
                conditions.Add(new Conditions {ConditionDetail = conditionData });
            }
            return conditions;
        }

        public dynamic GetParentTopicIds(dynamic ParentTopicIdValues)
        {
            List<ParentTopicId> ParentTopicIds = new List<ParentTopicId>();
            foreach (var parentTopic in ParentTopicIdValues)
            {
                string id = string.Empty;
                foreach (JProperty parentId in parentTopic)
                {
                    if (parentId.Name == "id")
                    {
                        id = parentId.Value.ToString();
                    }
                }
                ParentTopicIds.Add(new ParentTopicId { ParentTopicIds = id });
            }
            return ParentTopicIds;
        }
        
        public async Task<IEnumerable<object>> CreateResourcesUploadAsync(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                dynamic resources = null;
                string json = r.ReadToEnd();
                resources = await CreateResourceDocumentAsync(json);
                return resources;
            }
        }

        public async Task<IEnumerable<object>> CreateResourceDocumentAsync(dynamic resource)
        {
            List<dynamic> results = new List<dynamic>();
            List<dynamic> resources = new List<dynamic>();
            var resourceObjects = JsonConvert.DeserializeObject<List<dynamic>>(resource);
            Form forms = new Form();
            ActionPlan actionPlans = new ActionPlan();
            Article articles = new Article();
            Video videos = new Video();
            Organization organizations = new Organization();
            EssentialReading essentialReadings = new EssentialReading();

            foreach (var resourceObject in resourceObjects)
            {
                if (resourceObject.resourceType == "Forms")
                {
                    forms = CreateResourcesForms(resourceObject);
                    var serializedResult = JsonConvert.SerializeObject(forms);
                    var resourceDocument = JsonConvert.DeserializeObject<object>(serializedResult);
                    var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourceCollectionId);
                    resources.Add(result);
                }

                else if (resourceObject.resourceType == "Action Plans")
                {
                    actionPlans = CreateResourcesActionPlans(resourceObject);
                    var serializedResult = JsonConvert.SerializeObject(actionPlans);
                    var resourceDocument = JsonConvert.DeserializeObject<object>(serializedResult);
                    var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourceCollectionId);
                    resources.Add(result);
                }

                else if (resourceObject.resourceType == "Articles")
                {
                    articles = CreateResourcesArticles(resourceObject);
                    var serializedResult = JsonConvert.SerializeObject(articles);
                    var resourceDocument = JsonConvert.DeserializeObject<object>(serializedResult);
                    var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourceCollectionId);
                    resources.Add(result);
                }

                else if (resourceObject.resourceType == "Videos")
                {
                    videos = CreateResourcesVideos(resourceObject);
                    var serializedResult = JsonConvert.SerializeObject(videos);
                    var resourceDocument = JsonConvert.DeserializeObject<object>(serializedResult);
                    var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourceCollectionId);
                    resources.Add(result);
                }

                else if (resourceObject.resourceType == "Organizations")
                {
                    organizations = CreateResourcesOrganizations(resourceObject);
                    var serializedResult = JsonConvert.SerializeObject(organizations);
                    var resourceDocument = JsonConvert.DeserializeObject<object>(serializedResult);
                    var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourceCollectionId);
                    resources.Add(result);
                }

                else if (resourceObject.resourceType == "Essential Readings")
                {
                    essentialReadings = CreateResourcesEssentialReadings(resourceObject);
                    var serializedResult = JsonConvert.SerializeObject(essentialReadings);
                    var resourceDocument = JsonConvert.DeserializeObject<object>(serializedResult);
                    var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourceCollectionId);
                    resources.Add(result);
                }
            }
            return resources;
        }

        public dynamic CreateResourcesForms(dynamic resourceObject)
        {
            Form forms = new Form();
            List<ReferenceTag> referenceTags = new List<ReferenceTag>();
            List<Location> locations = new List<Location>();
            dynamic references = GetReferences(resourceObject);
            referenceTags = references[0];
            locations = references[1];

            forms = new Form()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                ExternalUrls = resourceObject.externalUrl,
                Urls = resourceObject.url,
                ReferenceTags = referenceTags,
                Location = locations,
                Icon = resourceObject.icon,
                Overview = resourceObject.overview,
                FullDescription = resourceObject.fullDescription,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy
            };
            forms.Validate();
            return forms;
        }

        public dynamic CreateResourcesActionPlans(dynamic resourceObject)
        {
            ActionPlan actionPlans = new ActionPlan();
            List<ReferenceTag> referenceTags = new List<ReferenceTag>();
            List<Location> locations = new List<Location>();
            List<Conditions> conditions = new List<Conditions>();
            dynamic references = GetReferences(resourceObject);
            referenceTags = references[0];
            locations = references[1];
            conditions = references[2];

            actionPlans = new ActionPlan()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                ExternalUrls = resourceObject.externalUrl,
                Urls = resourceObject.url,
                ReferenceTags = referenceTags,
                Location = locations,
                Icon = resourceObject.icon,
                Conditions = conditions,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy
            };
            actionPlans.Validate();
            return actionPlans;
        }

        public dynamic CreateResourcesArticles(dynamic resourceObject)
        {
            Article articles = new Article();
            List<ReferenceTag> referenceTags = new List<ReferenceTag>();
            List<Location> locations = new List<Location>();
            dynamic references = GetReferences(resourceObject);
            referenceTags = references[0];
            locations = references[1];

            articles = new Article()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                ExternalUrls = resourceObject.externalUrl,
                Urls = resourceObject.url,
                ReferenceTags = referenceTags,
                Location = locations,
                Icon = resourceObject.icon,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy,
                Overview = resourceObject.overview,
                HeadLine1 = resourceObject.headline1,
                HeadLine2 = resourceObject.headline2,
                HeadLine3 = resourceObject.headline3
            };
            articles.Validate();
            return articles;
        }

        public dynamic CreateResourcesVideos(dynamic resourceObject)
        {
            Video videos = new Video();
            List<ReferenceTag> referenceTags = new List<ReferenceTag>();
            List<Location> locations = new List<Location>();
            dynamic references = GetReferences(resourceObject);
            referenceTags = references[0];
            locations = references[1];

            videos = new Video()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                ExternalUrls = resourceObject.externalUrl,
                Urls = resourceObject.url,
                ReferenceTags = referenceTags,
                Location = locations,
                Icon = resourceObject.icon,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy,
                Overview = resourceObject.overview,
                IsRecommended = resourceObject.isRecommended,
                VideoUrls = resourceObject.videoUrl
            };
            videos.Validate();
            return videos;
        }

        public dynamic CreateResourcesOrganizations(dynamic resourceObject)
        {
            Organization organizations = new Organization();
            List<ReferenceTag> referenceTags = new List<ReferenceTag>();
            List<Location> locations = new List<Location>();
            dynamic references = GetReferences(resourceObject);
            referenceTags = references[0];
            locations = references[1];

            organizations = new Organization()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                ExternalUrls = resourceObject.externalUrl,
                Urls = resourceObject.url,
                ReferenceTags = referenceTags,
                Location = locations,
                Icon = resourceObject.icon,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy,
                Overview = resourceObject.overview,
                SubService = resourceObject.subService,
                Street = resourceObject.street,
                City = resourceObject.city,
                State = resourceObject.state,
                ZipCode = resourceObject.zipCode,
                Telephone = resourceObject.telephone,
                EligibilityInformation = resourceObject.eligibilityInformation,
                ReviewedByCommunityMember = resourceObject.reviewedByCommunityMember,
                ReviewerFullName = resourceObject.reviewerFullName,
                ReviewerTitle = resourceObject.reviewerTitle,
                ReviewerImage = resourceObject.reviewerImage
            };
            organizations.Validate();
            return organizations;
        }

        public dynamic CreateResourcesEssentialReadings(dynamic resourceObject)
        {
            EssentialReading essentialReadings = new EssentialReading();
            List<ReferenceTag> referenceTags = new List<ReferenceTag>();
            List<Location> locations = new List<Location>();
            dynamic references = GetReferences(resourceObject);
            referenceTags = references[0];
            locations = references[1];

            essentialReadings = new EssentialReading()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                ExternalUrls = resourceObject.externalUrl,
                Urls = resourceObject.url,
                ReferenceTags = referenceTags,
                Location = locations,
                Icon = resourceObject.icon,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy
            };
            essentialReadings.Validate();
            return essentialReadings;
        }

        public async Task<IEnumerable<object>> CreateTopicsUploadAsync(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                dynamic topics = null;
                string json = r.ReadToEnd();
                topics = await CreateTopicDocumentAsync(json);
                return topics;
            }
        }

        public async Task<IEnumerable<object>> CreateTopicDocumentAsync(dynamic topic)
        {
            List<dynamic> results = new List<dynamic>();
            List<dynamic> topics = new List<dynamic>();
            var topicObjects = JsonConvert.DeserializeObject<List<dynamic>>(topic);
            Topic topicdocuments = new Topic();

            foreach (var topicObject in topicObjects)
            {
                
                    topicdocuments = CreateTopics(topicObject);
                    var serializedResult = JsonConvert.SerializeObject(topicdocuments);
                    var topicDocument = JsonConvert.DeserializeObject<object>(serializedResult);
                    var result = await dbService.CreateItemAsync(topicDocument, dbSettings.TopicCollectionId);
                    topics.Add(result);                
            }
            return topics;
        }

        public dynamic CreateTopics(dynamic topicObject)
        {
            Topic topics = new Topic();
            List<ParentTopicId> parentTopicIds = new List<ParentTopicId>();
            List<Location> locations = new List<Location>();
            dynamic references = GetReferences(topicObject);
            locations = references[1];
            parentTopicIds = references[3];

            topics = new Topic()
            {
                Id = topicObject.id == "" ? Guid.NewGuid() : topicObject.id,
                Name = topicObject.name,
                ParentTopicId = parentTopicIds,
                Keywords = topicObject.keywords,
                JsonContent = topicObject.jsonContent,
                Location = locations,
                Icon = topicObject.icon,
                CreatedBy = topicObject.createdBy,
                ModifiedBy = topicObject.modifiedBy
            };
            topics.Validate();
            return topics;
        }
    }
}