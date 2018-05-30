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
            Topics topics = new Topics();

            using (var fileStream = File.OpenRead(textFilePath))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line1, line2;
                line1 = streamReader.ReadLine();
                string[] parts = line1.Split(',');
                int len = parts.Length;
                string val;
                int j = 0, q = 0; //k = 0;

                while ((line2 = streamReader.ReadLine()) != null)
                {
                    List<string> value = new List<string>();
                    string[] partsb = line2.Split(',');
                    ParentTopicID[] parentTopicIds = null;
                    List<string> parent_Id = new List<string>();
                    List<Location> location = new List<Location>();
                    List<string> location_Id = new List<string>();
                    for (int i = 0; i < partsb.Length; i++)
                    {
                        val = parts[i];
                        if (val.EndsWith("TopicId"))
                        {
                            string tempParentId = partsb[i];
                            parent_Id.Add(partsb[i]);
                            string[] parentsb = null;
                            parentsb = tempParentId.Split(' ');
                            parentTopicIds = new ParentTopicID[parentsb.Length];
                            for (int m = 0; m < parentsb.Length; m++)
                            {
                                parentTopicIds[m] = new ParentTopicID()
                                {
                                    ParentTopicId = parentsb[m],
                                };
                            }
                        }

                        else if (val.EndsWith("location"))
                        {
                            string templocationId = partsb[i];
                            location_Id.Add(partsb[i]);
                            string[] locationsb = null;
                            locationsb = templocationId.Split('|');
                            location.Add(new Location() //todo location specific changes to be implemented, for now its taking format "State->County->City->ZipCode" format.
                            {
                                State = locationsb[0] ?? string.Empty,
                                County = locationsb[1] ?? string.Empty,
                                City = locationsb[2] ?? string.Empty,
                                ZipCode = locationsb[3] ?? string.Empty
                            });
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
                        Location = location,
                        Icon = value[4],
                        CreatedBy = value[5],
                        CreatedTimeStamp = value[6],
                        ModifiedBy = value[7],
                        ModifiedTimeStamp = value[8]
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

