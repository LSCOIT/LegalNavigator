﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace Access2Justice.Api.BusinessLogic
{
    public class PdfService : IPdfService
    {
        private readonly IConverter pdfConverter;
        private readonly IStaticResourceBusinessLogic staticResources;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;
        private readonly ITemplateService templateService;

        private const string _planTemplate = "/Views/Templates/PersonalActionPlan.cshtml";
        private const string _topicTemplate = "/Views/Templates/SubtopicDetail.cshtml";
        private const string _resourceTemplate = "/Views/Templates/ResourceCardDetail.cshtml";

        public PdfService(ITemplateService templateService, IConverter pdfConverter, IStaticResourceBusinessLogic staticResources, 
            ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic)
        {
            this.templateService = templateService;
            this.pdfConverter = pdfConverter;
            this.staticResources = staticResources;
            this.topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
        }

        public async Task<Image> GetImage(string organizationalUnit)
        {
            var image = (await staticResources.RetrieveLogo(new List<string> { organizationalUnit })).FirstOrDefault();
            return image.Value;
        }

        public async Task<byte[]> PrintPlan(PersonalizedPlanViewModel personalizedPlan)
        {
            foreach (var personalizedPlanTopic in personalizedPlan.Topics)
            {
                foreach (var planStep in personalizedPlanTopic.Steps)
                {
                    planStep.Description = Regex.Replace(planStep.Description,
                        @"(&nbsp;)?((http|https|ftp)\://[a-z\.\-0-9/#_\?\+&amp;%\$\=~]+){1,1}(\s(\[word\]|\[pdf\])){1,1}",
                        " <a href='$2'>$2</a>&nbsp;$5", RegexOptions.IgnoreCase);
                }
            }

            Action<dynamic> fillViewBag = null;

            var topicIds = personalizedPlan.Topics.Select(x => x.TopicId.ToString());
            var topics = await topicsResourcesBusinessLogic.GetTopics(topicIds);
            var logos = await staticResources.RetrieveLogo(topics.Select(x => x.OrganizationalUnit));

            if (logos != null && logos.Count != 0)
            {
                var topicLogos = topics.ToDictionary(x => (string)x.Id.ToString(), x =>
                {
                    logos.TryGetValue(x.OrganizationalUnit, out var logo);
                    return logo;
                }).Where(x => x.Value != null).ToDictionary(x => x.Key, x => x.Value);
                fillViewBag = x =>
                {
                    x.Logos = topicLogos;
                    x.SingleImage = logos.Count == 1;
                };

            }

            return await getFile(personalizedPlan, _planTemplate, fillViewBag);
        }

        public async Task<byte[]> PrintTopic(TopicView topic, IEnumerable<dynamic> resources)
        {
            var printableTopic = new PrintableTopicView
            {
                Topic = topic
            };
            foreach (var resource in resources)
            {
                switch ((string) resource.ResourceType)
                {
                    case "Forms":
                        (printableTopic.Forms ?? (printableTopic.Forms = new List<Form>()))
                            .Add(JsonUtilities.DeserializeDynamicObject<Form>(resource)); break;

                    case "Action Plans":
                        (printableTopic.ActionPlans ?? (printableTopic.ActionPlans = new List<ActionPlan>()))
                            .Add(JsonUtilities.DeserializeDynamicObject<ActionPlan>(resource));
                        break;

                    case "Articles":
                        (printableTopic.Articles ?? (printableTopic.Articles = new List<Article>()))
                            .Add(JsonUtilities.DeserializeDynamicObject<Article>(resource)); break;

                    case "Videos":
                        (printableTopic.Videos ?? (printableTopic.Videos = new List<Video>()))
                            .Add(JsonUtilities.DeserializeDynamicObject<Video>(resource)); break;

                    case "Organizations":
                        (printableTopic.Organizations ?? (printableTopic.Organizations = new List<Organization>()))
                            .Add(JsonUtilities.DeserializeDynamicObject<Organization>(resource)); break;

                    case "Additional Readings":
                        (printableTopic.AdditionalReadings ?? (printableTopic.AdditionalReadings = new List<AdditionalReading>()))
                            .Add(JsonUtilities.DeserializeDynamicObject<AdditionalReading>(resource));
                        break;

                    case "Related Links":
                        (printableTopic.RelatedLinks ?? (printableTopic.RelatedLinks = new List<RelatedLink>()))
                            .Add(JsonUtilities.DeserializeDynamicObject<RelatedLink>(resource));
                        break;
                }
            }


            var action = await getViewBagInitialization(topic.OrganizationalUnit);

			return await getFile(printableTopic, _topicTemplate, action);
        }

        public async Task<byte[]> PrintResource(dynamic resource)
        {
            Resource deserializedResource;
            switch ((string) resource.ResourceType)
            {
                case "Forms":
                    deserializedResource = JsonUtilities.DeserializeDynamicObject<Form>(resource);
                    break;

                case "Action Plans":
                    deserializedResource = JsonUtilities.DeserializeDynamicObject<ActionPlan>(resource);
                    break;

                case "Articles":
                    deserializedResource = JsonUtilities.DeserializeDynamicObject<Article>(resource);
                    break;

                case "Videos":
                    deserializedResource = JsonUtilities.DeserializeDynamicObject<Video>(resource);
                    break;

                case "Organizations":
                    deserializedResource = JsonUtilities.DeserializeDynamicObject<Organization>(resource);
                    break;

                case "Additional Readings":
                    deserializedResource = JsonUtilities.DeserializeDynamicObject<AdditionalReading>(resource);
                    break;

                case "Related Links":
                    deserializedResource = JsonUtilities.DeserializeDynamicObject<RelatedLink>(resource);
                    break;
                default:
                    deserializedResource = JsonUtilities.DeserializeDynamicObject<Resource>(resource);
                    break;
			}

			var action = await getViewBagInitialization(deserializedResource.OrganizationalUnit);

			return await getFile(deserializedResource, _resourceTemplate, action);
        }

        private async Task<Action<dynamic>> getViewBagInitialization(string organizationalUnit)
        {
	        var viewBagInitialize = new List<Action<dynamic>> { x => x.Shared = new List<object>() };

	        var image = (await staticResources.RetrieveLogo(new[] { organizationalUnit }))
		        ?.FirstOrDefault();
	        if (image != null)
	        {
		        viewBagInitialize.Add(x => x.Image = image.Value.Value);
	        }

	        return x => viewBagInitialize.ForEach(y => y.Invoke(x));
		}

        private async Task<byte[]> getFile<T>(T personalizedPlan, string templateFile, Action<dynamic> initializeViewBag = null)
        {
            var content = await templateService.RenderTemplateAsync(templateFile, personalizedPlan, initializeViewBag);
            var result = pdfConverter.Convert(new HtmlToPdfDocument
            {
                Objects =
                {
                    new ObjectSettings
                    {
                        UseLocalLinks = false,
                        HtmlContent = content
                    }
                }
            });
            return result;
        }
    }
}
