using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentsUploader.Models
{
    public class LanguageTranslation
    {
        
        [Required]
        [Display(Name = "Language")]
        public string SupportedLanguage { get; set; }
        public IEnumerable<SelectListItem> SupportedLanguages { get; set; }
     
    }
}