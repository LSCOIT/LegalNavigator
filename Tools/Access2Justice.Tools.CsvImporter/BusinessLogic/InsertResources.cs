using Access2Justice.Tools.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Access2Justice.Tools.BusinessLogic
{
    class InsertResources
    {
        public dynamic CreateJsonFromCSV()
        {
            //string path = Path.Combine(Environment.CurrentDirectory, "SampleFiles\\Resource_Data_tab.txt");
            string pathForm = @"C:\\Users\\v-sobhad\\Desktop\\EvolveDataTool\\Form_Data_tab.txt";
            string pathOrganization = @"C:\\Users\\v-sobhad\\Desktop\\EvolveDataTool\\Organization_Data_tab.txt";
            string pathArticle = @"C:\\Users\\v-sobhad\\Desktop\\EvolveDataTool\\Article_Data_tab.txt";
            string pathVideo = @"C:\\Users\\v-sobhad\\Desktop\\EvolveDataTool\\Video_Data_tab.txt";
            string pathRelatedLink = @"C:\\Users\\v-sobhad\\Desktop\\EvolveDataTool\\RelatedLink_Data_tab.txt";
            string[] pathResources = { pathForm, pathOrganization , pathArticle, pathVideo, pathRelatedLink };
            Resource resource = new Resource();
            List<dynamic> ResourcesList = new List<dynamic>();
            List<dynamic> Resources = new List<dynamic>();
            foreach (var path in pathResources)
            {
                string textFilePath = path;
                const Int32 BufferSize = 128;
                int recordNumber = 1;
                try
                {
                    int lineCount = File.ReadLines(path).Count();
                    using (var fileStream = File.OpenRead(textFilePath))
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        String line1, line2;
                        line1 = streamReader.ReadLine();
                        string[] parts = line1.Split('\t');
                        string val;
                        if (path.Contains("Form"))
                        {
                            Form form = new Form();
                            if (ValidateHeader(parts, 0, "Forms") && (recordNumber < lineCount))
                            {
                                while ((line2 = streamReader.ReadLine()) != null)
                                {
                                    List<string> value = new List<string>();
                                    string[] partsb = line2.Split('\t');
                                    List<TopicTag> topicTagIds = new List<TopicTag>();
                                    List<Locations> locations = new List<Locations>();
                                    dynamic id = null; string name = string.Empty; string type = string.Empty; string description = string.Empty; string url = string.Empty;
                                    string resourceType = string.Empty; string state = string.Empty; string county = string.Empty; string city = string.Empty; string zipcode = string.Empty;
                                    string overview = string.Empty; string icon = string.Empty;
                                    for (int iterationCounter = 0; iterationCounter < partsb.Length; iterationCounter++)
                                    {
                                        val = parts[iterationCounter];
                                        if (val.EndsWith("Id", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            id = (partsb[0]).Trim();
                                        }

                                        else if (val.EndsWith("Name*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            name = (partsb[1]).Trim();
                                        }

                                        else if (val.Equals("Type*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            type = (partsb[2]).Trim();
                                        }

                                        else if (val.EndsWith("Description*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            description = (partsb[3]).Trim();
                                        }

                                        else if (val.Equals("Resource Type*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            resourceType = (partsb[4]).Trim();
                                        }

                                        else if (val.EndsWith("URL*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            url = (partsb[5]).Trim();
                                        }

                                        else if (val.EndsWith("Topic*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            string topicTag = (partsb[6]).Trim();
                                            topicTagIds = GetTopicTags(topicTag);
                                        }

                                        else if (val.EndsWith("Location_State*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            state = (partsb[7]).Trim();
                                        }

                                        else if (val.EndsWith("Location_County", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            county = (partsb[8]).Trim();
                                        }

                                        else if (val.EndsWith("Location_City", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            city = (partsb[9]).Trim();
                                        }

                                        else if (val.EndsWith("Location_Zip", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            zipcode = (partsb[10]).Trim();
                                        }

                                        else if (val.EndsWith("Icon", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            icon = (partsb[11]).Trim();
                                        }

                                        else if (val.EndsWith("Overview", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            overview = (partsb[12]).Trim();
                                        }
                                    }

                                    InsertTopics topic = new InsertTopics();
                                    locations = topic.GetLocations(state, county, city, zipcode);
                                    form = new Form()
                                    {
                                        ResourceId = id == "" ? Guid.NewGuid() : id,
                                        Name = name,
                                        Type = type,
                                        Description = description,
                                        ResourceType = resourceType,
                                        Urls = url,
                                        TopicTags = topicTagIds,
                                        Location = locations,
                                        Icon = icon,
                                        Overview = overview,
                                        CreatedBy = "Admin Import tool",
                                        ModifiedBy = "Admin Import tool"
                                    };
                                    form.Validate();
                                    ResourcesList.Add(form);
                                    recordNumber++;
                                }
                            }
                        }
                        else if (path.Contains("Organization"))
                        {                            
                            Organization organization = new Organization();
                            if (ValidateHeader(parts, 0, "Organizations") && (recordNumber < lineCount))
                            {
                                while ((line2 = Regex.Replace(streamReader.ReadLine(), @"(\r\n|\n)", String.Empty)) != null)
                                {
                                    List<string> value = new List<string>();
                                    string[] partsb = line2.Split('\t');
                                    List<TopicTag> topicTagIds = new List<TopicTag>();
                                    List<Locations> locations = new List<Locations>();
                                    dynamic id = null; string name = string.Empty; string type = string.Empty; string description = string.Empty; string url = string.Empty;
                                    string resourceType = string.Empty; string state = string.Empty; string county = string.Empty; string city = string.Empty; string zipcode = string.Empty;
                                    string overview = string.Empty; string icon = string.Empty; string address = string.Empty; string telephone = string.Empty; string eligibilityInformation = string.Empty;
                                    string reviewedByCommunityMember = string.Empty; string reviewerFullName = string.Empty; string reviewerTitle = string.Empty; string reviewerImage = string.Empty;
                                    for (int iterationCounter = 0; iterationCounter < partsb.Length; iterationCounter++)
                                    {
                                        val = parts[iterationCounter];
                                        if (val.EndsWith("Id", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            id = (partsb[0]).Trim();
                                        }

                                        else if (val.EndsWith("Name*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            name = (partsb[1]).Trim();
                                        }

                                        else if (val.Equals("Type*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            type = (partsb[2]).Trim();
                                        }

                                        else if (val.EndsWith("Description*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            description = (partsb[3]).Trim();
                                        }

                                        else if (val.Equals("Resource Type*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            resourceType = (partsb[4]).Trim();
                                        }

                                        else if (val.EndsWith("URL*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            url = (partsb[5]).Trim();
                                        }

                                        else if (val.EndsWith("Topic*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            string topicTag = (partsb[6]).Trim();
                                            topicTagIds = GetTopicTags(topicTag);
                                        }

                                        else if (val.EndsWith("Location_State*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            state = (partsb[7]).Trim();
                                        }

                                        else if (val.EndsWith("Location_County", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            county = (partsb[8]).Trim();
                                        }

                                        else if (val.EndsWith("Location_City", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            city = (partsb[9]).Trim();
                                        }

                                        else if (val.EndsWith("Location_Zip", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            zipcode = (partsb[10]).Trim();
                                        }

                                        else if (val.EndsWith("Icon", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            icon = (partsb[11]).Trim();
                                        }

                                        else if (val.EndsWith("Org Address*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            address = (partsb[12]).Trim();
                                        }

                                        else if (val.EndsWith("Phone*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            telephone = (partsb[13]).Trim();
                                        }

                                        else if (val.EndsWith("Overview", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            overview = (partsb[14]).Trim();
                                        }

                                        else if (val.EndsWith("Eligibility Information", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            eligibilityInformation = (partsb[15]).Trim();
                                        }

                                        else if (val.EndsWith("Reviewed By Community Member", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            reviewedByCommunityMember = (partsb[16]).Trim();
                                        }

                                        else if (val.EndsWith("Reviewer Full Name", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            reviewerFullName = (partsb[17]).Trim();
                                        }

                                        else if (val.EndsWith("Reviewer Title", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            reviewerTitle = (partsb[18]).Trim();
                                        }

                                        else if (val.EndsWith("Reviewer Image", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            reviewerImage = (partsb[19]).Trim();
                                        }
                                    }

                                    InsertTopics topic = new InsertTopics();
                                    locations = topic.GetLocations(state, county, city, zipcode);
                                    organization = new Organization()
                                    {
                                        ResourceId = id == "" ? Guid.NewGuid() : id,
                                        Name = name,
                                        Type = type,
                                        Description = description,
                                        ResourceType = resourceType,
                                        Urls = url,
                                        TopicTags = topicTagIds,
                                        Location = locations,
                                        Icon = icon,
                                        Address = address,
                                        Telephone = telephone,
                                        Overview = overview,
                                        EligibilityInformation = eligibilityInformation,
                                        ReviewedByCommunityMember = reviewedByCommunityMember,
                                        ReviewerFullName = reviewerFullName,
                                        ReviewerTitle = reviewerTitle,
                                        ReviewerImage = reviewerImage,
                                        CreatedBy = "Admin Import tool",
                                        ModifiedBy = "Admin Import tool"
                                    };
                                    organization.Validate();
                                    ResourcesList.Add(organization);
                                    recordNumber++;
                                }
                            }
                        }
                        else if (path.Contains("Article"))
                        {
                            Article article = new Article();
                            if (ValidateHeader(parts, 0, "Articles") && (recordNumber < lineCount))
                            {
                                while ((line2 = streamReader.ReadLine()) != null)
                                {
                                    List<string> value = new List<string>();
                                    string[] partsb = line2.Split('\t');
                                    List<TopicTag> topicTagIds = new List<TopicTag>();
                                    List<Locations> locations = new List<Locations>();
                                    dynamic id = null; string name = string.Empty; string type = string.Empty; string description = string.Empty; string url = string.Empty;
                                    string resourceType = string.Empty; string state = string.Empty; string county = string.Empty; string city = string.Empty; string zipcode = string.Empty;
                                    string overview = string.Empty; string icon = string.Empty; string headline1 = string.Empty; string content1 = string.Empty; string headline2 = string.Empty; string content2 = string.Empty;
                                    for (int iterationCounter = 0; iterationCounter < partsb.Length; iterationCounter++)
                                    {
                                        val = parts[iterationCounter];
                                        if (val.EndsWith("Id", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            id = (partsb[0]).Trim();
                                        }

                                        else if (val.EndsWith("Name*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            name = (partsb[1]).Trim();
                                        }

                                        else if (val.Equals("Type*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            type = (partsb[2]).Trim();
                                        }

                                        else if (val.EndsWith("Description*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            description = (partsb[3]).Trim();
                                        }

                                        else if (val.Equals("Resource Type*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            resourceType = (partsb[4]).Trim();
                                        }

                                        else if (val.EndsWith("URL*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            url = (partsb[5]).Trim();
                                        }

                                        else if (val.EndsWith("Topic*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            string topicTag = (partsb[6]).Trim();
                                            topicTagIds = GetTopicTags(topicTag);
                                        }

                                        else if (val.EndsWith("Location_State*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            state = (partsb[7]).Trim();
                                        }

                                        else if (val.EndsWith("Location_County", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            county = (partsb[8]).Trim();
                                        }

                                        else if (val.EndsWith("Location_City", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            city = (partsb[9]).Trim();
                                        }

                                        else if (val.EndsWith("Location_Zip", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            zipcode = (partsb[10]).Trim();
                                        }

                                        else if (val.EndsWith("Icon", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            icon = (partsb[11]).Trim();
                                        }

                                        else if (val.EndsWith("Overview*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            overview = (partsb[12]).Trim();
                                        }

                                        else if (val.EndsWith("Headline 1 (optional)", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            headline1 = (partsb[13]).Trim();
                                        }

                                        else if (val.EndsWith("Content 1 (Optional)", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            content1 = (partsb[14]).Trim();
                                        }

                                        else if (val.EndsWith("Headline 2 (optional)", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            headline2 = (partsb[15]).Trim();
                                        }

                                        else if (val.EndsWith("Content 2 (Optional)", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            content2 = (partsb[16]).Trim();
                                        }
                                    }

                                    InsertTopics topic = new InsertTopics();
                                    locations = topic.GetLocations(state, county, city, zipcode);
                                    article = new Article()
                                    {
                                        ResourceId = id == "" ? Guid.NewGuid() : id,
                                        Name = name,
                                        Type = type,
                                        Description = description,
                                        ResourceType = resourceType,
                                        Urls = url,
                                        TopicTags = topicTagIds,
                                        Location = locations,
                                        Icon = icon,
                                        Overview = overview,
                                        HeadLine1=headline1,
                                        Content1=content1,
                                        HeadLine2=headline2,
                                        Content2=content2,
                                        CreatedBy = "Admin Import tool",
                                        ModifiedBy = "Admin Import tool"
                                    };
                                    article.Validate();
                                    ResourcesList.Add(article);
                                    recordNumber++;
                                }
                            }
                        }
                        else if (path.Contains("Video"))
                        {
                            Video video = new Video();
                            if (ValidateHeader(parts, 0, "Videos") && (recordNumber < lineCount))
                            {
                                while ((line2 = streamReader.ReadLine()) != null)
                                {
                                    List<string> value = new List<string>();
                                    string[] partsb = line2.Split('\t');
                                    List<TopicTag> topicTagIds = new List<TopicTag>();
                                    List<Locations> locations = new List<Locations>();
                                    dynamic id = null; string name = string.Empty; string type = string.Empty; string description = string.Empty; string url = string.Empty;
                                    string resourceType = string.Empty; string state = string.Empty; string county = string.Empty; string city = string.Empty; string zipcode = string.Empty;
                                    string overview = string.Empty; string icon = string.Empty;
                                    for (int iterationCounter = 0; iterationCounter < partsb.Length; iterationCounter++)
                                    {
                                        val = parts[iterationCounter];
                                        if (val.EndsWith("Id", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            id = (partsb[0]).Trim();
                                        }

                                        else if (val.EndsWith("Name*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            name = (partsb[1]).Trim();
                                        }

                                        else if (val.Equals("Type*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            type = (partsb[2]).Trim();
                                        }

                                        else if (val.EndsWith("Description*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            description = (partsb[3]).Trim();
                                        }

                                        else if (val.Equals("Resource Type*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            resourceType = (partsb[4]).Trim();
                                        }

                                        else if (val.EndsWith("URL*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            url = (partsb[5]).Trim();
                                        }

                                        else if (val.EndsWith("Topic*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            string topicTag = (partsb[6]).Trim();
                                            topicTagIds = GetTopicTags(topicTag);
                                        }

                                        else if (val.EndsWith("Location_State*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            state = (partsb[7]).Trim();
                                        }

                                        else if (val.EndsWith("Location_County", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            county = (partsb[8]).Trim();
                                        }

                                        else if (val.EndsWith("Location_City", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            city = (partsb[9]).Trim();
                                        }

                                        else if (val.EndsWith("Location_Zip", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            zipcode = (partsb[10]).Trim();
                                        }

                                        else if (val.EndsWith("Icon", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            icon = (partsb[11]).Trim();
                                        }

                                        else if (val.EndsWith("Overview", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            overview = (partsb[12]).Trim();
                                        }
                                    }

                                    InsertTopics topic = new InsertTopics();
                                    locations = topic.GetLocations(state, county, city, zipcode);
                                    video = new Video()
                                    {
                                        ResourceId = id == "" ? Guid.NewGuid() : id,
                                        Name = name,
                                        Type = type,
                                        Description = description,
                                        ResourceType = resourceType,
                                        Urls = url,
                                        TopicTags = topicTagIds,
                                        Location = locations,
                                        Icon = icon,
                                        Overview = overview,
                                        CreatedBy = "Admin Import tool",
                                        ModifiedBy = "Admin Import tool"
                                    };
                                    video.Validate();
                                    ResourcesList.Add(video);
                                    recordNumber++;
                                }
                            }
                        }
                        else if (path.Contains("RelatedLink"))
                        {
                            EssentialReading essentialReading = new EssentialReading();
                            if (ValidateHeader(parts, 0, "Related Links") && (recordNumber < lineCount))
                            {
                                while ((line2 = streamReader.ReadLine()) != null)
                                {
                                    List<string> value = new List<string>();
                                    string[] partsb = line2.Split('\t');
                                    List<TopicTag> topicTagIds = new List<TopicTag>();
                                    List<Locations> locations = new List<Locations>();
                                    dynamic id = null; string name = string.Empty; string type = string.Empty; string description = string.Empty; string url = string.Empty;
                                    string resourceType = string.Empty; string state = string.Empty; string county = string.Empty; string city = string.Empty; string zipcode = string.Empty; string icon = string.Empty;
                                    for (int iterationCounter = 0; iterationCounter < partsb.Length; iterationCounter++)
                                    {
                                        val = parts[iterationCounter];
                                        if (val.EndsWith("Id", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            id = (partsb[0]).Trim();
                                        }

                                        else if (val.EndsWith("Name*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            name = (partsb[1]).Trim();
                                        }

                                        else if (val.Equals("Type*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            type = (partsb[2]).Trim();
                                        }

                                        else if (val.EndsWith("Description*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            description = (partsb[3]).Trim();
                                        }

                                        else if (val.Equals("Resource Type*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            resourceType = (partsb[4]).Trim();
                                        }

                                        else if (val.EndsWith("URL*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            url = (partsb[5]).Trim();
                                        }

                                        else if (val.EndsWith("Topic*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            string topicTag = (partsb[6]).Trim();
                                            topicTagIds = GetTopicTags(topicTag);
                                        }

                                        else if (val.EndsWith("Location_State*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            state = (partsb[7]).Trim();
                                        }

                                        else if (val.EndsWith("Location_County", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            county = (partsb[8]).Trim();
                                        }

                                        else if (val.EndsWith("Location_City", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            city = (partsb[9]).Trim();
                                        }

                                        else if (val.EndsWith("Location_Zip", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            zipcode = (partsb[10]).Trim();
                                        }

                                        else if (val.EndsWith("Icon", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            icon = (partsb[11]).Trim();
                                        }
                                    }

                                    InsertTopics topic = new InsertTopics();
                                    locations = topic.GetLocations(state, county, city, zipcode);
                                    essentialReading = new EssentialReading()
                                    {
                                        ResourceId = id == "" ? Guid.NewGuid() : id,
                                        Name = name,
                                        Type = type,
                                        Description = description,
                                        ResourceType = resourceType,
                                        Urls = url,
                                        TopicTags = topicTagIds,
                                        Location = locations,
                                        Icon = icon,
                                        CreatedBy = "Admin Import tool",
                                        ModifiedBy = "Admin Import tool"
                                    };
                                    essentialReading.Validate();
                                    ResourcesList.Add(essentialReading);
                                    recordNumber++;
                                }
                            }
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    InsertTopics.ErrorLogging(ex, recordNumber);
                    InsertTopics.ReadError();
                    Resources = null;
                }
                Resources = ResourcesList;
            }
            return Resources;
        }

        public dynamic GetTopicTags(string tagId)
        {
            string[] topicTagsb = null;
            topicTagsb = tagId.Split('|');
            List<TopicTag> topicTagIds = new List<TopicTag>();
            for (int topicTagIterator = 0; topicTagIterator < topicTagsb.Length; topicTagIterator++)
            {
                string trimTopicTagId = (topicTagsb[topicTagIterator]).Trim();
                string topicTagGuid = string.Empty;
                if (trimTopicTagId.Length > 36)
                {
                    topicTagGuid = trimTopicTagId.Substring(trimTopicTagId.Length - 36, 36);

                    topicTagIds.Add(new TopicTag
                    {
                        TopicTags = topicTagGuid
                    });
                }
            }
            return topicTagIds;
        }

        public dynamic GetConditions(string conditionId)
        {
            Conditions[] conditions = null;
            string[] conditionsb = null;
            conditionsb = conditionId.Split('|');
            conditions = new Conditions[conditionsb.Length];
            for (int conditionIterator = 0; conditionIterator < conditionsb.Length; conditionIterator++)
            {
                conditions[conditionIterator] = new Conditions()
                {
                    //Condition = conditionsb[conditionIterator],
                };
            }
            return conditions;
        }

        public static bool ValidateHeader(string[] header, int recordNumber, string resourceType)
        {
            bool correctHeader = false;
            IStructuralEquatable actualHeader = header;
            string[] expectedFormHeader = {"Id", "Name*", "Type*", "Description*", "Resource Type*", "URL*", "Topic*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip", "Icon", "Overview" };
            string[] expectedOrganizationHeader = {"Id", "Name*", "Type*", "Description*", "Resource Type*", "URL*", "Topic*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip", "Icon", "Org Address*", "Phone*", "Overview", "Eligibility Information", "Reviewed By Community Member", "Reviewer Full Name", "Reviewer Title", "Reviewer Image" };
            string[] expectedArticleHeader = {"Id", "Name*", "Type*", "Description*", "Resource Type*", "URL*", "Topic*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip", "Icon", "Overview*", "Headline 1 (optional)", "Content 1 (Optional)", "Headline 2 (optional)", "Content 2 (Optional)" };
            string[] expectedVideoHeader = {"Id", "Name*", "Type*", "Description*", "Resource Type*", "URL*", "Topic*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip", "Icon", "Overview" };
            string[] expectedRelatedLinkHeader = {"Id", "Name*", "Type*", "Description*", "Resource Type*", "URL*", "Topic*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip", "Icon" };

            try
            {
                if (resourceType == "Forms")
                {
                    if (actualHeader.Equals(expectedFormHeader, StructuralComparisons.StructuralEqualityComparer))
                    {
                        correctHeader = true;
                    }
                    else
                    {
                        dynamic logHeader = null;
                        int count = 0;
                        foreach (var item in expectedFormHeader)
                        {
                            logHeader = logHeader + item;
                            if (count < expectedFormHeader.Count() - 1)
                            {
                                logHeader = logHeader + ", ";
                                count++;
                            }
                        }
                        throw new Exception("Expected header:" + "\n" + logHeader);
                    }
                }

                else if (resourceType == "Organizations")
                {
                    if (actualHeader.Equals(expectedOrganizationHeader, StructuralComparisons.StructuralEqualityComparer))
                    {
                        correctHeader = true;
                    }
                    else
                    {
                        dynamic logHeader = null;
                        int count = 0;
                        foreach (var item in expectedOrganizationHeader)
                        {
                            logHeader = logHeader + item;
                            if (count < expectedOrganizationHeader.Count() - 1)
                            {
                                logHeader = logHeader + ", ";
                                count++;
                            }
                        }
                        throw new Exception("Expected header:" + "\n" + logHeader);
                    }
                }

                else if (resourceType == "Articles")
                {
                    if (actualHeader.Equals(expectedArticleHeader, StructuralComparisons.StructuralEqualityComparer))
                    {
                        correctHeader = true;
                    }
                    else
                    {
                        dynamic logHeader = null;
                        int count = 0;
                        foreach (var item in expectedArticleHeader)
                        {
                            logHeader = logHeader + item;
                            if (count < expectedArticleHeader.Count() - 1)
                            {
                                logHeader = logHeader + ", ";
                                count++;
                            }
                        }
                        throw new Exception("Expected header:" + "\n" + logHeader);
                    }
                }

                else if (resourceType == "Videos")
                {
                    if (actualHeader.Equals(expectedVideoHeader, StructuralComparisons.StructuralEqualityComparer))
                    {
                        correctHeader = true;
                    }
                    else
                    {
                        dynamic logHeader = null;
                        int count = 0;
                        foreach (var item in expectedVideoHeader)
                        {
                            logHeader = logHeader + item;
                            if (count < expectedVideoHeader.Count() - 1)
                            {
                                logHeader = logHeader + ", ";
                                count++;
                            }
                        }
                        throw new Exception("Expected header:" + "\n" + logHeader);
                    }
                }

                else if (resourceType == "Related Links")
                {
                    if (actualHeader.Equals(expectedRelatedLinkHeader, StructuralComparisons.StructuralEqualityComparer))
                    {
                        correctHeader = true;
                    }
                    else
                    {
                        dynamic logHeader = null;
                        int count = 0;
                        foreach (var item in expectedRelatedLinkHeader)
                        {
                            logHeader = logHeader + item;
                            if (count < expectedRelatedLinkHeader.Count() - 1)
                            {
                                logHeader = logHeader + ", ";
                                count++;
                            }
                        }
                        throw new Exception("Expected header:" + "\n" + logHeader);
                    }
                }
            }
            catch (Exception ex)
            {
                InsertTopics.ErrorLogging(ex, recordNumber);
                InsertTopics.ReadError();
            }
            return correctHeader;
        }
    }
}