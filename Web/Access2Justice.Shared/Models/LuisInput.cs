using Access2Justice.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Access2Justice.Shared
{
    public class LuisInput
    {
        [Required]
        public string Sentence { get; set; }

        public Location Location { get; set; }

        public string TranslateFrom { get; set; }

        public string TranslateTo { get; set; }

        public string LuisTopScoringIntent { get; set; }

        public bool IsFromCuratedExperience { get; set; } = false;
        
    }
}
