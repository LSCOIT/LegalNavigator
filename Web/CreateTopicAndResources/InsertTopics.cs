using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateTopicAndResources
{
    public class InsertTopics
    {
        public Topics CreateJsonFromCSV()
        {
            string path = "C:\\Users\\v-sobhad\\Desktop\\reading-csv\\Topic_Data.csv";
            string textFilePath = path;
            const Int32 BufferSize = 128;
            int lineCount = File.ReadLines(path).Count();
            List<ParentTopic> parentTopics = new List<ParentTopic>();
            List<Topic> topicsList = new List<Topic>();
            Topics topics = new Topics();//new Topic[lineCount - 1];

            using (var fileStream = File.OpenRead(textFilePath))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line1, line2;
                line1 = streamReader.ReadLine();
                string[] parts = line1.Split(',');
                int len = parts.Length;
                string val;
                int j = 0, q = 0;

                while ((line2 = streamReader.ReadLine()) != null)
                {
                    List<string> value = new List<string>();
                    string[] partsb = line2.Split(',');
                    ParentTopicID[] parentTopicIds = null;
                    List<string> parent_Id = new List<string>();
                    List<Location> location = new List<Location>();
                    List<Location> locations = new List<Location>();
                    List<string> loc_Id = new List<string>();
                    List<string> location_Id = new List<string>();
                    for (int i = 0; i < partsb.Length; i++)
                    {
                        val = parts[i];
                        if (val.EndsWith("TopicId"))
                        {
                            string tempParentId = partsb[i];
                            parent_Id.Add(partsb[i]);
                            string[] parentsb = null;
                            parentsb = tempParentId.Split('|');
                            parentTopicIds = new ParentTopicID[parentsb.Length];
                            for (int m = 0; m < parentsb.Length; m++)
                            {
                                parentTopicIds[m] = new ParentTopicID()
                                {
                                    ParentTopicId = parentsb[m],
                                };
                            }
                            //k++;
                        }

                        else if (val.EndsWith("location"))
                        {
                            string templocId = partsb[i];
                            loc_Id.Add(partsb[i]);
                            string[] locsb = null;
                            locsb = templocId.Split('|');
                            for (int m = 0; m < locsb.Length; m++)
                            {
                                string templocationId = locsb[m];
                                location_Id.Add(locsb[m]);
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
                            value.Add(partsb[i]);
                        }
                        j++;
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
                        //CreatedTimeStamp = value[6],//DateTime.UtcNow.Date
                        ModifiedBy = value[6],
                        //ModifiedTimeStamp = value[8] //DateTime.UtcNow.Date
                    });
                    q++;
                }
            }
            topics.TopicsList = topicsList.ToList();
            topics.ParentTopicList = parentTopics.ToList();
            return topics;
        }
    }
}
