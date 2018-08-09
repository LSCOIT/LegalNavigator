using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Shared.Models
{
    public class CuratedExperienceAnswers
    {
        [JsonProperty(PropertyName = "id")]
        public Guid AnswersDocId { get; set; }

        [JsonProperty(PropertyName = "curatedExperienceId")]
        public Guid CuratedExperienceId { get; set; }

        [JsonProperty(PropertyName = "answers")]
        public IList<Answer> Answers { get; set; }

        public CuratedExperienceAnswers()
        {
            Answers = new List<Answer>();
        }
    }

    public class Answer
    {
        [JsonProperty(PropertyName = "answerButtonId")]
        public Guid AnswerButtonId { get; set; }

        [JsonProperty(PropertyName = "answerFields")]
        public IList<AnswerField> AnswerFields { get; set; }
        public Answer()
        {
            AnswerFields = new List<AnswerField>();
        }
    }

    public class AnswerField
    {
        [JsonProperty(PropertyName = "fieldId")]
        public string FieldId { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}