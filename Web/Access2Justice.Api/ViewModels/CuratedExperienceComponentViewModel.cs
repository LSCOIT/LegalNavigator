using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System;
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
    }
}