using Microsoft.AspNetCore.JsonPatch.Operations;
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
    public class Navigation: NameLocation
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "language")]
        public Language Language { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "locationNavContent")]
        public HeaderLocation LocationNav { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "privacyPromise")]
        public ButtonImage PrivacyPromise { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "helpAndFAQ")]
        public ButtonImage HelpAndFAQ { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "login")]
        public ButtonImage Login { get; set; }

        [JsonProperty(PropertyName = "logo")]
        public Logo Logo { get; set; }
        
        [DefaultValue("")]
        [JsonProperty(PropertyName = "home")]
        public ButtonStaticContent Home { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "guidedAssistant")]
        public ButtonStaticContent GuidedAssistant { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "topicAndResources")]
        public ButtonStaticContent TopicAndResources { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "about")]
        public ButtonStaticContent About { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "search")]
        public ButtonImage Search { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "organizationalUnit")]
        public string OrganizationalUnit { get; set; }
    }

    public class Language
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "button")]
        public ButtonStaticContent Button { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "navigationImage")]
        public Image NavigationImage { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "dropDownImage")]
        public Image DropDownImage { get; set; }
    }

    public class HeaderLocation
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "altText")]
        public string AltText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "button")]
        public ButtonStaticContent Button { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }
    }

    public class ButtonImage
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "button")]
        public ButtonStaticContent Button { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }
    }

    public class Logo
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "firstLogo")]
        public string FirstLogo { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "secondLogo")]
        public string SecondLogo { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
    }
}