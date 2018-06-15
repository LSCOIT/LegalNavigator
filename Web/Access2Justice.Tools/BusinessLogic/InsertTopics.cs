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
            string path = "C:\\Users\\v-sobhad\\Desktop\\reading-csv\\Topic_Data_tab.txt";
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
                    for (int iterationCounter = 0; iterationCounter < partsb.Length; iterationCounter++)
                    {
                        val = parts[iterationCounter];
                        if (val.EndsWith("TopicId", StringComparison.CurrentCultureIgnoreCase))
                        {
                            string parentId = partsb[iterationCounter];
                            parentTopicIds = GetParentId(parentId);
                        }

                        else if (val.EndsWith("location", StringComparison.CurrentCultureIgnoreCase))
                        {
                            string locationId = partsb[iterationCounter];
                            locations = GetLocations(locationId);
                        }

                        else
                        {
                            value.Add(partsb[iterationCounter]);
                        }
                    }
                    var newTopicId = Guid.NewGuid();
                    parentTopics.Add(new ParentTopic() { DummyId = value[0], NewId = newTopicId });
                    topicsList.Add(new Topic()
                    {
                        Id = newTopicId,
                        Name = value[1],
                        ParentTopicID = parentTopicIds,
                        Keywords = value[2],
                        JsonContent = value[3],
                        Location = locations,
                        Icon = value[4],
                        CreatedBy = value[5],
                        ModifiedBy = value[6]
                    });
                }
            }
            topics.TopicsList = topicsList.ToList();
            topics.ParentTopicList = parentTopics.ToList();
            return topics;
        }

        public dynamic GetLocations(string locationId)
        {
            List<Locations> locations = new List<Locations>();
            string[] locsb = null;
            locsb = locationId.Split('|');
            for (int locationIterator = 0; locationIterator < locsb.Length; locationIterator++)
            {
                string tempLocationsId = locsb[locationIterator];
                string[] locationsb = null;
                locationsb = tempLocationsId.Split(';');
                if (locationsb.Length == 4)
                {
                    int position = 0;
                    var specificLocations = new Locations();
                    string state = string.Empty, county = string.Empty, city = string.Empty, zipCode = string.Empty;
                    foreach (var subLocations in tempLocationsId.Split(';'))
                    {
                        state = position == 0 && string.IsNullOrEmpty(state) ? subLocations : state;
                        county = position == 1 && string.IsNullOrEmpty(county) ? subLocations : county;
                        city = position == 2 && string.IsNullOrEmpty(city) ? subLocations : city;
                        zipCode = position == 3 && string.IsNullOrEmpty(zipCode) ? subLocations : zipCode;

                        if (position == 3)
                        {
                            specificLocations = new Locations()
                            {
                                State = state,
                                County = county,
                                City = city,
                                ZipCode = zipCode,
                            };
                        }
                        position++;
                    }
                    locations.Add(specificLocations);
                }

            }
            return locations;
        }

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
                    ParentTopicId = parentsb[topicIdIterator],
                };
            }
            return parentTopicIds;
        }
    }
}