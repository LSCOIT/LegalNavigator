using Access2Justice.Tools.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Access2Justice.Tools.BusinessLogic
{
    class InsertResources
    {
        public dynamic CreateJsonFromCSV()
        {
            //string path = Path.Combine(Environment.CurrentDirectory, "SampleFiles\\Resource_Data_tab.txt");
            string path = @"C:\\Users\\v-sobhad\\Desktop\\EvolveDataTool\\Form_Data_tab.txt";
            string textFilePath = path;
            const Int32 BufferSize = 128;
            int recordNumber = 1;
            Resource resource = new Resource();
            List<dynamic> ResourcesList = new List<dynamic>();
            List<dynamic> Resources = new List<dynamic>();
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
                    string tabName = "Forms";

                    if (tabName == "Forms")
                    {
                        Form form = new Form();
                        if (ValidateFormHeader(parts, 0) && (recordNumber < lineCount))
                        {
                            while ((line2 = streamReader.ReadLine()) != null)
                            {
                                List<string> value = new List<string>();
                                string[] partsb = line2.Split('\t');
                                //ParentTopicID[] ParentTopicIDs = null;
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
                    else if(tabName == "Action Plans") { }
                }
                Resources = ResourcesList;
            }
            catch (Exception ex)
            {
                InsertTopics.ErrorLogging(ex, recordNumber);
                InsertTopics.ReadError();
                Resources = null;
            }
            return Resources;
        }

        //public dynamic GetReferenceTags(string referenceId)
        //{
        //    ReferenceTag[] referenceTags = null;
        //    string[] referencesb = null;
        //    referencesb = referenceId.Split('|');
        //    referenceTags = new ReferenceTag[referencesb.Length];
        //    for (int referenceTagIterator = 0; referenceTagIterator < referencesb.Length; referenceTagIterator++)
        //    {
        //        referenceTags[referenceTagIterator] = new ReferenceTag()
        //        {
        //            ReferenceTags = referencesb[referenceTagIterator],
        //        };
        //    }
        //    return referenceTags;
        //}

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

        public static bool ValidateFormHeader(string[] header, int recordNumber)
        {
            bool correctHeader = false;
            IStructuralEquatable actualHeader = header;
            string[] expectedHeader = {"Id", "Name*", "Type*", "Description*", "Resource Type*", "URL*", "Topic*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip", "Icon", "Overview" };

            try
            {
                if (actualHeader.Equals(expectedHeader, StructuralComparisons.StructuralEqualityComparer))
                {
                    correctHeader = true;
                }
                else
                {
                    dynamic logHeader = null;
                    int count = 0;
                    foreach (var item in expectedHeader)
                    {
                        logHeader = logHeader + item;
                        if (count < expectedHeader.Count() - 1)
                        {
                            logHeader = logHeader + ", ";
                            count++;
                        }
                    }
                    throw new Exception("Expected header:" + "\n" + logHeader);
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