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
using Access2Justice.Shared.Models;
using System.Text;

namespace Access2Justice.DataImportTool.BusinessLogic
{
    class InsertResources
    {
        #region Variables

        dynamic id = null;
        string name, resourceCategory, description, url, resourceType, state, county, city, zipcode = string.Empty;
        string overview, address, telephone, eligibilityInformation, specialties, qualifications, businessHours = string.Empty;
        string organizationName, reviewerFullName, reviewerTitle, reviewText, reviewerImage = string.Empty;
        string articleName, headline, content, organizationalUnit = string.Empty; string delete = "N"; string ranking = "1";
        List<TopicTag> topicTagIds = null;
        List<Shared.Models.Location> locations = null;
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
            List<string> sheetNames = new List<string>() { Constants.ArticleSheetName, Constants.ArticleSectionSheetName, Constants.VideoSheetName, Constants.AdditionalReadingSheetName, Constants.FormSheetName, Constants.OrganizationSheetName, Constants.OrganizationReviewSheetName, Constants.RelatedLinkSheetName };

            var currentPage = string.Empty;
            var currentPageRecord = 1;

            try
            {
                using (SpreadsheetDocument spreadsheetDocument =
                        SpreadsheetDocument.Open(filePath, true))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    Spreadsheet.Sheets sheets = workbookPart.Workbook.GetFirstChild<Spreadsheet.Sheets>();
                    foreach (Spreadsheet.Sheet sheet in sheets)
                    {
                        currentPageRecord = 1;
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
                            locations = new List<Shared.Models.Location>();
                            string resourceIdCell = string.Empty;
                            string resourceType = GetResourceType(sheet.Name.Value);
                            currentPage = sheet.Name.Value;
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
                                        if (cell.DataType != null && cell.DataType == Spreadsheet.CellValues.SharedString)
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
                                            else if (cell.CellReference.Value.Length == 4)
                                            {
                                                keyValue = from a in keyValuePairs where a.Value.Take(3).First() == cell.CellReference.Value.Take(3).First() select a.Key;
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
                                    if (resourceType == Constants.FormResourceType)
                                    {
                                        Form form = new Form()
                                        {
                                            ResourceId = (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) ? Guid.NewGuid() : id,
                                            Name = name,
                                            Description = description,
                                            ResourceType = resourceType,
                                            Url = url,
                                            TopicTags = topicTagIds,
                                            OrganizationalUnit = organizationalUnit,
                                            Location = locations,
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin,
                                            Delete = delete,
                                            Ranking = GetRanking(ranking)

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
                                            Url = url,
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
                                            ModifiedBy = Constants.Admin,
                                            Delete = delete,
                                            Ranking = GetRanking(ranking)
                                        };
                                        organization.Validate();
                                        organizationsList.Add(organization);
                                        ClearVariableData();
                                    }
                                    if (resourceType == Constants.OrganizationReview)
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
                                            ModifiedBy = Constants.Admin,
                                            Delete = delete,
                                            Ranking = GetRanking(ranking)
                                        };
                                        article.Validate();
                                        articlesList.Add(article);
                                        ClearVariableData();
                                    }
                                    if (resourceType == Constants.ArticleContent)
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
                                            Url = url,
                                            TopicTags = topicTagIds,
                                            OrganizationalUnit = organizationalUnit,
                                            Location = locations,
                                            Overview = overview,
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin,
                                            Delete = delete,
                                            Ranking = GetRanking(ranking)
                                        };
                                        video.Validate();
                                        ResourcesList.Add(video);
                                        ClearVariableData();
                                    }
                                    if (resourceType == Constants.AdditionalReadingResourceType)
                                    {
                                        AdditionalReading additionalReading = new AdditionalReading()
                                        {
                                            ResourceId = (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id)) ? Guid.NewGuid() : id,
                                            Name = name,
                                            ResourceType = resourceType,
                                            Url = url,
                                            TopicTags = topicTagIds,
                                            OrganizationalUnit = organizationalUnit,
                                            Location = locations,
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin,
                                            Delete = delete,
                                            Ranking = GetRanking(ranking)
                                        };
                                        additionalReading.Validate();
                                        ResourcesList.Add(additionalReading);
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
                                            Url = url,
                                            TopicTags = topicTagIds,
                                            OrganizationalUnit = organizationalUnit,
                                            Location = locations,
                                            CreatedBy = Constants.Admin,
                                            ModifiedBy = Constants.Admin,
                                            Delete = delete,
                                            Ranking = GetRanking(ranking)
                                        };
                                        relatedLink.Validate();
                                        ResourcesList.Add(relatedLink);
                                        ClearVariableData();
                                    }
                                }
                                counter++;
                                recordNumber++;
                                currentPageRecord++;
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
                    List<ArticleContent> articleContentList = new List<ArticleContent>();
                    ArticleContent articleContents = new ArticleContent();
                    for (int iterator = 0; iterator < articleNameList.Count; iterator++)
                    {
                        var na = articleNameList[iterator];

                        if (articleList.Name == articleNameList[iterator])
                        {
                            articleContents = new ArticleContent
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
                InsertTopics.ErrorLogging(ex,
                    $"Please correct error at record number: {recordNumber} (sheet {currentPage}, row {currentPageRecord})");
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
            delete = "N";
            ranking = "1";
        }
        private static Spreadsheet.SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<Spreadsheet.SharedStringItem>().ElementAt(id);
        }
        private static string GetResourceType(string sheetName)
        {
            switch (sheetName)
            {
                case Constants.ArticleSheetName:
                    return Constants.ArticleResourceType;
                case Constants.VideoSheetName:
                    return Constants.VideoResourceType;
                case Constants.AdditionalReadingSheetName:
                    return Constants.AdditionalReadingResourceType;
                case Constants.FormSheetName:
                    return Constants.FormResourceType;
                case Constants.OrganizationSheetName:
                    return Constants.OrganizationResourceType;
                case Constants.OrganizationReviewSheetName:
                    return Constants.OrganizationReview;
                case Constants.ArticleSectionSheetName:
                    return Constants.ArticleContent;
                case Constants.RelatedLinkSheetName:
                    return Constants.RelatedLinkResourceType;
                default:
                    return string.Empty;
            }
        }

        public static int GetRanking(string ranking)
        {
            var isRankingCorrect = int.TryParse(ranking, out var rank);
            if (!isRankingCorrect)
            {
                throw new InvalidCastException("Ranking must be a number.");
            }

            return rank;
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

            if (resourceType == Constants.FormResourceType)
            {
                if (val.EndsWith("Overview", StringComparison.CurrentCultureIgnoreCase))
                {
                    overview = InsertTopics.FormatData(cellActualValue);
                }
            }

            else if (val.EndsWith("Delete", StringComparison.CurrentCultureIgnoreCase))
            {
                delete = cellActualValue;
            }

            if (val.EndsWith("Ranking", StringComparison.CurrentCultureIgnoreCase))
            {
                ranking = cellActualValue;
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

            if (resourceType == Constants.ArticleContent)
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

            if (resourceType == Constants.OrganizationReview)
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

            string[] expectedAdditionalReadingHeader = {"Id", "Name*", "Resource_Type*", "URL*", "Topic*", "Organizational_Unit*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip"};

            string[] expectedOrganizationReviewHeader = { "Organization*", "Reviewer_Full_Name*", "Reviewer_Title", "Review_Text*", "Reviewer_Image_URL" };

            string[] expectedArticleContentsHeader = { "Article*", "Section_Headline", "Section_Content" };

            string[] expectedRelatedLinkHeader = {"Id", "Name*", "Description*", "Resource_Type*", "URL*", "Topic*", "Organizational_Unit*", "Location_State*", "Location_County", "Location_City",
                    "Location_Zip" };
            try
            {
                if (resourceType == Constants.FormResourceType)
                {
                    correctHeader = HeaderValidation(header, expectedFormHeader, Constants.FormResourceType);
                }

                else if (resourceType == Constants.OrganizationResourceType)
                {
                    correctHeader = HeaderValidation(header, expectedOrganizationHeader, Constants.OrganizationResourceType);
                }

                else if (resourceType == Constants.ArticleResourceType)
                {
                    correctHeader = HeaderValidation(header, expectedArticleHeader, Constants.ArticleResourceType);
                }

                else if (resourceType == Constants.ArticleContent)
                {
                    correctHeader = HeaderValidation(header, expectedArticleContentsHeader, Constants.ArticleContent);
                }

                else if (resourceType == Constants.VideoResourceType)
                {
                    correctHeader = HeaderValidation(header, expectedVideoHeader, Constants.VideoResourceType);
                }

                else if (resourceType == Constants.AdditionalReadingResourceType)
                {
                    correctHeader = HeaderValidation(header, expectedAdditionalReadingHeader, Constants.AdditionalReadingResourceType);
                }

                else if (resourceType == Constants.OrganizationReview)
                {
                    correctHeader = HeaderValidation(header, expectedOrganizationReviewHeader, Constants.OrganizationReview);
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
                var missedElements = expectedHeader.Except(header);
                var requiredElements = missedElements.Where(x => x.EndsWith("*"));
                var optionalElements = missedElements.Except(requiredElements);
                if (requiredElements.Any())
                {
                    System.Windows.Forms.MessageBox.Show("File missed required elements such as " + string.Join(" ", requiredElements));
                    throw new Exception("File missed required elements such as " + string.Join(" ", requiredElements));
                }

                if (optionalElements.Any())
                {
                    System.Windows.Forms.MessageBox.Show("File missed optional elements such as " + string.Join(" ", optionalElements));
                }
                correctHeader = true;
            }
            
            return correctHeader;
        }
    }
}