﻿using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Text;
using System.ComponentModel;

namespace Access2Justice.Shared.Models
{
    public class AboutContent: NameLocation
    {
        [JsonProperty(PropertyName = "aboutImage")]
        public Image AboutImage { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "mission")]
        public Mission Mission { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "service")]
        public Service Service { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "privacyPromise")]
        public PrivacyPromise PrivacyPromise { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "organizationalUnit")]
        public string OrganizationalUnit { get; set; }
    }

    public class Mission
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }
    }

    public class Service
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "guidedAssistantButton")]
        public ButtonStaticContent GuidedAssistantButton { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "topicsAndResourcesButton")]
        public ButtonStaticContent TopicsAndResourcesButton { get; set; }
    }

    public class PrivacyPromise
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "privacyPromiseButton")]
        public ButtonStaticContent PrivacyPromiseButton { get; set; }
    }
}