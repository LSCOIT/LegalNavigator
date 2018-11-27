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

        [DefaultValue("")]
        [JsonProperty(PropertyName = "contactUs")]
        public Contact ContactUs { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "mediaInquiries")]
        public Contact MediaInquiries { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "inTheNews")]
        public InTheNews InTheNews { get; set; }
    }

    public class Mission : TitleDescription
    {
        public Mission()
        {
            Sponsors = new List<Sponsors>();
        }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "sponsors")]
        public List<Sponsors> Sponsors { get; set; }
    }

    public class Sponsors 
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "altText")]
        public string AltText { get; set; }

    }

    public class Service : TitleDescription
    {
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

    public class PrivacyPromise : TitleDescription
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "privacyPromiseButton")]
        public ButtonStaticContent PrivacyPromiseButton { get; set; }
    }

    public class Contact : TitleDescription
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }

    public class InTheNews : TitleDescription
    {
        public InTheNews()
        {
            News = new List<News>();
        }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "news")]
        public List<News> News { get; set; }
    }

    public class News : TitleDescription
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "url")]
        public string Link { get; set; }
    }

    public class TitleDescription
    {

        [DefaultValue("")]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}