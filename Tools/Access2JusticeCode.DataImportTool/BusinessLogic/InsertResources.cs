using DocumentFormat.OpenXml.Packaging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Spreadsheet = DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using Access2Justice.DataImportTool.Models;

namespace Access2Justice.DataImportTool.BusinessLogic
{
    class InsertResources
    {
        #region Variables

        dynamic id = null;
        string name, resourceCategory, description, url, resourceType, state, county, city, zipcode = string.Empty;
        string overview, icon, address, telephone, eligibilityInformation, specialties, qualifications, businessHours = string.Empty;
        string organizationName, reviewerFullName, reviewerTitle, reviewText, reviewerImage = string.Empty;
        string articleName, headline, content, organizationalUnit = string.Empty;
        List<TopicTag> topicTagIds = null;
        List<Locations> locations = null;
        List<string> orgNameList = new List<string>();
        List<string> orgFullNameList = new List<string>();
        List<string> orgTitleList = new List<string>();
        List<string> orgReviewTextList = new List<string>();
        List<string> orgReviewerImageList = new List<string>();
        List<string> articleNameList = new List<string>();
        List<string> headlineList = new List<string>();
        List<string> contentList = new List<string>();

        #endregion Variables

        public dynamic CreateJsonFromCSV(string filePath)
        {
            int recordNumber = 1;
            Resource resource = new Resource();
            List<dynamic> ResourcesList = new List<dynamic>();
            List<dynamic> organizationsList = new List<dynamic>();
            List<dynamic> articlesList = new List<dynamic>();
            List<dynamic> organizationReviewsList = new List<dynamic>();
            List<dynamic> Resources = new List<dynamic>();
            List<string> sheetNames = new List<string>() { "Articles", "Article Sections", "Videos", "EssentialReadings & QuickLinks", "Forms", "Organizations", "OrganizationReviews (Optional)", "Related Links" };

            try
            {
                using (SpreadsheetDocument spreadsheetDocument =
                        SpreadsheetDocument.Open(filePath, true))
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
                            string resourceIdCell = string.Empty;
                            string resourceType = GetResourceType(sheet.Name.Value);
                            foreach (Spreadsheet.Row row in sheetData.Elements<Spreadsheet.Row>())
                            {
                                if (counter == 1)
                                {
                                    var resourceIdColumn = from a in keyValuePairs where a.Key == "Id" select a.Value.First().ToString();
                                    if (resourceIdColumn.Count() > 0)
                                    {
                                        resourceIdCell = resourceIdColumn.First();
                                    }
                                }

                                foreach (Spreadsheet.Cell cell in row.Elements<Spreadsheet.Cell>())
                                {
                                    cellValue = cell.InnerText;
                                    if (string.IsNullOrEmpty(cellValue))
                                    {
                                        if (!string.IsNullOrEmpty(resourceIdCell) && cell.CellReference == string.Concat(resourceIdCell + row.RowIndex))
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
                                            ResourceId = (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) ? Guid.NewGuid() : id,
                                            Name = name,
                                            Description = description,
                                            ResourceType = resourceType,
                                            Urls = url,
                                            TopicTags = topicTagIds,
                                            OrganizationalUnit = organizationalUnit,
                                            Location = locations,
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin
                                        };
                                        form.Validate();
                                        ResourcesList.Add(form);
                                        ClearVariableData();
                                    }
                                    if (resourceType == Constants.OrganizationResourceType)
                                    {
                                        Organization organization = new Organization()
                                        {
                                            ResourceId = (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) ? Guid.NewGuid() : id,
                                            Name = name,
                                            ResourceCategory = resourceCategory,
                                            Description = description,
                                            ResourceType = resourceType,
                                            Urls = url,
                                            TopicTags = topicTagIds,
                                            OrganizationalUnit = organizationalUnit,
                                            Location = locations,
                                            Address = address,
                                            Telephone = telephone,
                                            Overview = overview,
                                            Specialties = specialties,
                                            EligibilityInformation = eligibilityInformation,
                                            Qualifications = qualifications,
                                            BusinessHours = businessHours,
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin
                                        };
                                        organization.Validate();
                                        organizationsList.Add(organization);
                                        ClearVariableData();
                                    }
                                    if (resourceType == Constants.OrganizationReviews)
                                    {
                                        orgNameList.Add(organizationName);
                                        orgFullNameList.Add(reviewerFullName);
                                        orgTitleList.Add(reviewerTitle);
                                        orgReviewTextList.Add(reviewText);
                                        orgReviewerImageList.Add(reviewerImage);
                                        ClearVariableData();
                                    }
                                    if (resourceType == Constants.ArticleResourceType)
                                    {
                                        Article article = new Article()
                                        {
                                            ResourceId = (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) ? Guid.NewGuid() : id,
                                            Name = name,
                                            Description = description,
                                            ResourceType = resourceType,
                                            TopicTags = topicTagIds,
                                            OrganizationalUnit = organizationalUnit,
                                            Location = locations,
                                            Overview = overview,
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin
                                        };
                                        article.Validate();
                                        articlesList.Add(article);
                                        ClearVariableData();
                                    }
                                    if (resourceType == Constants.ArticleContents)
                                    {
                                        articleNameList.Add(articleName);
                                        headlineList.Add(headline);
                                        contentList.Add(content);
                                        ClearVariableData();
                                    }
                                    if (resourceType == Constants.VideoResourceType)
                                    {
                                        Video video = new Video()
                                        {
                                            ResourceId = (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) ? Guid.NewGuid() : id,
                                            Name = name,
                                            ResourceCategory = resourceCategory,
                                            Description = description,
                                            ResourceType = resourceType,
                                            Urls = url,
                                            TopicTags = topicTagIds,
                                            OrganizationalUnit = organizationalUnit,
                                            Location = locations,
                                            Overview = overview,
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin
                                        };
                                        video.Validate();
                                        ResourcesList.Add(video);
                                        ClearVariableData();
                                    }
                                    if (resourceType == Constants.EssentialReadingResourceType)
                                    {
                                        EssentialReading essentialReading = new EssentialReading()
                                        {
                                            ResourceId = (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) ? Guid.NewGuid() : id,
                                            Name = name,
                                            //Description = description,
                                            ResourceType = resourceType,
                                            Urls = url,
                                            TopicTags = topicTagIds,
                                            OrganizationalUnit = organizationalUnit,
                                            Location = locations,
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin
                                        };
                                        essentialReading.Validate();
                                        ResourcesList.Add(essentialReading);
                                        ClearVariableData();
                                    }
                                    if (resourceType == Constants.RelatedLinkResourceType)
                                    {
                                        RelatedLink relatedLink = new RelatedLink()
                                        {
                                            ResourceId = (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) ? Guid.NewGuid() : id,
                                            Name = name,
                                            Description = description,
                                            ResourceType = resourceType,
                                            Urls = url,
                                            TopicTags = topicTagIds,
                                            OrganizationalUnit = organizationalUnit,
                                            Location = locations,
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin
                                        };
                                        relatedLink.Validate();
                                        ResourcesList.Add(relatedLink);
                                        ClearVariableData();
                                    }
                                }
                                counter++;
                                recordNumber++;
                            }
                        }
                    }
                }

                foreach (var resourceList in organizationsList)
                {
                    List<OrganizationReviewer> organizationReviewer = new List<OrganizationReviewer>();
                    OrganizationReviewer orgReviewer = new OrganizationReviewer();
                    for (int iterator = 0; iterator < orgNameList.Count; iterator++)
                    {
                        var na = orgNameList[iterator];

                        if (resourceList.Name == orgNameList[iterator])
                        {
                            orgReviewer = new OrganizationReviewer
                            {
                                ReviewerFullName = orgFullNameList[iterator],
                                ReviewerTitle = orgTitleList[iterator],
                                ReviewText = orgReviewTextList[iterator],
                                ReviewerImage = orgReviewerImageList[iterator]
                            };
                            organizationReviewer.Add(orgReviewer);
                        }
                    }
                    var serializedResult = JsonConvert.SerializeObject(organizationReviewer);
                    var orgReviewData = JsonConvert.DeserializeObject(serializedResult);
                    resourceList.Reviewer = organizationReviewer;
                    ResourcesList.Add(resourceList);
                }

                foreach (var articleList in articlesList)
                {
                    List<ArticleContents> articleContentList = new List<ArticleContents>();
                    ArticleContents articleContents = new ArticleContents();
                    for (int iterator = 0; iterator < articleNameList.Count; iterator++)
                    {
                        var na = articleNameList[iterator];

                        if (articleList.Name == articleNameList[iterator])
                        {
                            articleContents = new ArticleContents
                            {
                                Headline = headlineList[iterator],
                                Content = contentList[iterator],
                            };
                            articleContentList.Add(articleContents);
                        }
                    }
                    var serializedResult = JsonConvert.SerializeObject(articleContentList);
                    var articleContentData = JsonConvert.DeserializeObject(serializedResult);
                    articleList.Contents = articleContentList;
                    ResourcesList.Add(articleList);
                }
            }
            catch (Exception ex)
            {
                InsertTopics.ErrorLogging(ex, recordNumber);
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
                if (trimTopicTagId.Length > 0)
                {
                    topicTagIds.Add(new TopicTag
                    {
                        TopicTags = trimTopicTagId
                    });
                }
            }
            return topicTagIds;
        }

        private void ClearVariableData()
        {
            id = null;
            name = string.Empty;
            resourceCategory = string.Empty;
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
            specialties = string.Empty;
            qualifications = string.Empty;
            businessHours = string.Empty;
            reviewerFullName = string.Empty;
            reviewerTitle = string.Empty;
            reviewerImage = string.Empty;
            articleName = string.Empty;
            headline = string.Empty;
            content = string.Empty;
            organizationalUnit = string.Empty;
            organizationName = string.Empty;
            reviewText = string.Empty;
        }
        private static Spreadsheet.SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<Spreadsheet.SharedStringItem>().ElementAt(id);
        }
        private static string GetResourceType(string sheetName)
        {
            switch (sheetName)
            {
                case "Articles":
                    return Constants.ArticleResourceType;
                case "Videos":
                    return Constants.VideoResourceType;
                case "EssentialReadings & QuickLinks":
                    return Constants.EssentialReadingResourceType;
                case "Forms":
                    return Constants.FormsResourceType;
                case "Organizations":
                    return Constants.OrganizationResourceType;
                case "OrganizationReviews (Optional)":
                    return Constants.OrganizationReviews;
                case "Article Sections":
                    return Constants.ArticleContents;
                case "Related Links":
                    return Constants.RelatedLinkResourceType;
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

            if (val.EndsWith("Name*", StringComparison.CurrentCultureIgnoreCase))
            {
                name = cellActualValue;
            }

            else if (val.EndsWith("Description*", StringComparison.CurrentCultureIgnoreCase))
            {
                description = InsertTopics.FormatData(cellActualValue);
            }

            else if (val.Equals("Resource_Type*", StringComparison.CurrentCultureIgnoreCase))
            {
                resourceType = cellActualValue;
            }

            else if (val.EndsWith("URL*", StringComparison.CurrentCultureIgnoreCase))
            {
                url = InsertTopics.FormatData(cellActualValue);
            }

            else if (val.EndsWith("Topic*", StringComparison.CurrentCultureIgnoreCase))
            {
                string topicTag = cellActualValue;
                topicTagIds = GetTopicTags(topicTag);
            }

            else if (val.EndsWith("Organizational_Unit*", StringComparison.CurrentCultureIgnoreCase))
            {
                organizationalUnit = cellActualValue;
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

            else if (val.Equals("Resource_Category", StringComparison.CurrentCultureIgnoreCase))
            {
                resourceCategory = cellActualValue;
            }

            #endregion Common field mapping

            if (resourceType == Constants.FormsResourceType)
            {
                if (val.EndsWith("Overview", StringComparison.CurrentCultureIgnoreCase))
                {
                    overview = InsertTopics.FormatData(cellActualValue);
                }
            }

            if (resourceType == Constants.OrganizationResourceType)
            {
                if (val.EndsWith("Org_Address*", StringComparison.CurrentCultureIgnoreCase))
                {
                    address = InsertTopics.FormatData(cellActualValue);
                }

                else if (val.EndsWith("Phone*", StringComparison.CurrentCultureIgnoreCase))
                {
                    telephone = InsertTopics.FormatData(cellActualValue);
                }

                else if (val.EndsWith("Overview", StringComparison.CurrentCultureIgnoreCase))
                {
                    overview = InsertTopics.FormatData(cellActualValue);
                }

                else if (val.EndsWith("Specialties", StringComparison.CurrentCultureIgnoreCase))
                {
                    specialties = InsertTopics.FormatData(cellActualValue);
                }

                else if (val.EndsWith("Eligibility_Information", StringComparison.CurrentCultureIgnoreCase))
                {
                    eligibilityInformation = InsertTopics.FormatData(cellActualValue);
                }

                else if (val.EndsWith("Qualifications", StringComparison.CurrentCultureIgnoreCase))
                {
                    qualifications = InsertTopics.FormatData(cellActualValue);
                }

                else if (val.EndsWith("Business_Hours", StringComparison.CurrentCultureIgnoreCase))
                {
                    businessHours = InsertTopics.FormatData(cellActualValue);
                }
            }

            if (resourceType == Constants.ArticleResourceType)
            {
                if (val.EndsWith("Overview", StringComparison.CurrentCultureIgnoreCase))
                {
                    overview = InsertTopics.FormatData(cellActualValue);
                }
            }

            if (resourceType == Constants.ArticleContents)
            {
                if (val.EndsWith("Article*", StringComparison.CurrentCultureIgnoreCase))
                {
                    articleName = InsertTopics.FormatData(cellActualValue);
                }

                else if (val.EndsWith("Section_Headline", StringComparison.CurrentCultureIgnoreCase))
                {
                    headline = InsertTopics.FormatData(cellActualValue);
                }

                else if (val.EndsWith("Section_Content", StringComparison.CurrentCultureIgnoreCase))
                {
                    content = InsertTopics.FormatData(cellActualValue);
                }
            }

            if (resourceType == Constants.VideoResourceType)
            {
                if (val.EndsWith("Overview", StringComparison.CurrentCultureIgnoreCase))
                {
                    overview = InsertTopics.FormatData(cellActualValue);
                }
            }

            if (resourceType == Constants.OrganizationReviews)
            {
                if (val.EndsWith("Organization*", StringComparison.CurrentCultureIgnoreCase))
                {
                    organizationName = InsertTopics.FormatData(cellActualValue);
                }

                else if (val.EndsWith("Reviewer_Full_Name*", StringComparison.CurrentCultureIgnoreCase))
                {
                    reviewerFullName = InsertTopics.FormatData(cellActualValue);
                }

                else if (val.EndsWith("Reviewer_Title", StringComparison.CurrentCultureIgnoreCase))
                {
                    reviewerTitle = InsertTopics.FormatData(cellActualValue);
                }

                else if (val.EndsWith("Review_Text*", StringComparison.CurrentCultureIgnoreCase))
                {
                    reviewText = InsertTopics.FormatData(cellActualValue);
                }

                else if (val.EndsWith("Reviewer_Image_URL", StringComparison.CurrentCultureIgnoreCase))
                {
                    reviewerImage = InsertTopics.FormatData(cellActualValue);
                }
            }
        }

        public static bool ValidateHeader(string[] header, int recordNumber, string resourceType)
        {
            bool correctHeader = false;
            IStructuralEquatable actualHeader = header;
            string[] expectedFormHeader = {"Id", "Name*", "Description*", "Resource_Type*", "URL*", "Topic*", "Organizational_Unit*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip" };

            string[] expectedOrganizationHeader = {"Id", "Name*", "Description*", "Resource_Type*", "URL*", "Topic*", "Organizational_Unit*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip", "Org_Address*", "Phone*", "Overview", "Specialties", "Eligibility_Information", "Qualifications", "Business_Hours", "Resource_Category" };

            string[] expectedArticleHeader = {"Id", "Name*", "Description*", "Resource_Type*", "Topic*", "Organizational_Unit*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip", "Overview" };

            string[] expectedVideoHeader = {"Id", "Name*", "Description*", "Resource_Type*", "URL*", "Topic*", "Organizational_Unit*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip", "Resource_Category", "Overview" };

            string[] expectedEssentialReadingHeader = {"Id", "Name*", "Resource_Type*", "URL*", "Topic*", "Organizational_Unit*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip"};

            string[] expectedOrganizationReviewsHeader = { "Organization*", "Reviewer_Full_Name*", "Reviewer_Title", "Review_Text*", "Reviewer_Image_URL" };

            string[] expectedArticleContentsHeader = { "Article*", "Section_Headline", "Section_Content" };

            string[] expectedRelatedLinkHeader = {"Id", "Name*", "Description*", "Resource_Type*", "URL*", "Topic*", "Organizational_Unit*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip" };
            try
            {
                if (resourceType == Constants.FormsResourceType)
                {
                    correctHeader = HeaderValidation(header, expectedFormHeader, Constants.FormsResourceType);
                }

                else if (resourceType == Constants.OrganizationResourceType)
                {
                    correctHeader = HeaderValidation(header, expectedOrganizationHeader, Constants.OrganizationResourceType);
                }

                else if (resourceType == Constants.ArticleResourceType)
                {
                    correctHeader = HeaderValidation(header, expectedArticleHeader, Constants.ArticleResourceType);
                }

                else if (resourceType == Constants.ArticleContents)
                {
                    correctHeader = HeaderValidation(header, expectedArticleContentsHeader, Constants.ArticleContents);
                }

                else if (resourceType == Constants.VideoResourceType)
                {
                    correctHeader = HeaderValidation(header, expectedVideoHeader, Constants.VideoResourceType);
                }

                else if (resourceType == Constants.EssentialReadingResourceType)
                {
                    correctHeader = HeaderValidation(header, expectedEssentialReadingHeader, Constants.EssentialReadingResourceType);
                }

                else if (resourceType == Constants.OrganizationReviews)
                {
                    correctHeader = HeaderValidation(header, expectedOrganizationReviewsHeader, Constants.OrganizationReviews);
                }

                else if (resourceType == Constants.RelatedLinkResourceType)
                {
                    correctHeader = HeaderValidation(header, expectedRelatedLinkHeader, Constants.RelatedLinkResourceType);
                }
            }
            catch (Exception ex)
            {
                InsertTopics.ErrorLogging(ex, recordNumber);
            }
            return correctHeader;
        }

        public static bool HeaderValidation(string[] header, string[] expectedHeader, string resourceType)
        {
            bool correctHeader = false;
            IStructuralEquatable actualHeader = header;
            if (actualHeader.Equals(expectedHeader, StructuralComparisons.StructuralEqualityComparer))
            {
                correctHeader = true;
            }
            else
            {
                string column = string.Empty;
                for (var iterator = 0; iterator < expectedHeader.Length; iterator++)
                {
                    if (!(header[iterator] == expectedHeader[iterator]))
                    {
                        column = expectedHeader[iterator];
                        break;
                    }
                }
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
                throw new Exception("Header Mismatch for " + resourceType + " at column " + column + "\n" + "Expected header:" + "\n" + logHeader);
            }
            return correctHeader;
        }
    }
}