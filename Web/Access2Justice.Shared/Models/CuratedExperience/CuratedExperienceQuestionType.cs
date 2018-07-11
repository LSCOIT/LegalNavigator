using Newtonsoft.Json;

namespace Access2Justice.Shared.Models.CuratedExperience
{
    public enum CuratedExperienceQuestionType
    {
        [JsonProperty(PropertyName = "bool")]
        Bool,
        [JsonProperty(PropertyName = "list")]
        List,
        [JsonProperty(PropertyName = "textbox")]
        Textbox
    }
}
