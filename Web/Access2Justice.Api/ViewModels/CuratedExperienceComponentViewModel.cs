using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Api.ViewModels
{
    public class CuratedExperienceComponentViewModel
    {
        public CuratedExperienceComponentViewModel()
        {
            Fields = new List<Field>();
        }
        public Guid CuratedExperienceId { get; set; }
        public Guid ComponentId { get; set; }
        public IList<Field> Fields { get; set; }
    }

    public class Field
    {
        public string FieldId { get; set; }
        public string Value { get; set; }
    }
}