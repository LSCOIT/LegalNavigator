using Newtonsoft.Json;

namespace Access2Justice.Shared.Models.CuratedExperience
{
    public enum CuratedExperienceQuestionType
    {
        [JsonProperty(PropertyName = "text")]
        Text,
        [JsonProperty(PropertyName = "richText")]
        RichText,
        [JsonProperty(PropertyName = "list")]
        List,
        [JsonProperty(PropertyName = "number")]
        Number,
        [JsonProperty(PropertyName = "currency")]
        Currency,
        [JsonProperty(PropertyName = "ssn")]
        Ssn,
        [JsonProperty(PropertyName = "phone")]
        Phone,
        [JsonProperty(PropertyName = "zipCode")]
        ZipCode,
        [JsonProperty(PropertyName = "date")]
        Date,
        [JsonProperty(PropertyName = "radioButton")]
        RadioButton,
        [JsonProperty(PropertyName = "checkBox")]
        CheckBox
    }
}
