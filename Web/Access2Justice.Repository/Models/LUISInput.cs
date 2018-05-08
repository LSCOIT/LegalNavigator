namespace Access2Justice.Repository
{
    using System.ComponentModel.DataAnnotations;

    public class LUISInput
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
