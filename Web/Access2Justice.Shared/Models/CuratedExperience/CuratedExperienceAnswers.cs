using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Shared.Models.CuratedExperience
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
        [JsonProperty(PropertyName = "clickedButtonId")]
        public Guid ClickedButtonId { get; set; }
        [JsonProperty(PropertyName = "selectedItemsIds")]
        public IList<Guid> SelectedItemsIds { get; set; }
        [JsonProperty(PropertyName = "filledInTexts")]
        public IList<FilledInText> FilledInTexts { get; set; }
        public Answer()
        {
            SelectedItemsIds = new List<Guid>();
            FilledInTexts = new List<FilledInText>();
        }
    }

    public class FilledInText
    {
        [JsonProperty(PropertyName = "fieldId")]
        public string FieldId { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}