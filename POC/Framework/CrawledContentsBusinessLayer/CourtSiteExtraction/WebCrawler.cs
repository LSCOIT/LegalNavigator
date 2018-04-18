
namespace CrawledContentsBusinessLayer.CourtSiteExtraction
{
    using ContentDataAccess;
    using ContentDataAccess.DataContextFactory;
    using ContentDataAccess.StateBasedContents;
    using CrawledContentDataAccess.StateBasedContents;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;

    //  using WlhDataExtraction.Utils;

    public class WebCrawler
    {
          public static string s_statelhUrlBase = "http://www.courts.alaska.gov/shc/";


        public List<Intent> GetWebPageList()
        {
            // var bingList = GetUrlListFromBing();
            //bingList = NormalizeUrlName(bingList);

            var wlhSiteList = GetUrlListFromStateLawHelpSite();
            // wlhSiteList = NormalizeUrlName(wlhSiteList);

            // var mergedList = CleanupAndMergeLists(bingList, wlhSiteList);
            // var isContained= mergedList.Exists(x => x.Contains("ending-your-marriage-or-domestic-partnership"));
            //return mergedList;
            return wlhSiteList;
        }

        private List<string> GetUrlListFromBing()
        {
            // Logger.Info("Starting to get URL list from Bing");

            string urlBase = "https://www.bing.com/search?q=site%3Awashingtonlawhelp.org&first=";
            List<string> urlsList = new List<string>();

            for (int i = 0; i < 400; i++)
            {
                Thread.Sleep(300);
                //  Logger.Info("num articles extracted so far: " + urlsList.Count);

                string webSiteUrl = $"{urlBase}{i * 10 + 1}";
                // Logger.Info($"Extracting articles from: {webSiteUrl}");

                string html = GetPageHtml(webSiteUrl);
                if (string.IsNullOrEmpty(html)) continue;

                string begSig = "b_algo\"><h2><a href=\"";
                string endSig = "\"";

                while (html.Contains(begSig))
                {
                    var indxBegin = html.IndexOf(begSig);

                    if (-1 == indxBegin)
                    {
                        break;
                    }

                    html = html.Remove(0, indxBegin + begSig.Length);
                    int indxEnd = html.IndexOf(endSig);

                    string url = html.Substring(0, indxEnd);

                    if (url.Contains("/resource/") && !urlsList.Contains(url))
                    {
                        urlsList.Add(url);
                    }

                    html = html.Substring(indxEnd);
                }
            }

            // Logger.Info("Finished. # unique URLs found from Bing: " + urlsList.Count);

            return urlsList;
        }

        private List<Intent> GetUrlListFromStateLawHelpSite()
        {
            // Logger.Info("Starting to get URL list from Washington Law Help site");
         
            List<string> ret = new List<string>();
            var intents = Intents.Get();

            foreach (var intent in intents)
            {
                // Logger.Info($"Number of urls so far: {ret.Count}. Getting sub-topics for: {topic.Url}");

                var subTopics = SubTopics.Get(intent.Url);
              //  intent. = subTopics;
                foreach (var subTopic in subTopics)
                {
                    var docs = Documents.Get(subTopic.Url);
                    subTopic.Docs = docs;
                    foreach (var doc in docs)
                    {
                        
                        if (!doc.Url.Contains("/resource/")) continue;
                        doc.DocumentContents = GetRelevantDocumentContents(doc.Url);
                        if(doc.DocumentContents.Count > 0 )
                        {
                            doc.HasMoreContents = true;
                        }
                        ret.Add(doc.Url);
                    }
                }
            }

            return intents;
        }

        public List<DocumentContent> GetRelevantDocumentContents(string pageUrl)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlNode.ElementsFlags["br"] = HtmlAgilityPack.HtmlElementFlag.Empty;
            doc.OptionWriteEmptyNodes = true;
            List<DocumentContent> relevantDocumentContents = new List<DocumentContent>();
            try
            {
                var webRequest = HttpWebRequest.Create(pageUrl);

            Stream stream = webRequest.GetResponse().GetResponseStream();
            doc.Load(stream);
            stream.Close();
           }
           catch (System.UriFormatException uex)
           {
                    Console.WriteLine("There was an error in the format of the url: " + pageUrl, uex);
                    //throw;
           }
           catch (System.Net.WebException wex)
           {
                    Console.WriteLine("There was an error connecting to the url: " + pageUrl, wex);
                    //throw;
           }
         
          HtmlAgilityPack.HtmlNode titleNode = null;
          HtmlAgilityPack.HtmlNode possibleTitleNodeFromH2 = null;
          HtmlAgilityPack.HtmlNode possibleTitleNodeFromH4 = null;

            var itemNumber = 1;

            do
            {

                var titleNodeSelector = string.Format("/html/body/div[2]/div[3]/div[1]/div[2]/div[1]/div[2]/div/div/h3[{0}]", itemNumber);
                var possibleTitleNodeSelectorFromH2 = $"/html/body/div[2]/div[3]/div[1]/div[2]/div[1]/div[2]/div/div/h2[{itemNumber}]";
                var possibleTitleNodeSelectorFromH4= $"//*[@id=\"main\"]/div[2]/div[1]/div[3]/div[{itemNumber + 1}]/div[1]/div[1]/div/h4";
                titleNode = doc.DocumentNode.SelectSingleNode(titleNodeSelector);
                possibleTitleNodeFromH2 = doc.DocumentNode.SelectSingleNode(possibleTitleNodeSelectorFromH2);
                possibleTitleNodeFromH4 = doc.DocumentNode.SelectSingleNode(possibleTitleNodeSelectorFromH4);

                if (titleNode != null)
                {


                    var documentContent = new DocumentContent() { Title = titleNode.InnerText };

                    var nextsibling = titleNode;//to be incremented in the 1st line of loop body

                    StringBuilder content = new StringBuilder();
                    try
                    {
                        do
                        {
                            nextsibling = nextsibling.NextSibling;
                            if (nextsibling != null)
                            {
                                if (nextsibling.Name == "p" || nextsibling.Name == "ul" || nextsibling.Name == "ol")
                                {
                                    content.Append(nextsibling.InnerText + "<br/>");
                                }
                                else if (nextsibling.Name == "div")
                                {
                                    if (nextsibling.ChildNodes["ul"] != null || nextsibling.ChildNodes["ol"] != null)
                                    {
                                        content.Append(nextsibling.InnerText + "<br/>");
                                    }
                                    else if (nextsibling.ChildNodes["blockquote"] != null)
                                    {
                                        content.Append(nextsibling.ChildNodes["blockquote"].InnerText + "<br/>");
                                        if (nextsibling.ChildNodes["h3"] != null)
                                        {
                                            var result = GetRelevantDocumentContentsOnPageSection(doc, "/html/body/div[2]/div[3]/div[1]/div[2]/div[1]/div[2]/div/div/div[4]/h3[{0}]");
                                            if (result != null)
                                            {
                                                relevantDocumentContents.AddRange(result);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        while (nextsibling != null && nextsibling.Name != "h3");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    documentContent.Content = content.ToString();
                        relevantDocumentContents.Add(documentContent);
                }

                if (possibleTitleNodeFromH2 != null)
                {


                    var documentContent = new DocumentContent() { Title = possibleTitleNodeFromH2.InnerText };

                    var nextsibling = possibleTitleNodeFromH2;//to be incremented in the 1st line of loop body

                    StringBuilder content = new StringBuilder();
                    try
                    {
                        do
                        {
                            nextsibling = nextsibling.NextSibling;
                            if (nextsibling != null)
                            {
                                if (nextsibling.Name == "p" || nextsibling.Name == "ul" || nextsibling.Name == "ol")
                                {
                                    content.Append(nextsibling.InnerText + "<br/>");
                                }
                                else if (nextsibling.Name == "div")
                                {
                                    if (nextsibling.ChildNodes["ul"] != null || nextsibling.ChildNodes["ol"] != null)
                                    {
                                        content.Append(nextsibling.InnerText + "<br/>");
                                    }
                                    else if (nextsibling.ChildNodes["blockquote"] != null)
                                    {
                                        content.Append(nextsibling.ChildNodes["blockquote"].InnerText + "<br/>");                                        
                                    }
                                }
                            }
                        }
                        while (nextsibling != null && nextsibling.Name != "h2" && nextsibling.Name != "h3");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    if (!string.IsNullOrEmpty(content.ToString()))
                    {
                        documentContent.Content = content.ToString();
                        relevantDocumentContents.Add(documentContent);
                    }
                   

                }
                if (possibleTitleNodeFromH4 != null)
                {


                    var documentContent = new DocumentContent() { Title = possibleTitleNodeFromH4.InnerText };

                    var nextsibling = possibleTitleNodeFromH4;//to be incremented in the 1st line of loop body

                    StringBuilder content = new StringBuilder();
                    try
                    {
                        do
                        {
                            nextsibling = nextsibling.ParentNode.ParentNode.ParentNode.NextSibling;
                            if (nextsibling != null)
                            {
                                while(nextsibling.Name == "a")
                                {
                                    nextsibling = nextsibling.NextSibling;
                                }
                                if (nextsibling.Name == "p" || nextsibling.Name == "ul" || nextsibling.Name == "ol")
                                {
                                    content.Append(nextsibling.InnerText + "<br/>");
                                }
                                else if (nextsibling.Name == "div")
                                {
                                    if (nextsibling.ChildNodes["ul"] != null || nextsibling.ChildNodes["ol"] != null)
                                    {
                                        content.Append(nextsibling.InnerText + "<br/>");
                                    }
                                    else if (nextsibling.ChildNodes["blockquote"] != null)
                                    {
                                        content.Append(nextsibling.ChildNodes["blockquote"].InnerText + "<br/>");
                                    }
                                }
                            }
                        }
                        while (nextsibling != null && nextsibling.Name != "h2" && nextsibling.Name != "h3" && nextsibling.Name != "h4");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    if (!string.IsNullOrEmpty(content.ToString()))
                    {
                        documentContent.Content = content.ToString();
                        relevantDocumentContents.Add(documentContent);
                    }


                }
                itemNumber++;

            }
            while ((titleNode != null)) ;
            
            
                      
          return relevantDocumentContents;
        }

        public List<DocumentContent> GetRelevantDocumentContentsOnPageSection(HtmlAgilityPack.HtmlDocument doc, string xpath)
        {
            
            HtmlAgilityPack.HtmlNode.ElementsFlags["br"] = HtmlAgilityPack.HtmlElementFlag.Empty;
            doc.OptionWriteEmptyNodes = true;
            List<DocumentContent> relevantDocumentContents = new List<DocumentContent>();
            HtmlAgilityPack.HtmlNode titleNode = null;

            var itemNumber = 1;
            do
            {

                var titleNodeSelector = string.Format(xpath, itemNumber);

                titleNode = doc.DocumentNode.SelectSingleNode(titleNodeSelector);

                if (titleNode != null)
                {


                    var documentContent = new DocumentContent() { Title = titleNode.InnerText };

                    var nextsibling = titleNode;//to be incremented in the 1st line of loop body

                    StringBuilder content = new StringBuilder();

                    do
                    {
                        nextsibling = nextsibling.NextSibling;
                        if (nextsibling != null)
                        {
                            if (nextsibling.Name == "p" || nextsibling.Name == "ul" || nextsibling.Name == "ol")
                            {
                                content.Append(nextsibling.InnerText + "<br/>");
                            }
                        }
                    }
                    while (nextsibling != null && nextsibling.Name != "h3");

                    documentContent.Content = content.ToString();
                    relevantDocumentContents.Add(documentContent);
                }

                itemNumber++;

            }
            while ((titleNode != null));

            return relevantDocumentContents;

        }

        private List<string> NormalizeUrlName(List<string> list)
        {
            var ret = new List<string>();

            foreach (var row in list)
            {
                int idx = row.IndexOf("?");
                var cleanUrl = (idx > -1) ? row.Substring(0, idx) : row;
                cleanUrl = cleanUrl.Replace("https://", "http://");
                ret.Add(cleanUrl);
            }

            return ret;
        }

        private List<string> CleanupAndMergeLists(List<string> list1, List<string> list2)
        {
            var combinedList = list1.Concat(list2).Distinct().ToList();
            return combinedList;
        }

        public static string NormalizeName(string name)
        {
            var ret = WebUtility.HtmlDecode(name).ToLower().Replace("and/or", " and or").Replace("/", " or ");
            ret = ret.Replace("“", "").Replace("”", "").Replace("…", "...");
            ret = ret.Replace("–", "-").Replace("’", "'");

            return ret;
        }

        public static string GetPageHtml(string url)
        {
            try
            {
                using (var client = new WebClient() { })
                {
                    client.Headers.Add("user-agent", "crawler");

                    return client.DownloadString(url);
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #region Sub-classes

        public class Documents
        {
            public static List<Document> Get(string subTopicUrl)
            {
                List<Document> docs = new List<Document>();

                int numPages = GetNumPages(subTopicUrl);

                for (int i = 1; i <= numPages; i++)
                {
                    var html = GetPageHtml($"{subTopicUrl}?page={i}");
                    var lines = html.Split('\n');

                    foreach (var line in lines)
                    {
                        if (!line.Contains("<a href=\"/resource/") || line.Contains("class=\"all\"") ||
                            line.Contains("?lang=")) continue;

                        docs.Add(GetDocument(line));
                    }
                }

                return docs;
            }

            private static Document GetDocument(string line)
            {
                var htmlBeg = "<a href=\"";

                var idx = line.IndexOf(htmlBeg);

                if (-1 == idx) return null;

                // get rid of junk before the relative URL link
                var parsedLine = line.Substring(idx);
                parsedLine = parsedLine.Replace(htmlBeg, "");

                // get rid of the junk after the document name
                idx = parsedLine.IndexOf("</a>");
                parsedLine = parsedLine.Substring(0, idx);

                // save off the relative URL
                var htmlRef = "?ref=";
                idx = parsedLine.IndexOf(htmlRef);
                var url = s_statelhUrlBase + parsedLine.Substring(0, idx);

                // save the document title name
                idx = parsedLine.IndexOf(">") + 1;
                var title = NormalizeName(parsedLine.Substring(idx));

                // TODO - let's disable this for now in order to speed up the process
                var content = GetPageHtml(url);
                //var content = string.Empty;
                htmlBeg = "<div class=\"resource article\"";
                idx = content.IndexOf(htmlBeg);

                if (idx != -1)
                {
                    // get rid of junk before the relative URL link
                    parsedLine = content.Substring(idx);
                    idx = parsedLine.IndexOf("<p");
                    parsedLine = parsedLine.Substring(idx + 3);
                    idx = parsedLine.IndexOf("</p");
                    content = parsedLine.Substring(0, idx);
                }
                else
                {
                    content = string.Empty;
                }

                return new Document
                {
                    Content = content,
                    Title = title,
                    Url = url
                };
            }

            private static int GetNumPages(string url)
            {
                var html = GetPageHtml(url);

                int ret = 1;

                while (true)
                {
                    if (!html.Contains("?page=" + ret)) break;
                    ret++;
                }

                return ret - 1;
            }

            //public class Document
            //{
            //    public string Title { get; set; }
            //    public string Url { get; set; }
            //    public string Content { get; set; }
            //}
        }

        public class SubTopics
        {
            public static List<SubTopic> Get(string url)
            {
                var trackTopics = new Dictionary<string, SubTopic>();

                var html = GetPageHtml(url);
                if (html.Contains("class=\"sep\""))
                {
                    var lines = html.Split('\n');

                    foreach (var line in lines)
                    {
                        if (!line.Contains("class=\"sep\"")) continue;

                        var topic = GetTopicFromLine(line, "<a class=\"subtopic\" href=\"");

                        if (topic != null && !trackTopics.ContainsKey(topic.Name))
                        {
                            trackTopics.Add(topic.Name, topic);
                        }
                    }
                }

                return trackTopics.Values.ToList();
            }

            private static SubTopic GetTopicFromLine(string line, string htmlBeg)
            {
                var idx = line.IndexOf(htmlBeg);

                if (-1 == idx) return null;

                var parsedLine = line.Substring(idx);
                parsedLine = parsedLine.Replace(htmlBeg, "");

                idx = parsedLine.IndexOf("</a>");

                parsedLine = parsedLine.Substring(0, idx);

                idx = parsedLine.IndexOf("\"");
                var url = s_statelhUrlBase + parsedLine.Substring(0, idx);

                idx = parsedLine.IndexOf(">");
                var name = NormalizeName(parsedLine.Substring(idx + 1));

                return new SubTopic
                {
                    Url = url,
                    Name = name
                };
            }

            //public class Topic
            //{
            //    public string Url { get; set; }
            //    public string Name { get; set; }

            //    public List<Documents.Document> Docs { get; set; }
            //}
        }

        public class Intents
        {
            public static List<Intent> Get()
            {
                var issueUrl = s_statelhUrlBase + @"representing-yourself.htm";

                var trackIntents = new Dictionary<string, Intent>();

                var html = GetPageHtml(issueUrl);
                var lines = html.Split('\n');

                var initialIndex = html.IndexOf("h1");
                var finalIndex = html.IndexOf("</h1>");
                var title = RemoveHTMLComments(html.Substring(initialIndex + 3, finalIndex- initialIndex - 3)?.Trim());
                var parrentNode = new Intent { Title = title, Url = issueUrl };

                IContentDataRepository crowledContentDataRepository = new ContentDataRepository(new CrowledContentDataContextFactory());

               crowledContentDataRepository.Save(parrentNode, "ContentsDb_AL");
              // crowledContentDataRepository.Save(new LawCategory { Description = "description", NSMICode = "12334", RelatedIntents = new[] { new LawCategory { } } }, "ContentsDb_AL");

                //find h1 and take title, then create Intent and save it
                foreach (var line in lines)
                {
                    if (!line.Contains("<td><a href=")) continue;

                    var intent = GetTopicFromLine(line, "<a href=\"", parrentNode);

                    if (intent != null && !trackIntents.ContainsKey(intent.Title))
                    {
                        trackIntents.Add(intent.Title, intent);
                    }
                }


                return trackIntents.Values.ToList();
            }

            private static string RemoveHTMLComments(string input)
            {
                string output = string.Empty;
                string[] temp = System.Text.RegularExpressions.Regex.Split(input, "<!--");
                foreach (string s in temp)
                {
                    string str = string.Empty;
                    if (!s.Contains("-->"))
                    {
                        str = s;
                    }
                    else
                    {
                        str = s.Substring(s.IndexOf("-->") + 3);
                    }
                    if (str.Trim() != string.Empty)
                    {
                        output = output + str.Trim();
                    }
                }
                return output;
            }
            private static Intent GetTopicFromLine(string line, string htmlBeg,Intent parentIntent)
            {
                var idx = line.IndexOf(htmlBeg);

                if (-1 == idx) return null;

                var parsedLine = line.Substring(idx);
                parsedLine = parsedLine.Replace(htmlBeg, "");

                idx = parsedLine.IndexOf("</a>");

                parsedLine = parsedLine.Substring(0, idx);

                idx = parsedLine.IndexOf("\"");
                var url = s_statelhUrlBase + parsedLine.Substring(0, idx);

                idx = parsedLine.IndexOf(">");
                var name = NormalizeName(parsedLine.Substring(idx + 1));

                return new Intent
                {
                    Url = url,
                    Title = name,
                    ParentIntent =parentIntent,
                    ParentId = parentIntent.Id
                };
            }

            //public class Topic
            //{
            //    public string Url { get; set; }
            //    public string Name { get; set; }
            //    public List<SubTopics.Topic> SubTopics { get; set; }
            //}
        }

        #endregion
    }
}
