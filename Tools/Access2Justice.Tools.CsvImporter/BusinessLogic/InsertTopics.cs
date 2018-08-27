using Access2Justice.Tools.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Access2Justice.Tools.BusinessLogic
{
    public class InsertTopics
    {
        public Topics CreateJsonFromCSV()
        {
            // path to the schema file to be converted. Look at the included 
            // sample file in this same project 'SampleFiles/Topic_Data_tab.txt'.
            string path = "C:\\Users\\v-sobhad\\Desktop\\EvolveDataTool\\Topic_Data_tab.txt";
            string textFilePath = path;
            const Int32 BufferSize = 128;
            int lineCount = File.ReadLines(path).Count();
            List<ParentTopic> parentTopics = new List<ParentTopic>();
            List<Topic> topicsList = new List<Topic>();
            Topics topics = new Topics();

            using (var fileStream = File.OpenRead(textFilePath))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line1, line2;
                line1 = streamReader.ReadLine();
                string[] parts = line1.Split('\t');
                string val;

                while ((line2 = streamReader.ReadLine()) != null)
                {
                    List<string> value = new List<string>();
                    string[] partsb = line2.Split('\t');
                    ParentTopicID[] parentTopicIds = null;                    
                    List<Locations> locations = new List<Locations>();
                    QuickLinks[] quickLinks = null;
                    dynamic id = null; string name = string.Empty; string keywords = string.Empty;
                    string state = string.Empty; string county = string.Empty; string city = string.Empty; string zipcode = string.Empty;
                    string overview = string.Empty; dynamic quickLinkURLText = null; dynamic quickLinkURLLink = null; string icon = string.Empty;
                    for (int iterationCounter = 0; iterationCounter < partsb.Length; iterationCounter++)
                    {
                        val = parts[iterationCounter];
                        if (val.EndsWith("Topic_ID*", StringComparison.CurrentCultureIgnoreCase))
                        {
                            id = partsb[0];
                        }

                        else if (val.EndsWith("Topic_Name*", StringComparison.CurrentCultureIgnoreCase))
                        {
                            name = partsb[1];
                        }

                        else if (val.EndsWith("Parent_Topic*", StringComparison.CurrentCultureIgnoreCase))
                        {
                            string parentId = partsb[2];
                            parentTopicIds = GetParentId(parentId);
                        }

                        else if (val.EndsWith("Keywords*", StringComparison.CurrentCultureIgnoreCase))
                        {
                            keywords = partsb[3];
                        }

                        else if (val.EndsWith("Location_State*", StringComparison.CurrentCultureIgnoreCase))
                        {
                            state = partsb[4];
                        }

                        else if (val.EndsWith("Location_County", StringComparison.CurrentCultureIgnoreCase))
                        {
                            county = partsb[5];
                        }

                        else if (val.EndsWith("Location_City", StringComparison.CurrentCultureIgnoreCase))
                        {
                            city = partsb[6];
                            
                        }

                        else if (val.EndsWith("Location_Zip", StringComparison.CurrentCultureIgnoreCase))
                        {
                            zipcode = partsb[7];
                        }

                        else if (val.EndsWith("Overview", StringComparison.CurrentCultureIgnoreCase))
                        {
                            overview = partsb[8];
                        }

                        else if (val.EndsWith("Quick_Links_URL_text", StringComparison.CurrentCultureIgnoreCase))
                        {
                            quickLinkURLText = partsb[9];
                        }

                        else if (val.EndsWith("Quick_Links_URL_link", StringComparison.CurrentCultureIgnoreCase))
                        {
                            quickLinkURLLink = partsb[10];
                        }
                        
                        else if (val.EndsWith("Icon", StringComparison.CurrentCultureIgnoreCase))
                        {
                            icon = partsb[11];
                        }                        
                    }
                    locations.Add(new Locations() { State=state, County=county, City = city, ZipCode=zipcode });
                    quickLinks = GetQuickLinks(quickLinkURLText, quickLinkURLLink);
                    //quickLinks.Add(new QuickLinks() { Text = quickLinkURLText, Urls = quickLinkURLLink });
                    topicsList.Add(new Topic()
                    {
                        Id = id == "" ? Guid.NewGuid() : id,
                        Name = name,
                        Overview = overview,
                        QuickLinks = quickLinks,
                        ParentTopicId = parentTopicIds,
                        ResourceType = "Topics",
                        Keywords = keywords,
                        Location = locations,
                        Icon = icon,
                        CreatedBy = "Admin Import tool",
                        ModifiedBy = "Admin Import tool"
                    });
                }
            }
            topics.TopicsList = topicsList.ToList();
            topics.ParentTopicList = parentTopics.ToList();
            return topics;
        }

        //public dynamic GetLocations(string locationId)
        //{
        //    List<Locations> locations = new List<Locations>();
        //    string[] locsb = null;
        //    locsb = locationId.Split('|');
        //    for (int locationIterator = 0; locationIterator < locsb.Length; locationIterator++)
        //    {
        //        string tempLocationsId = locsb[locationIterator];
        //        string[] locationsb = null;
        //        locationsb = tempLocationsId.Split(';');
        //        if (locationsb.Length == 4)
        //        {
        //            int position = 0;
        //            var specificLocations = new Locations();
        //            string state = string.Empty, county = string.Empty, city = string.Empty, zipCode = string.Empty;
        //            foreach (var subLocations in tempLocationsId.Split(';'))
        //            {
        //                state = position == 0 && string.IsNullOrEmpty(state) ? subLocations : state;
        //                county = position == 1 && string.IsNullOrEmpty(county) ? subLocations : county;
        //                city = position == 2 && string.IsNullOrEmpty(city) ? subLocations : city;
        //                zipCode = position == 3 && string.IsNullOrEmpty(zipCode) ? subLocations : zipCode;

        //                if (position == 3)
        //                {
        //                    specificLocations = new Locations()
        //                    {
        //                        State = state,
        //                        County = county,
        //                        City = city,
        //                        ZipCode = zipCode,
        //                    };
        //                }
        //                position++;
        //            }
        //            locations.Add(specificLocations);
        //        }

        //    }
        //    return locations;
        //}

        public dynamic GetParentId(string parentId)
        {
            ParentTopicID[] parentTopicIds = null;
            string[] parentsb = null;
            parentsb = parentId.Split('|');
            parentTopicIds = new ParentTopicID[parentsb.Length];
            for (int topicIdIterator = 0; topicIdIterator < parentsb.Length; topicIdIterator++)
            {
                parentTopicIds[topicIdIterator] = new ParentTopicID()
                {
                    ParentTopicId = (parentsb[topicIdIterator]).Trim(),
                };
            }
            return parentTopicIds;
        }

        public QuickLinks[] GetQuickLinks(string quickLinkText, string quickLinkLink)
        {
            QuickLinks[] quickLinks = null;
            string[] quickLinkTextsb = null;
            string[] quickLinkUrlsb = null;
            quickLinkTextsb = quickLinkText.Split('|');
            quickLinkUrlsb = quickLinkLink.Split('|');
            quickLinks = new QuickLinks[quickLinkTextsb.Length];
            if (quickLinkTextsb.Length == quickLinkUrlsb.Length)
            {
                for (int quickLinkIterator = 0; quickLinkIterator < quickLinkTextsb.Length; quickLinkIterator++)
                {
                    quickLinks[quickLinkIterator] = new QuickLinks()
                    {
                        Text = (quickLinkTextsb[quickLinkIterator]).Trim(),
                        Urls = (quickLinkUrlsb[quickLinkIterator]).Trim()
                    };
                }
            }
            return quickLinks;
        }
    }
}