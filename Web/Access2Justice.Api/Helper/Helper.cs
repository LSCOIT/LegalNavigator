using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Access2Justice.Api
{
    public class Helper
    {
        public static IntentWithScore GetIntentFromLuisApi(string sentence)
        {
            try
            {

                var uri = string.Format("https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/9e4e4260-0e40-4904-9481-f0e748cf4da2?subscription-key=66ecb973fd46463bab121fbb572c8072&verbose=true&timezoneOffset=0&q={0}", sentence);

                string result;
                using (var client = new HttpClient())
                {
                    using (var r = client.GetAsync(new Uri(uri)).Result)
                    {
                        result = r.Content.ReadAsStringAsync().Result;
                    }
                }
                JObject jObject = JObject.Parse(result);
                jObject.Children();
                JToken jTopScoringIntent = jObject["topScoringIntent"];
                var TopTwoIntentsOtherthanTopScoringIntent = jObject["intents"].Skip(1).Take(6).Select(x => x["intent"]).ToArray();
                return new IntentWithScore
                {
                    IsSuccessful = true,
                    TopScoringIntent = (string)jTopScoringIntent["intent"],
                    Score = (decimal)jTopScoringIntent["score"],
                    TopSixIntents = new[] { ((JValue)TopTwoIntentsOtherthanTopScoringIntent[0]).Value.ToString() ,
                                            ((JValue)TopTwoIntentsOtherthanTopScoringIntent[1]).Value.ToString()
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
                    IsSuccessful = false,
                    ErrorMessage = e.Message
                };

            }


        }

        public string GetChatResponse(string input)
        {
            var abuseTypes = new List<LawTypes>();
            switch (input)
            {
                case "ChildAbuse":                    
                    return JsonConvert.SerializeObject(new ChildAbuse { Description = "Ohh..! can you please let me know you're age.", AbuseTypes = abuseTypes });
                case "Greetings":
                    return JsonConvert.SerializeObject(new ChildAbuse { Description = "Hi, I'm Virutal Legal Agent.Describe your problem and I'll look for the best solution and help.", AbuseTypes = abuseTypes });
                case "Age":
                    return JsonConverter();
                case "Above18Age":
                    return JsonConverter2();
                //return JsonConvert.SerializeObject(new ChildAbuse { Description = " This comes under Abuse & Harassment, please select specific type of abuse or harassment cases", AbuseTypes = abuseTypes });
                default:                    
                    return JsonConvert.SerializeObject(new ChildAbuse { Description = "Glad to talk to you. Could you describe your problem in detail?", AbuseTypes = abuseTypes });
            }
        }

        public string JsonConverter()
        {
            var abuseTypes = new List<LawTypes>();
            abuseTypes.Add( new LawTypes { LawName ="Physical abuse", LawDetails = "" });
            abuseTypes.Add(new LawTypes { LawName = "Sexual abuse", LawDetails = "" });
            abuseTypes.Add(new LawTypes { LawName = "Psychological abuse", LawDetails = "" });
            abuseTypes.Add(new LawTypes { LawName = "Neglect", LawDetails = "" });            
            var childAbuse = new ChildAbuse { Description = "This comes under Child Abuse, please select specfic types of Child Abuse.", AbuseTypes = abuseTypes };
            return JsonConvert.SerializeObject(childAbuse);
        }

        public string JsonConverter2()
        {
            var abuseTypes = new List<LawTypes>();            
            abuseTypes.Add(new LawTypes { LawName = "Domestic Violence", LawDetails = "Domestic violence is abuse or threats of abuse when the person being abused and the abusive person are: Married or registered domestic partners, Divorced or separated, Dating or used to date, Living together or used to live together(but more than just roommates), OR Closely related(like parent, child, brother, sister, grandmother, grandfather, in -law)." });
            abuseTypes.Add(new LawTypes { LawName = "Elder or Dependent Adult Abuse", LawDetails = "Abuse of an elder or a dependent adult is abuse of: Someone 65 years old or older; or A dependent adult, who is someone between 18 and 64 that has certain mental or physical disabilities that keep him or her from being able to do normal activities or protect himself or herself." });
            abuseTypes.Add(new LawTypes { LawName = "Civil Harassment", LawDetails = "In general, civil harassment is abuse, threats of abuse, stalking, sexual assault, or serious harassment by someone you have not dated and do NOT have a close family relationship with, like a neighbor, a roommate, or a friend (that you have never dated). It is also civil harassment if the abuse is from a family member that is not included in the list under domestic violence. So, for example, if the abuse is from an uncle or aunt, a niece or nephew, or a cousin, it is considered civil harassment and NOT domestic violence" });
            abuseTypes.Add(new LawTypes { LawName = "Workplace Violence", LawDetails = "For a workplace violence situation, the harassment is defined in the same way as for civil harassment. The difference is that the harassment happens primarily at work AND it is the employer of the harassed employee who asks for protection for the employee (and, if necessary, for the employee’s family)." });
            var childAbuse = new ChildAbuse { Description = "This comes under Abuse & Harassment, please select specific types of abuse or harassment cases", AbuseTypes = abuseTypes };
            return JsonConvert.SerializeObject(childAbuse);
        }

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

    public class ChildAbuse
    {
        public string Description { get; set; }

        public List<LawTypes> AbuseTypes { get; set; }

    }

    public class LawTypes
    {
        public string LawName { get; set; }

        public string LawDetails { get; set; }
    }

}
