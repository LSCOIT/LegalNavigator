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
                string linguisticsAnalyticsKey = "7c6f2b8697fd4e4a91a07ea75bfbdfbb";
                 //"0055fe14a5854f90b6cb46954ce2b64b";//"b1ca157753ff42c68410678871b584cd";

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", linguisticsAnalyticsKey);

                var uri = "https://westus.api.cognitive.microsoft.com/linguistics/v1.0/analyze";

                HttpResponseMessage response;

               // string reqBody = "{\"language\" : \"en\",\"analyzerIds\" : [\"22a6b758-420f-4745-8a3c-46835a67c0d2\", \"d70ddc6f-ccb7-4221-ad45-a89458ce02b5\"], \"text\" : \"" + sentence + "\" }";
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

        public static IntentWithScore GetIntentFromLuisApi(string sentence)
        {
            try
            {
           
                var uri = string.Format("https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/d70ddc6f-ccb7-4221-ad45-a89458ce02b5?subscription-key=0055fe14a5854f90b6cb46954ce2b64b&timezoneOffset=0&verbose=true&q={0}", sentence);

                string result;
                using (var client = new HttpClient())
                {
                    using (var r =  client.GetAsync(new Uri(uri)).Result)
                    {
                        result =  r.Content.ReadAsStringAsync().Result;                        
                    }
                }
                JObject jObject = JObject.Parse(result);
                jObject.Children();
                JToken jTopScoringIntent = jObject["topScoringIntent"];
                var TopTwoIntentsOtherthanTopScoringIntent = jObject["intents"].Skip(1).Take(6).Select(x => x["intent"]).ToArray();
                return new IntentWithScore
                {
                    IsSuccessful =true,
                    TopScoringIntent = (string)jTopScoringIntent["intent"],
                    Score = (decimal)jTopScoringIntent["score"],
                    TopSixIntents =new [] { ((JValue)TopTwoIntentsOtherthanTopScoringIntent[0]).Value.ToString() ,
                                            ((JValue)TopTwoIntentsOtherthanTopScoringIntent[1]).Value.ToString() ,
                                            ((JValue)TopTwoIntentsOtherthanTopScoringIntent[2]).Value.ToString() ,"None of these",
                                            ((JValue)TopTwoIntentsOtherthanTopScoringIntent[3]).Value.ToString() ,                                           
                                            ((JValue)TopTwoIntentsOtherthanTopScoringIntent[4]).Value.ToString() ,
                                            ((JValue)TopTwoIntentsOtherthanTopScoringIntent[5]).Value.ToString(),"None of these"
                                          }
                    
                };

               // var intent = (string)jTopScoringIntent["intent"];
               // var score= (decimal)jTopScoringIntent["score"];

               // return intentWithScore;
            }
            catch (Exception e)
            {
                return new IntentWithScore
                {
                    IsSuccessful =false,
                    ErrorMessage = e.Message
                };

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

    public class IntentWithScore
    {
        /// <summary>
        /// Top Scoring Intent
        /// </summary>
        public string TopScoringIntent { get; set; }

        /// <summary>
        /// Score weight of the intent with respect to the user input
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        ///  Top two intents other than the top Scoring intent
        /// </summary>
        public string[] TopSixIntents { get; set; }

        /// <summary>
        /// Error message in case error happens before completion of result parsing
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Indicator of whether the parsing is successful or not
        /// </summary>
        public bool IsSuccessful { get; set; }
    }
}