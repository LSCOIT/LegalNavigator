using Newtonsoft.Json;

namespace Access2Justice.Shared.Models.CuratedExperience
{
    public enum CuratedExperienceQuestionType
    {
        [JsonProperty(PropertyName = "text")]
        Text,
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

        // Todo: these data types are from A2J Authors but we don't have equivalent 
        // for them of at this point. We should find their use case or remove them if
        // we could manage without them.
        // Text (Long)
        // Check Box (None of the Above)
    }
}
