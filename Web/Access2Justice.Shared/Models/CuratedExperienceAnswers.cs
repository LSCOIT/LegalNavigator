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
        public List<Answer> Answers { get; set; }

        public CuratedExperienceAnswers()
        {
            Answers = new List<Answer>();
        }
    }

    public class Answer
    {
        [JsonProperty(PropertyName = "answerButtons")]
        public List<AnswerButton> AnswerButtons { get; set; }

        [JsonProperty(PropertyName = "answerFields")]
        public List<AnswerField> AnswerFields { get; set; }

        [JsonProperty(PropertyName = "codeBefore")]
        public string CodeBefore { get; set; }

        [JsonProperty(PropertyName = "codeAfter")]
        public string CodeAfter { get; set; }

        public Answer()
        {
            AnswerButtons = new List<AnswerButton>();
            AnswerFields = new List<AnswerField>();
        }
    }

    public class AnswerButton
    {
        [JsonProperty(PropertyName = "buttonId")]
        public Guid ButtonId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    public class AnswerField
    {
        [JsonProperty(PropertyName = "fieldId")]
        public string FieldId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}