using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Api.ViewModels
{
    public class CuratedExperienceComponentViewModel
    {
        public Guid CuratedExperienceId { get; set; }
        public Guid ButtonId { get; set; }
        public IList<Field> Fields { get; set; }
        public CuratedExperienceComponentViewModel()
        {
            Fields = new List<Field>();
        }
    }

    public class Field
    {
        public string FieldId { get; set; }
        public string Value { get; set; }
    }
}