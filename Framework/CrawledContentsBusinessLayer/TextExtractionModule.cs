using ContentsExtractionApi.Models;
using CrawledContentsBusinessLayer.Model.TextExtraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CrawledContentsBusinessLayer
{

    public class TextExtractionModule
    {

        static async Task<string> getTextFromOCR(byte[] imageStream)
        {
            try
            {
                //stub result
                //  return "this is stubbed content";
                var client = new HttpClient();

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "cb9bc1c228e24015aac83b83eaf5f35c");

                // Request parameters and UR
                string requestParameters = "handwriting=false";
                string uri = "https://westus.api.cognitive.microsoft.com/vision/v1.0/recognizeText?" + requestParameters;

                HttpResponseMessage response;

                // Request body. Try this sample with a locally stored JPEG image.
                //byte[] byteData = GetImageAsByteArray(imageFilePath);
                //byte[] byteData = test;
                using (var content = new ByteArrayContent(imageStream))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json" and "multipart/form-data".
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = client.PostAsync(uri, content).Result;
                }
                var jsonString = await response.Content.ReadAsStringAsync();
                return jsonString;


            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static ExtractedContent ExtractTextContent(byte[] contentBytes)
        {
            try
            {
                //Get text from OCR
                var text = getTextFromOCR(contentBytes).Result;
                //Parse the json to "PageJSONObject" instance
                var parsedObject = JsonConvert.DeserializeObject<PageJSONObject>(text);

                // parsed word collector
                StringBuilder extractedTexts = new StringBuilder();

                parsedObject?.regions.ForEach(region => region?.lines.ForEach(line => line?.words.ForEach(word => extractedTexts.Append(word.text + " "))));
                return new ExtractedContent
                {
                    Resource = "under construction",
                    Summary = extractedTexts.ToString().TrimEnd(' ')
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public static IEnumerable<string> GetPhrasesFromText(string document)
        {
            try
            {
                var json = getPhrasesFromAnalytics(document).Result;
                JObject obj = JObject.Parse(json);

                IEnumerable<string> phrases = obj.SelectToken("$.documents..keyPhrases").Children().Values<string>();
                return phrases;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public static List<string> ExtractTextUsingLinguisticApi(string sentence)
        {
           try
            {
                var result = new List<string>();
                var client = new HttpClient();
                string linguisticsAnalyticsKey = "b1ca157753ff42c68410678871b584cd";

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", linguisticsAnalyticsKey);

                var uri = "https://westus.api.cognitive.microsoft.com/linguistics/v1.0/analyze";

                HttpResponseMessage response;

                string reqBody = "{\"language\" : \"en\",\"analyzerIds\" : [\"4fa79af1-f22c-408d-98bb-b7d7aeef7f04\", \"22a6b758-420f-4745-8a3c-46835a67c0d2\"], \"text\" : \"" + sentence + "\" }";

                using (var content = new StringContent(reqBody))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = client.PostAsync(uri, content).Result;
                }
                var jsonString = response.Content.ReadAsStringAsync().Result;
               // var result_obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LinguisticAnalyzerResultItem>>(jsonString);

                var index = -1;

                //Tobe Improved
                do
                {
                    index = jsonString.IndexOf("result");
                    jsonString = jsonString.Remove(0, index + 6);
                }
                while (index != -1);

                index = jsonString.IndexOf("NN");
                while (index != -1)
                {
                    
                    
                    jsonString = jsonString.Remove(0, index + 2);
                    var delimiterIndex = jsonString.IndexOf(")");
                    result.Add(jsonString.Substring(0, delimiterIndex).Trim());
                    index = jsonString.IndexOf("NN");
                }
                
                return result;
            }
            catch (Exception e)
            {
                //Log Exception
                return null;
            }


        }

        public static string GetIntentFromLuisApi(string sentence)
        {
            try
            {
              
                var uri = string.Format("https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/d70ddc6f-ccb7-4221-ad45-a89458ce02b5?subscription-key=cc7f076047764c8bb37fec016887db9e&timezoneOffset=0&verbose=true&q={0}", sentence);

                string result;
                using (var client = new HttpClient())
                {
                    using (var r =  client.GetAsync(new Uri(uri)).Result)
                    {
                        result =  r.Content.ReadAsStringAsync().Result;                        
                    }
                }
                JObject jObject = JObject.Parse(result);
                JToken jTopScoringIntent = jObject["topScoringIntent"];
                var intent = (string)jTopScoringIntent["intent"];
                
                return intent;
            }
            catch (Exception e)
            {
                return e.Message;
            }


        }
        static async Task<string> getPhrasesFromAnalytics(string document)
        {
            try
            {
                var client = new HttpClient();
                string textAnalyticsKey = "425ec82bdf714befa721ae2633a9e3c1";

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", textAnalyticsKey);

                var uri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/keyPhrases";

                HttpResponseMessage response;

                // Request body
                //byte[] byteData = Encoding.UTF8.GetBytes("{body}");

                string reqBody = "{\"documents\": [{\"language\": \"en\",\"id\": \"1\",\"text\":\"" + document + "\"}]}";

                using (var content = new StringContent(reqBody))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = client.PostAsync(uri, content).Result;
                }
                var jsonString = await response.Content.ReadAsStringAsync();
                return jsonString;


            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }


    public class LinguisticAnalyzerResultItem
    {
       public string analyzerId { get; set; }
       public List<string[]> result { get; set; }
    }
}