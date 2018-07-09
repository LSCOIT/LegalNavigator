using Newtonsoft.Json;
using System.Collections.Generic;

namespace Access2Justice.Api.ViewModels
{
    public class CuratedExperienceSurveyViewModel
    {
        // Todo:@Alaa update this based on the new schema structure
        [JsonProperty(PropertyName = "questionId")]
        public string QuestionId { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "questionType")]
        public string QuestionType { get; set; }
        [JsonProperty(PropertyName = "userAnswer")]
        public string UserAnswer { get; set; }
        [JsonProperty(PropertyName = "choices")]
        public List<ChoiceViewModel> ChoicesViewModel { get; set; }

        public CuratedExperienceSurveyViewModel()
        {
            ChoicesViewModel = new List<ChoiceViewModel>();
        }
    }

    public class ChoiceViewModel
    {
        [JsonProperty(PropertyName = "choiceId")]
        public string ChoiceId { get; set; }
        [JsonProperty(PropertyName = "choiceText")]
        public string ChoiceText { get; set; }
    }
}