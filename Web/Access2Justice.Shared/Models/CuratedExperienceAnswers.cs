using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Shared.Models
{
    public class CuratedExperienceAnswers
    {
        public CuratedExperienceAnswers()
        {
            ButtonComponents = new List<ButtonComponent>();
            FieldComponents = new List<FieldComponent>();
        }

        [JsonProperty(PropertyName = "id")]
        public Guid AnswersDocId { get; set; }

        [JsonProperty(PropertyName = "curatedExperienceId")]
        public Guid CuratedExperienceId { get; set; }

        [JsonProperty(PropertyName = "buttonComponents")]
        public List<ButtonComponent> ButtonComponents { get; set; }

        [JsonProperty(PropertyName = "fieldComponents")]
        public List<FieldComponent> FieldComponents { get; set; }
    }

    public class ButtonComponent
    {
        [JsonProperty(PropertyName = "buttonId")]
        public Guid ButtonId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "codeBefore")]
        public string CodeBefore { get; set; }

        [JsonProperty(PropertyName = "codeAfter")]
        public string CodeAfter { get; set; }
    }

    public class FieldComponent
    {
        public FieldComponent()
        {
            Fields = new List<AnswerField>();
        }

        [JsonProperty(PropertyName = "fields")]
        public List<AnswerField> Fields { get; set; }

        [JsonProperty(PropertyName = "codeBefore")]
        public string CodeBefore { get; set; }

        [JsonProperty(PropertyName = "codeAfter")]
        public string CodeAfter { get; set; }
    }

    public class AnswerField
    {
        [JsonProperty(PropertyName = "fieldId")]
        public Guid FieldId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}