using Access2Justice.Tools.Models;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Spreadsheet = DocumentFormat.OpenXml.Spreadsheet;

namespace Access2Justice.Tools.BusinessLogic
{
    class InsertResources
    {
        #region Variables

        dynamic id = null;
        string name, type, description, url, resourceType, state, county, city, zipcode = string.Empty;
        string overview, icon, address, telephone, eligibilityInformation = string.Empty;
        string reviewedByCommunityMember, reviewerFullName, reviewerTitle, reviewerImage = string.Empty;
        string headline1, content1, headline2, content2 = string.Empty;
        List<TopicTag> topicTagIds = null;
        List<Locations> locations = null;

        #endregion Variables

        public dynamic CreateJsonFromCSV()
        {
            int recordNumber = 1;
            Resource resource = new Resource();
            List<dynamic> ResourcesList = new List<dynamic>();
            List<dynamic> Resources = new List<dynamic>();
            string filePath = Path.Combine(Environment.CurrentDirectory, "SampleFiles\\Resources_Import_Template_v4.xlsx");
            List<string> sheetNames = new List<string>() { "Organizations", "Brochures or Articles", "Videos", "Related Links", "Forms" };

            try
            {
                using (SpreadsheetDocument spreadsheetDocument =
                        SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    Spreadsheet.Sheets sheets = workbookPart.Workbook.GetFirstChild<Spreadsheet.Sheets>();
                    foreach (Spreadsheet.Sheet sheet in sheets)
                    {
                        if (sheet.Name.HasValue && sheetNames.Find(a => a == sheet.Name.Value) != null)
                        {
                            Spreadsheet.Worksheet worksheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
                            Spreadsheet.SheetData sheetData = worksheet.Elements<Spreadsheet.SheetData>().First();
                            Spreadsheet.SharedStringTable sharedStringTable = spreadsheetDocument.WorkbookPart.SharedStringTablePart.SharedStringTable;

                            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                            string cellValue;
                            int counter = 0;
                            bool isValidated = false;
                            ClearVariableData();
                            topicTagIds = new List<TopicTag>();
                            locations = new List<Locations>();
                            string resourceType = GetResourceType(sheet.Name.Value);
                            foreach (Spreadsheet.Row row in sheetData.Elements<Spreadsheet.Row>())
                            {
                                foreach (Spreadsheet.Cell cell in row.Elements<Spreadsheet.Cell>())
                                {
                                    cellValue = cell.InnerText;
                                    if (!string.IsNullOrEmpty(cellValue))
                                    {
                                        string cellActualValue = string.Empty;
                                        if (cell.DataType == Spreadsheet.CellValues.SharedString)
                                        {
                                            cellActualValue = sharedStringTable.ElementAt(Int32.Parse(cellValue, CultureInfo.InvariantCulture)).InnerText;
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
                                                if (!ValidateHeader(headerValues.ToArray<string>(), recordNumber, resourceType))
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
                                                UpdateFormData(keyValue, cellActualValue, resourceType);
                                            }
                                        }
                                    }
                                }

                                if (counter > 0)
                                {
                                    InsertTopics topic = new InsertTopics();
                                    locations = topic.GetLocations(state, county, city, zipcode);
                                    if (resourceType == Constants.FormsResourceType)
                                    {
                                        Form form = new Form()
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
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin
                                        };
                                        form.Validate();
                                        ResourcesList.Add(form);
                                    }
                                    if (resourceType == Constants.OrganizationResourceType)
                                    {
                                        Organization organization = new Organization()
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
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin
                                        };
                                        organization.Validate();
                                        ResourcesList.Add(organization);
                                    }
                                    if (resourceType == Constants.ArticleResourceType)
                                    {
                                        Article article = new Article()
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
                                            HeadLine1 = headline1,
                                            Content1 = content1,
                                            HeadLine2 = headline2,
                                            Content2 = content2,
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin
                                        };
                                        article.Validate();
                                        ResourcesList.Add(article);
                                    }
                                    if (resourceType == Constants.VideoResourceType)
                                    {
                                        Video video = new Video()
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
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin
                                        };
                                        video.Validate();
                                        ResourcesList.Add(video);
                                    }
                                    if (resourceType == Constants.RelatedLinkResourceType)
                                    {
                                        EssentialReading essentialReading = new EssentialReading()
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
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin
                                        };
                                        essentialReading.Validate();
                                        ResourcesList.Add(essentialReading);
                                    }
                                }
                                counter++;
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

        private void ClearVariableData()
        {
            id = null;
            name = string.Empty;
            type = string.Empty;
            description = string.Empty;
            url = string.Empty;
            resourceType = string.Empty;
            state = string.Empty;
            county = string.Empty;
            city = string.Empty;
            zipcode = string.Empty;
            overview = string.Empty;
            icon = string.Empty;
            address = string.Empty;
            telephone = string.Empty;
            eligibilityInformation = string.Empty;
            reviewedByCommunityMember = string.Empty;
            reviewerFullName = string.Empty;
            reviewerTitle = string.Empty;
            reviewerImage = string.Empty;
            headline1 = string.Empty;
            content1 = string.Empty;
            headline2 = string.Empty;
            content2 = string.Empty;
        }
        private static Spreadsheet.SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<Spreadsheet.SharedStringItem>().ElementAt(id);
        }
        private static string GetResourceType(string sheetName)
        {
            switch (sheetName)
            {
                case "Organizations":
                    return Constants.OrganizationResourceType;
                case "Brochures or Articles":
                    return Constants.ArticleResourceType;
                case "Videos":
                    return Constants.VideoResourceType;
                case "Related Links":
                    return Constants.RelatedLinkResourceType;
                case "Forms":
                    return Constants.FormsResourceType;
                default:
                    return string.Empty;
            }
        }

        private void UpdateFormData(IEnumerable<string> keyValue, string cellActualValue, string resourceType)
        {
            string val = keyValue.First();
            cellActualValue = cellActualValue.Trim();

            #region Common field mapping

            if (val.EndsWith("Id", StringComparison.CurrentCultureIgnoreCase))
            {
                id = cellActualValue;
            }

            else if (val.EndsWith("Name*", StringComparison.CurrentCultureIgnoreCase))
            {
                name = cellActualValue;
            }

            else if (val.Equals("Type*", StringComparison.CurrentCultureIgnoreCase))
            {
                type = cellActualValue;
            }

            else if (val.EndsWith("Description*", StringComparison.CurrentCultureIgnoreCase))
            {
                description = cellActualValue;
            }

            else if (val.Equals("Resource Type*", StringComparison.CurrentCultureIgnoreCase))
            {
                resourceType = cellActualValue;
            }

            else if (val.EndsWith("URL*", StringComparison.CurrentCultureIgnoreCase))
            {
                url = cellActualValue;
            }

            else if (val.EndsWith("Topic*", StringComparison.CurrentCultureIgnoreCase))
            {
                string topicTag = cellActualValue;
                topicTagIds = GetTopicTags(topicTag);
            }

            else if (val.EndsWith("Location_State*", StringComparison.CurrentCultureIgnoreCase))
            {
                state = cellActualValue;
            }

            else if (val.EndsWith("Location_County", StringComparison.CurrentCultureIgnoreCase))
            {
                county = cellActualValue;
            }

            else if (val.EndsWith("Location_City", StringComparison.CurrentCultureIgnoreCase))
            {
                city = cellActualValue;
            }

            else if (val.EndsWith("Location_Zip", StringComparison.CurrentCultureIgnoreCase))
            {
                zipcode = cellActualValue;
            }

            else if (val.EndsWith("Icon", StringComparison.CurrentCultureIgnoreCase))
            {
                icon = cellActualValue;
            }

            #endregion Common field mapping

            if (resourceType == Constants.FormsResourceType)
            {
                if (val.EndsWith("Overview", StringComparison.CurrentCultureIgnoreCase))
                {
                    overview = cellActualValue;
                }
            }

            if (resourceType == Constants.OrganizationResourceType)
            {
                if (val.EndsWith("Org Address*", StringComparison.CurrentCultureIgnoreCase))
                {
                    address = cellActualValue;
                }

                else if (val.EndsWith("Phone*", StringComparison.CurrentCultureIgnoreCase))
                {
                    telephone = cellActualValue;
                }

                else if (val.EndsWith("Overview", StringComparison.CurrentCultureIgnoreCase))
                {
                    overview = cellActualValue;
                }

                else if (val.EndsWith("Eligibility Information", StringComparison.CurrentCultureIgnoreCase))
                {
                    eligibilityInformation = cellActualValue;
                }

                else if (val.EndsWith("Reviewed By Community Member", StringComparison.CurrentCultureIgnoreCase))
                {
                    reviewedByCommunityMember = cellActualValue;
                }

                else if (val.EndsWith("Reviewer Full Name", StringComparison.CurrentCultureIgnoreCase))
                {
                    reviewerFullName = cellActualValue;
                }

                else if (val.EndsWith("Reviewer Title", StringComparison.CurrentCultureIgnoreCase))
                {
                    reviewerTitle = cellActualValue;
                }

                else if (val.EndsWith("Reviewer Image", StringComparison.CurrentCultureIgnoreCase))
                {
                    reviewerImage = cellActualValue;
                }
            }

            if (resourceType == Constants.ArticleResourceType)
            {
                if (val.EndsWith("Overview*", StringComparison.CurrentCultureIgnoreCase))
                {
                    overview = cellActualValue;
                }

                else if (val.EndsWith("Headline 1 (optional)", StringComparison.CurrentCultureIgnoreCase))
                {
                    headline1 = cellActualValue;
                }

                else if (val.EndsWith("Content 1 (Optional)", StringComparison.CurrentCultureIgnoreCase))
                {
                    content1 = cellActualValue;
                }

                else if (val.EndsWith("Headline 2 (optional)", StringComparison.CurrentCultureIgnoreCase))
                {
                    headline2 = cellActualValue;
                }

                else if (val.EndsWith("Content 2 (Optional)", StringComparison.CurrentCultureIgnoreCase))
                {
                    content2 = cellActualValue;
                }
            }

            if (resourceType == Constants.VideoResourceType)
            {
                if (val.EndsWith("Overview", StringComparison.CurrentCultureIgnoreCase))
                {
                    overview = cellActualValue;
                }
            }
            
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
                if (resourceType == Constants.FormsResourceType)
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

                else if (resourceType == Constants.OrganizationResourceType)
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

                else if (resourceType == Constants.ArticleResourceType)
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

                else if (resourceType == Constants.VideoResourceType)
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

                else if (resourceType == Constants.RelatedLinkResourceType)
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