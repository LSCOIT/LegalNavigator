using Access2Justice.Tools.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Access2Justice.Tools.BusinessLogic
{
    public class InsertTopics
    {
        public dynamic CreateJsonFromCSV()
        {
            string path = "C:\\Users\\v-sobhad\\Desktop\\EvolveDataTool\\Topic_Data_tab.txt";
            string textFilePath = path;
            const Int32 BufferSize = 128;
            
            Topic topic = new Topic();
            List<dynamic> topicsList = new List<dynamic>();
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

                        if (ValidateTopicHeader(parts))
                        {
                            while ((line2 = streamReader.ReadLine()) != null)
                            {
                                List<string> value = new List<string>();
                                string[] partsb = line2.Split('\t');
                                ParentTopicID[] parentTopicIds = null;
                                List<Locations> locations = new List<Locations>();
                                QuickLinks[] quickLinks = null;
                                dynamic id = null; string name = string.Empty; string keywords = string.Empty;
                                string state = string.Empty; string county = string.Empty; string city = string.Empty; string zipcode = string.Empty;
                                string overview = string.Empty; string quickLinkURLText = string.Empty; string quickLinkURLLink = string.Empty; string icon = string.Empty;
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
                                locations.Add(new Locations() { State = state, County = county, City = city, ZipCode = zipcode });
                                quickLinks = GetQuickLinks(quickLinkURLText, quickLinkURLLink);
                                topic = new Topic()
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
                                };
                                topic.Validate();
                                topicsList.Add(topic);
                            }
                        }
                    }                    
                }

                catch (Exception ex)
                {
                    ErrorLogging(ex);
                    ReadError();
                }

            return topicsList;
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

        public static bool ValidateTopicHeader(string[] header)
        {
            bool correctHeader = false;
            IStructuralEquatable actualHeader = header;
            string[] expectedHeader = {"Topic_ID*", "Topic_Name*", "Parent_Topic*", "Keywords*", "Location_State*", "Location_County",
                "Location_City", "Location_Zip", "Overview", "Quick_Links_URL_text", "Quick_Links_URL_link", "Icon" };

            try
            {
                if (actualHeader.Equals(expectedHeader, StructuralComparisons.StructuralEqualityComparer))
                {
                    correctHeader = true;
                }
                else
                {
                    throw new Exception("Header mismatch. Please correct header errors.");
                }
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                ReadError();
            }

            return correctHeader;
        }
        
        public static void ErrorLogging(Exception ex)
        {
            string strPath = @"C:\\Users\\v-sobhad\\Desktop\\EvolveDataTool\\Error.txt";
            if (!File.Exists(strPath))
            {
                File.Create(strPath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(strPath))
            {
                sw.WriteLine("=============Error Logging ===========");
                sw.WriteLine("===========Start============= " + DateTime.Now);
                sw.WriteLine("Error Message: " + ex.Message);
                sw.WriteLine("Stack Trace: " + ex.StackTrace);
                sw.WriteLine("===========End============= " + DateTime.Now);
                sw.WriteLine();
            }
        }

        public static void ReadError()
        {
            string strPath = @"C:\\Users\\v-sobhad\\Desktop\\EvolveDataTool\\Error.txt";
            using (StreamReader sr = new StreamReader(strPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}