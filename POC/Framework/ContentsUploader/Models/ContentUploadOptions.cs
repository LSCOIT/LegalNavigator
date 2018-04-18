using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentsUploader.Models
{
    public class ContentUploadOptions
    {
        [Display(Name = "Selected Option")]
        public UploadOptios SelectedOption { get; set; }
        public IEnumerable<SelectListItem> UploadOptions
        {
            get
            {
                return new List<SelectListItem>{
                    new SelectListItem
                      {
                        Text = "Curated Experience",
                        Value = UploadOptios.CuratedExperience.ToString(),
                      },
                    new SelectListItem
                     {
                       Text = "Questions and Answers",
                       Value = UploadOptios.QA.ToString(),
                     },
                    new SelectListItem
                    {
                        Text = "Video",
                        Value = UploadOptios.Video.ToString(),
                    }
                 };
            }
        }
    }

    public enum UploadOptios
    {
        CuratedExperience,
        Video,
        QA
    }
}