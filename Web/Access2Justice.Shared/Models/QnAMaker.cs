using Newtonsoft.Json;
using System.Collections.Generic;

namespace Access2Justice.Shared.Models
{
    public class QnAMaker
    {
        public QnAMaker()
        {
            Questions = new List<string>();
        }

        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; }
        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }
        [JsonProperty(PropertyName = "questions")]
        public IEnumerable<string> Questions { get; set; }
    }

    public class QnAMakerResult
    {
        public QnAMakerResult()
        {
            Answers = new List<QnAMaker>();
        }

        [JsonProperty(PropertyName = "answers")]
        public IEnumerable<QnAMaker> Answers { get; set; }
    }
}
