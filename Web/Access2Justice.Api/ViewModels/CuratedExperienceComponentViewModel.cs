using Access2Justice.Shared.Models.CuratedExperience;
using Newtonsoft.Json;
using System;

namespace Access2Justice.Api.ViewModels
{
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