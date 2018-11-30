using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Spreadsheet = DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Access2Justice.Shared.Models;

namespace Access2Justice.DataImportTool.BusinessLogic
{
    public class InsertTopics
    {
        public dynamic CreateJsonFromCSV(string filePath)
        {
            int recordNumber = 1;
            Shared.Models.Topic topic = new Shared.Models.Topic();
            List<dynamic> topicsList = new List<dynamic>();
            List<dynamic> topics = new List<dynamic>();
            try
            {
                using (SpreadsheetDocument spreadsheetDocument =
                    SpreadsheetDocument.Open(filePath, true))
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
                        dynamic id = null; string name = string.Empty; string keywords = string.Empty; string organizationalUnit = string.Empty;
                        string state = string.Empty; string county = string.Empty; string city = string.Empty; string zipcode = string.Empty;
                        string overview = string.Empty; string icon = string.Empty;
                        List<ParentTopicId> parentTopicIds = new List<ParentTopicId>();
                        List<Shared.Models.Location> locations = new List<Shared.Models.Location>();
                        string topicIdCell = string.Empty;
                        if (counter > 0)
                        {
                            var topicIdColumn = from a in keyValuePairs where a.Key == "Topic_ID*" select a.Value.First().ToString();
                            if (topicIdColumn.Count() > 0)
                            {
                                topicIdCell = topicIdColumn.First();
                            }
                        }

                        foreach (Spreadsheet.Cell cell in row.Elements<Spreadsheet.Cell>())
                        {
                            cellValue = cell.InnerText;
                            if (string.IsNullOrEmpty(cellValue))
                            {
                                if (!string.IsNullOrEmpty(topicIdCell) && cell.CellReference == string.Concat(topicIdCell + row.RowIndex))
                                {
                                    cell.CellValue = new CellValue(Guid.NewGuid().ToString());
                                    cell.DataType = new EnumValue<CellValues>(CellValues.String);
                                    workbookPart.Workbook.Save();
                                }
                            }
                            else if (!string.IsNullOrEmpty(cellValue))
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
                                            keywords = FormatData(cellActualValue);
                                        }

                                        else if (val.EndsWith("Organizational_Unit*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            organizationalUnit = FormatData(cellActualValue);
                                        }

                                        else if (val.EndsWith("Location_State*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            state = FormatData(cellActualValue);
                                        }

                                        else if (val.EndsWith("Location_County", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            county = FormatData(cellActualValue);
                                        }

                                        else if (val.EndsWith("Location_City", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            city = FormatData(cellActualValue);

                                        }

                                        else if (val.EndsWith("Location_Zip", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            zipcode = FormatData(cellActualValue);
                                        }

                                        else if (val.EndsWith("Overview*", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            overview = FormatData(cellActualValue);
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
                            locations = GetLocations(state, county, city, zipcode);
                            topic = new Shared.Models.Topic()
                            {
                                Id = (string.IsNullOrEmpty(id)|| string.IsNullOrWhiteSpace(id)) ? Guid.NewGuid() : id,
                                Name = name,
                                Overview = overview,
                                ParentTopicId = parentTopicIds.Count > 0 ? parentTopicIds : null,
                                ResourceType = "Topics",
                                Keywords = keywords,
                                OrganizationalUnit = organizationalUnit,
                                Location = locations,
                                Icon = icon,
                                CreatedBy = Constants.Admin,
                                ModifiedBy = Constants.Admin
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
                topics = null;
            }
            return topics;
        }

        public dynamic GetParentId(string parentId)
        {
            string[] parentsb = null;
            parentsb = parentId.Split('|');
            List<ParentTopicId> parentTopicIds = new List<ParentTopicId>();
            for (int topicIdIterator = 0; topicIdIterator < parentsb.Length; topicIdIterator++)
            {
                string trimParentTopicId = (parentsb[topicIdIterator]).Trim();
                string parentTopicGuid = string.Empty;
                if (trimParentTopicId.Length > 0)
                {
                    parentTopicIds.Add(new ParentTopicId
                    {
                        ParentTopicIds = trimParentTopicId
                    });
                }
            }
            return parentTopicIds;
        }
                
        public dynamic GetLocations(string state, string county, string city, string zipcode)
        {
            List<Shared.Models.Location> location = new List<Shared.Models.Location>();
            string[] statesb = null;
            string[] countysb = null;
            string[] citysb = null;
            string[] zipcodesb = null;
            statesb = !string.IsNullOrEmpty(state) ? state.Split('|') : null;
            countysb = !string.IsNullOrEmpty(county) ? county.Split('|') : null;
            citysb = !string.IsNullOrEmpty(city) ? city.Split('|') : null;
            zipcodesb = !string.IsNullOrEmpty(zipcode) ? zipcode.Split('|') : null;
            List<Shared.Models.Location> states = new List<Shared.Models.Location>();
            List<Shared.Models.Location> counties = new List<Shared.Models.Location>();
            List<Shared.Models.Location> cities = new List<Shared.Models.Location>();
            List<Shared.Models.Location> zipcodes = new List<Shared.Models.Location>();

            if (statesb != null)
            {
                for (int locationIterator = 0; locationIterator < statesb.Length; locationIterator++)
                {
                    states.Add(new Shared.Models.Location
                    {
                        State = (statesb[locationIterator]).Trim()
                    });
                }
                foreach (var item in states)
                {
                    location.Add(item);
                }
            }
            if (countysb != null)
            {
                for (int locationIterator = 0; locationIterator < countysb.Length; locationIterator++)
                {
                    counties.Add(new Shared.Models.Location
                    {
                        County = (countysb[locationIterator]).Trim()
                    });
                }
                foreach (var item in counties)
                {
                    location.Add(item);
                }
            }
            if (citysb != null)
            {
                for (int locationIterator = 0; locationIterator < citysb.Length; locationIterator++)
                {
                    cities.Add(new Shared.Models.Location
                    {
                        City = (citysb[locationIterator]).Trim()
                    });
                }
                foreach (var item in cities)
                {
                    location.Add(item);
                }
            }
            if (zipcodesb != null)
            {
                for (int locationIterator = 0; locationIterator < zipcodesb.Length; locationIterator++)
                {
                    zipcodes.Add(new Shared.Models.Location
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
            string[] expectedHeader = {"Topic_ID*", "Topic_Name*", "Parent_Topic*", "Keywords*", "Organizational_Unit*", "Location_State*", "Location_County",
                "Location_City", "Location_Zip", "Overview*", "Icon" };
               
            correctHeader = InsertResources.HeaderValidation(header, expectedHeader, "Topics");          
            return correctHeader;
        }

        public static void ErrorLogging(Exception ex, int recordNumber)
        {
            string strPath = Path.Combine(Environment.CurrentDirectory, "..\\..\\SampleFiles\\Error.txt");
            if (File.Exists(strPath))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                File.Delete(strPath);
            }
            else
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

        public static dynamic FormatData(string inputText)
        {
            var inputTrimmed = inputText.Trim();
            return inputTrimmed.Replace("_x000D_", "");
        }
    }
}