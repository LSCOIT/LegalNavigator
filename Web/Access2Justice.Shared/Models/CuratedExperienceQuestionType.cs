using Newtonsoft.Json;

namespace Access2Justice.Shared.Models
{
    public enum CuratedExperienceQuestionType
    {
        text,      
        richText,
        list,
        number,
        currency,
        ssn,
        phone,
        zipCode,
        date,
        radioButton,
        checkBox
    }
}
