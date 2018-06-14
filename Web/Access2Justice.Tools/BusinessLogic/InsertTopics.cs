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
                    List<Location> locations = new List<Location>();
                    for (int iterationCounter = 0; iterationCounter < partsb.Length; iterationCounter++)
                    {
                        val = parts[iterationCounter];
                        if (val.EndsWith("TopicId", StringComparison.CurrentCultureIgnoreCase))
                        {
                            string tempParentId = partsb[iterationCounter];
                            string[] parentsb = null;
                            parentsb = tempParentId.Split('|');
                            parentTopicIds = new ParentTopicID[parentsb.Length];
                            for (int topicIdIterator = 0; topicIdIterator < parentsb.Length; topicIdIterator++)
                            {
                                parentTopicIds[topicIdIterator] = new ParentTopicID()
                                {
                                    ParentTopicId = parentsb[topicIdIterator],
                                };
                            }
                        }

                        else if (val.EndsWith("location", StringComparison.CurrentCultureIgnoreCase))
                        {
                            string templocId = partsb[iterationCounter];
                            string[] locsb = null;
                            locsb = templocId.Split('|');
                            for (int locationIterator = 0; locationIterator < locsb.Length; locationIterator++)
                            {
                                string templocationId = locsb[locationIterator];
                                string[] locationsb = null;
                                locationsb = templocationId.Split(';');
                                if (locationsb.Length == 4)
                                {
                                    int position = 0;
                                    var specificLocation = new Location();
                                    string state = string.Empty, county = string.Empty, city = string.Empty, zipCode = string.Empty;
                                    foreach (var subLocation in templocationId.Split(';'))
                                    {
                                        state = position == 0 && string.IsNullOrEmpty(state) ? subLocation : state;
                                        county = position == 1 && string.IsNullOrEmpty(county) ? subLocation : county;
                                        city = position == 2 && string.IsNullOrEmpty(city) ? subLocation : city;
                                        zipCode = position == 3 && string.IsNullOrEmpty(zipCode) ? subLocation : zipCode;

                                        if (position == 3)
                                        {
                                            specificLocation = new Location()
                                            {
                                                State = state,
                                                County = county,
                                                City = city,
                                                ZipCode = zipCode,
                                            };
                                        }
                                        position++;
                                    }
                                    locations.Add(specificLocation);
                                }
                            }
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
    }
}