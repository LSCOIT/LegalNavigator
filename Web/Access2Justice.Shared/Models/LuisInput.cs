// todo:@alaa remove file and related references?


using System.ComponentModel.DataAnnotations;

namespace Access2Justice.Shared
{
    public class LuisInput
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Sentence { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        public string State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ZipCodeValidation]
        public string ZipCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TranslateFrom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TranslateTo { get; set; }
    }
}
