using Access2Justice.Shared.Models;
using Access2Justice.Shared.Models.Integration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Access2Justice.Shared.Utilities
{
    public class SharedReferences
    {
        public dynamic GetTopicTags(dynamic tagValues)
        {
            List<TopicTag> topicTags = new List<TopicTag>();
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
                topicTags.Add(new TopicTag { TopicTags = id });
            }
            return topicTags;
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
                    List<Models.Condition> conditionData = new List<Models.Condition>();
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
                                    title = conditionDetail.Value.ToString();
                                }
                                else if (conditionDetail.Name == "description")
                                {
                                    description = conditionDetail.Value.ToString();
                                }
                            }
                            conditionData.Add(new Models.Condition { Title = title, ConditionDescription = description });
                        }
                    }
                    conditions.Add(new Conditions { ConditionDetail = conditionData });
                }
            return conditions;
        }

        public dynamic GetParentTopicIds(dynamic parentTopicIdValues)
        {
            List<ParentTopicId> parentTopicIds = new List<ParentTopicId>();
            foreach (var parentTopic in parentTopicIdValues)
            {
                string id = string.Empty;
                foreach (JProperty parentId in parentTopic)
                {
                    if (parentId.Name == "id")
                    {
                        id = parentId.Value.ToString();
                    }
                }
                parentTopicIds.Add(new ParentTopicId { ParentTopicIds = id });
            }
            return parentTopicIds;
        }

        public dynamic GetReviewer(dynamic reviewerValues)
        {
            List<OrganizationReviewer> organizationReviewer = new List<OrganizationReviewer>();
            foreach (var reviewerDetails in reviewerValues)
            {
                string reviewerFullName = string.Empty, reviewerTitle = string.Empty, reviewText = string.Empty, reviewerImage = string.Empty;
                foreach (JProperty reviewer in reviewerDetails)
                {
                    if (reviewer.Name == "reviewerFullName")
                    {
                        reviewerFullName = reviewer.Value.ToString();
                    }
                    else if (reviewer.Name == "reviewerTitle")
                    {
                        reviewerTitle = reviewer.Value.ToString();
                    }
                    else if (reviewer.Name == "reviewText")
                    {
                        reviewText = reviewer.Value.ToString();
                    }
                    else if (reviewer.Name == "reviewerImage")
                    {
                        reviewerImage = reviewer.Value.ToString();
                    }
                }
                organizationReviewer.Add(new OrganizationReviewer { ReviewerFullName = reviewerFullName, ReviewerTitle = reviewerTitle, ReviewText = reviewText, ReviewerImage = reviewerImage });
            }
            return organizationReviewer;
        }

        public dynamic GetContents(dynamic contentValues)
        {
            List<ArticleContent> articleContents = new List<ArticleContent>();
            foreach (var contentDetails in contentValues)
            {
                string headline = string.Empty, content = string.Empty;
                foreach (JProperty contentData in contentDetails)
                {
                    if (contentData.Name == "headline")
                    {
                        headline = contentData.Value.ToString();
                    }
                    else if (contentData.Name == "content")
                    {
                        content = contentData.Value.ToString();
                    }
                }
                articleContents.Add(new ArticleContent { Headline = headline, Content = content });
            }
            return articleContents;
        }

        public dynamic GetAvailability(dynamic availabilityValues)
        {
            Availability availability = new Availability();
            IEnumerable<Schedule> regularBusinessHours = null;
            IEnumerable<Schedule> holidayBusinessHours = null;
            var regularBusinessHoursData = availabilityValues.regularBusinessHours;
            regularBusinessHours = GetBusinessHours(regularBusinessHoursData);
            holidayBusinessHours = GetBusinessHours(availabilityValues.holidayBusinessHours);
            TimeSpan waitTime = TimeSpan.Parse(availabilityValues.waitTime.ToString(), CultureInfo.InvariantCulture);
            availability = new Availability { RegularBusinessHours = regularBusinessHours, HolidayBusinessHours = holidayBusinessHours, WaitTime = waitTime };            
            return availability;
        }

        public dynamic GetBusinessHours(dynamic businessHours)
        {
            List<Schedule> schedules = new List<Schedule>();
            Schedule schedule = new Schedule();
            foreach(var businessHour in businessHours)
            {
                TimeSpan openTime = TimeSpan.Parse(businessHour.opensAt.ToString(), CultureInfo.InvariantCulture);
                TimeSpan closeTime = TimeSpan.Parse(businessHour.opensAt.ToString(), CultureInfo.InvariantCulture);
                schedule = (new Schedule { Day = businessHour.day, OpensAt = openTime, ClosesAt = closeTime });
                schedules.Add(schedule);
            }
            return schedules;
        }

        public dynamic GetReferences(dynamic resourceObject)
        {
            List<TopicTag> topicTags = new List<TopicTag>();
            List<Location> locations = new List<Location>();
            List<Conditions> conditions = new List<Conditions>();
            List<ParentTopicId> parentTopicIds = new List<ParentTopicId>();
            List<OrganizationReviewer> organizationReviewers = new List<OrganizationReviewer>();
            List<ArticleContent> articleContents = new List<ArticleContent>();
            Availability availability = new Availability();
            List<dynamic> references = new List<dynamic>();
            foreach (JProperty field in resourceObject)
            {
                if (field.Name == "topicTags")
                {
                    topicTags = field.Value != null && field.Value.Count() > 0 ? GetTopicTags(field.Value) : null;
                }

                else if (field.Name == "location")
                {
                    locations = GetLocations(field.Value);
                }

                else if (field.Name == "conditions")
                {
                    conditions = field.Value != null && field.Value.Count() > 0 ? GetConditions(field.Value) : null;
                }

                else if (field.Name == "parentTopicId")
                {
                    parentTopicIds = field.Value != null && field.Value.Count() > 0 ? GetParentTopicIds(field.Value) : null;
                }

                else if (field.Name == "reviewer")
                {
                    organizationReviewers = field.Value != null && field.Value.Count() > 0 ? GetReviewer(field.Value) : null;
                }

                else if (field.Name == "contents")
                {
                    articleContents = field.Value != null && field.Value.Count() > 0 ? GetContents(field.Value) : null;
                }

                else if (field.Name == "availability")
                {
                    availability = field.Value != null && field.Value.Count() > 0 ? GetAvailability(field.Value) : null;
                }
            }

            references.Add(topicTags);
            references.Add(locations);
            references.Add(conditions);
            references.Add(parentTopicIds);
            references.Add(organizationReviewers);
            references.Add(articleContents);
            references.Add(availability);
            return references;
        }
    }
}
