using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Access2Justice.Api.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class CuratedExperienceComponentViewModel : CuratedExperienceComponent
    {
        [JsonProperty(PropertyName = "curatedExperienceId")]
        public Guid CuratedExperienceId { get; set; }

        [JsonProperty(PropertyName = "answersDocId")]
        public Guid AnswersDocId { get; set; }

        [JsonProperty(PropertyName = "questionsRemaining")]
        public int QuestionsRemaining { get; set; }

        [JsonProperty(PropertyName = "hasNextAnswers")]
        public bool HasNextAnswers { get; set; }

        [JsonProperty(PropertyName = "hasPreviousAnswers")]
        public bool HasPreviousAnswers { get; set; }

        [JsonProperty(PropertyName = "buttonsSelected", NullValueHandling = NullValueHandling.Ignore)]
        public List<ButtonComponent> ButtonsSelected { get; set; }

        [JsonProperty(PropertyName = "fieldsSelected", NullValueHandling = NullValueHandling.Ignore)]
        public List<FieldComponent> FieldsSelected { get; set; }

        [JsonProperty(PropertyName = "answersId")]
        public Guid AnswersId { get; set; }
    }
}