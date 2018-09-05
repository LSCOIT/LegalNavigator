using Access2Justice.Tools.Models;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Spreadsheet = DocumentFormat.OpenXml.Spreadsheet;
using System.Configuration;

namespace Access2Justice.Tools.BusinessLogic
{
    public class InsertTopics
    {
        public dynamic CreateJsonFromCSV()
        {
            int recordNumber = 1;
            Topic topic = new Topic();
            List<dynamic> topicsList = new List<dynamic>();
            List<dynamic> topics = new List<dynamic>();
            string appSettings = ConfigurationManager.AppSettings.Get("Topics");
            string path = Path.Combine(Environment.CurrentDirectory, appSettings);

            try
            {
                string textFilePath = path;
                using (SpreadsheetDocument spreadsheetDocument =
                    SpreadsheetDocument.Open(path, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    Spreadsheet.SharedStringTable sharedStringTable = spreadsheetDocument.WorkbookPart.SharedStringTablePart.SharedStringTable;
                    Spreadsheet.SheetData sheetData = worksheetPart.Worksheet.Elements<Spreadsheet.SheetData>().First();
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    string cellValue;
                    int counter = 0;
                    bool isValidated = false;
                    foreach (Spreadsheet.Row row in sheetData.Elements<Spreadsheet.Row>())
                    {
                        dynamic id = null; string name = string.Empty; string keywords = string.Empty;
                        string state = string.Empty; string county = string.Empty; string city = string.Empty; string zipcode = string.Empty;
                        string overview = string.Empty; string quickLinkURLText = string.Empty; string quickLinkURLLink = string.Empty; string icon = string.Empty;
                        List<ParentTopicID> parentTopicIds = new List<ParentTopicID>();
                        List<Locations> locations = new List<Locations>();
                        List<QuickLinks> quickLinks = null;

                        foreach (Spreadsheet.Cell cell in row.Elements<Spreadsheet.Cell>())
                        {
                            cellValue = cell.InnerText;
                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                string cellActualValue = string.Empty;
                                if (cell.DataType == Spreadsheet.CellValues.SharedString)
                                {
                                    cellActualValue = sharedStringTable.ElementAt(Int32.Parse(cellValue,CultureInfo.InvariantCulture)).InnerText;
                                }
                                else
                                {
                                    cellActualValue = cellValue;
                                }

                                if (counter == 0)
                                {
                                    keyValuePairs.Add(cellActualValue, cell.CellReference);
                                }
                                else
                                {
                                    var headerValues = from a in keyValuePairs select a.Key;
                                    if (!isValidated)
                                    {
                                        if (!ValidateTopicHeader(headerValues.ToArray<string>(), recordNumber))
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            isValidated = true;
                                        }
                                    }
                                    
                                    IEnumerable<string> keyValue = null;
                                    if (cell.CellReference.Value.Length == 2)
                                    {
                                        keyValue = from a in keyValuePairs where a.Value.Take(1).First() == cell.CellReference.Value.Take(1).First() select a.Key;
                                    }
                                    else if (cell.CellReference.Value.Length == 3)
                                    {
                                        keyValue = from a in keyValuePairs where a.Value.Take(2).First() == cell.CellReference.Value.Take(2).First() select a.Key;
                                    }

                                    if (keyValue.Count() > 0)
                                    {
                                        string val = keyValue.First();
                                        if (val.EndsWith("Topic_ID*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            id = cellActualValue.Trim();
                                        }

                                        else if (val.EndsWith("Topic_Name*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            name = cellActualValue.Trim();
                                        }

                                        else if (val.EndsWith("Parent_Topic*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            string parentId = cellActualValue;
                                            parentTopicIds = GetParentId(parentId);
                                        }

                                        else if (val.EndsWith("Keywords*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            keywords = (cellActualValue).Trim();
                                        }

                                        else if (val.EndsWith("Location_State*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            state = (cellActualValue).Trim();
                                        }

                                        else if (val.EndsWith("Location_County", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            county = (cellActualValue).Trim();
                                        }

                                        else if (val.EndsWith("Location_City", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            city = (cellActualValue).Trim();

                                        }

                                        else if (val.EndsWith("Location_Zip", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            zipcode = (cellActualValue).Trim();
                                        }

                                        else if (val.EndsWith("Overview", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            overview = (cellActualValue).Trim();
                                        }

                                        else if (val.EndsWith("Quick_Links_URL_text", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            quickLinkURLText = (cellActualValue).Trim();
                                        }

                                        else if (val.EndsWith("Quick_Links_URL_link", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            quickLinkURLLink = (cellActualValue).Trim();
                                        }

                                        else if (val.EndsWith("Icon", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            icon = cellActualValue;
                                        }
                                    }
                                }
                            }
                        }

                        if (counter > 0)
                        {
                            quickLinks = GetQuickLinks(quickLinkURLText, quickLinkURLLink);
                            locations = GetLocations(state, county, city, zipcode);
                            topic = new Topic()
                            {
                                Id = id == "" ? Guid.NewGuid() : id,
                                Name = name,
                                Overview = overview,
                                QuickLinks = quickLinks,
                                ParentTopicId = parentTopicIds.Count > 0 ? parentTopicIds : null,
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
                        counter++;
                        recordNumber++;
                    }
                    topics = topicsList;
                }
            }
            catch (Exception ex)
            {
                ErrorLogging(ex, recordNumber);
                ReadError();
                topics = null;
            }
            return topics;
        }

        public dynamic GetParentId(string parentId)
        {
            string[] parentsb = null;
            parentsb = parentId.Split('|');
            List<ParentTopicID> parentTopicIds = new List<ParentTopicID>();
            for (int topicIdIterator = 0; topicIdIterator < parentsb.Length; topicIdIterator++)
            {
                string trimParentTopicId = (parentsb[topicIdIterator]).Trim();
                string parentTopicGuid = string.Empty;
                if (trimParentTopicId.Length > 36)
                {
                    parentTopicGuid = trimParentTopicId.Substring(trimParentTopicId.Length - 36, 36);

                    parentTopicIds.Add(new ParentTopicID
                    {
                        ParentTopicId = parentTopicGuid
                    });
                }
            }
            return parentTopicIds;
        }

        public List<QuickLinks> GetQuickLinks(string quickLinkText, string quickLinkLink)
        {

            List<QuickLinks> quickLinks = new List<QuickLinks>();
            if (string.IsNullOrEmpty(quickLinkText) || string.IsNullOrEmpty(quickLinkLink))
            {
                quickLinks = null;
            }
            else
            {
                string[] quickLinkTextsb = null;
                string[] quickLinkUrlsb = null;
                quickLinkTextsb = quickLinkText.Split('|');
                quickLinkUrlsb = quickLinkLink.Split('|');
                if (quickLinkTextsb.Length == quickLinkUrlsb.Length)
                {
                    for (int quickLinkIterator = 0; quickLinkIterator < quickLinkTextsb.Length; quickLinkIterator++)
                    {
                        quickLinks.Add(new QuickLinks
                        {
                            Text = (quickLinkTextsb[quickLinkIterator]).Trim(),
                            Urls = (quickLinkUrlsb[quickLinkIterator]).Trim()
                        });
                    }
                }
                else if (quickLinkTextsb.Length < quickLinkUrlsb.Length)
                {
                    for (int quickLinkIterator = 0; quickLinkIterator < quickLinkTextsb.Length; quickLinkIterator++)
                    {
                        quickLinks.Add(new QuickLinks
                        {
                            Text = (quickLinkTextsb[quickLinkIterator]).Trim(),
                            Urls = (quickLinkUrlsb[quickLinkIterator]).Trim()
                        });
                    }
                }
                else if (quickLinkUrlsb.Length < quickLinkTextsb.Length)
                {
                    for (int quickLinkIterator = 0; quickLinkIterator < quickLinkUrlsb.Length; quickLinkIterator++)
                    {
                        quickLinks.Add(new QuickLinks
                        {
                            Text = (quickLinkTextsb[quickLinkIterator]).Trim(),
                            Urls = (quickLinkUrlsb[quickLinkIterator]).Trim()
                        });
                    }
                }
            }
            return quickLinks;
        }

        public dynamic GetLocations(string state, string county, string city, string zipcode)
        {
            List<Locations> location = new List<Locations>();
            string[] statesb = null;
            string[] countysb = null;
            string[] citysb = null;
            string[] zipcodesb = null;
            statesb = state.Split('|');
            countysb = county.Split('|');
            citysb = city.Split('|');
            zipcodesb = zipcode.Split('|');
            List<Locations> states = new List<Locations>();
            List<Locations> counties = new List<Locations>();
            List<Locations> cities = new List<Locations>();
            List<Locations> zipcodes = new List<Locations>();

            if (statesb.Length > 0 && (!string.IsNullOrEmpty(statesb.ToString())))
            {
                for (int locationIterator = 0; locationIterator < statesb.Length; locationIterator++)
                {
                    states.Add(new Locations
                    {
                        State = (statesb[locationIterator]).Trim()
                    });
                }
                foreach (var item in states)
                {
                    location.Add(item);
                }
            }
            else if (countysb.Length > 0 && (!string.IsNullOrEmpty(countysb.ToString())))
            {
                for (int locationIterator = 0; locationIterator < countysb.Length; locationIterator++)
                {
                    counties.Add(new Locations
                    {
                        County = (countysb[locationIterator]).Trim()
                    });
                }
                foreach (var item in counties)
                {
                    location.Add(item);
                }
            }

            else if (citysb.Length > 0 && (!string.IsNullOrEmpty(citysb.ToString())))
            {
                for (int locationIterator = 0; locationIterator < citysb.Length; locationIterator++)
                {
                    cities.Add(new Locations
                    {
                        City = (citysb[locationIterator]).Trim()
                    });
                }
                foreach (var item in cities)
                {
                    location.Add(item);
                }
            }

            else if (zipcodesb.Length > 0 && (!string.IsNullOrEmpty(zipcodesb.ToString())))
            {
                for (int locationIterator = 0; locationIterator < zipcodesb.Length; locationIterator++)
                {
                    zipcodes.Add(new Locations
                    {
                        ZipCode = (zipcodesb[locationIterator]).Trim()
                    });
                }
                foreach (var item in zipcodes)
                {
                    location.Add(item);
                }
            }
            
            return location;
        }

        public static bool ValidateTopicHeader(string[] header, int recordNumber)
        {
            bool correctHeader = false;
            IStructuralEquatable actualHeader = header;
            string[] expectedHeader = {"Topic_ID*", "Topic_Name*", "Parent_Topic*", "Keywords*", "Location_State*", "Location_County",
                "Location_City", "Location_Zip", "Overview", "Quick_Links_URL_text", "Quick_Links_URL_link", "Icon" };
               
            correctHeader = InsertResources.HeaderValidation(header, expectedHeader, "Topics");          
            return correctHeader;
        }

        public static void ErrorLogging(Exception ex, int recordNumber)
        {
            string strPath = Path.Combine(Environment.CurrentDirectory, "SampleFiles\\Error.txt");
            if (!File.Exists(strPath))
            {
                File.Create(strPath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(strPath))
            {
                sw.WriteLine("=============Error Logging ===========");
                sw.WriteLine("===========Start============= " + DateTime.Now);
                if (ex.Message == "Could not find document")
                {
                    sw.WriteLine("Error Message: " + ex.Message);
                }
                else if (ex.Message.Contains("Header"))
                {
                    sw.WriteLine("Error Message: " + ex.Message);
                }
                else
                {
                    sw.WriteLine("Error Message: " + ex.Message + "\n" + "Please correct error at record number: " + recordNumber);
                    sw.WriteLine("Stack Trace: " + ex.StackTrace);
                }
                sw.WriteLine("===========End============= " + DateTime.Now);
                sw.WriteLine();
            }
        }

        public static void ReadError()
        {
            string strPath = Path.Combine(Environment.CurrentDirectory, "SampleFiles\\Error.txt");
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