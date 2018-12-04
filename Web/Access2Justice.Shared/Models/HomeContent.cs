using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace Access2Justice.Shared.Models
{
    public class HomeContent: NameLocation
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "hero")]
        public HeroContent Hero { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "guidedAssistantOverview")]
        public GuidedAssistantContent GuidedAssistantOverview { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "topicAndResources")]
        public TopicAndResourcesContent TopicAndResources { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "carousel")]
        public CarouselContent Carousel { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "sponsorOverview")]
        public SponsorsContent SponsorsContent { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "privacy")]
        public PrivacyContent Privacy { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "helpText")]
        public HelpText HelpText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "organizationalUnit")]
        public string OrganizationalUnit { get; set; }
    }

    public class HeroContent
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public Description Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }
    }

    public class GuidedAssistantContent
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public GADescription Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "button")]
        public ButtonStaticContent Button { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image ImageUrl { get; set; }
    }

    public class TopicAndResourcesContent
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "button")]
        public ButtonStaticContent Button { get; set; }
    }

    public class CarouselContent
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "slides")]
        public IEnumerable<Slides> Overviewdetails { get; set; }
    }

    public class SponsorsContent
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "sponsors")]
        public IEnumerable<Image> Sponsors { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "button")]
        public ButtonStaticContent Button { get; set; }
    }

    public class PrivacyContent
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "button")]
        public ButtonStaticContent Button { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }
    }

    public class HelpText
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "beginningText")]
        public string BeginningText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "phoneNumber")]
        public string PhoneNumber { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "endingText")]
        public string EndingText { get; set; }
    }

    public class Description
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "textWithLink")]
        public TextWithLink TextWithLink { get; set; }
    }

    public class TextWithLink
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "urlText")]
        public string UrlsText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "url")]
        public string Urls { get; set; }
    }

    public class Image
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "altText")]
        public string AltText { get; set; }
    }

    public class GADescription
    {
        public GADescription()
        {
            GADescriptionDetails = new List<GADescriptionDetails>();
        }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "steps")]
        public List<GADescriptionDetails> GADescriptionDetails { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "textWithLink")]
        public TextWithLink TextWithLink { get; set; }
    }

    public class GADescriptionDetails
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "order")]
        public int Order { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }

    public class ButtonStaticContent
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "buttonText")]
        public string ButtonText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "buttonAltText")]
        public string ButtonAltText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "buttonLink")]
        public string ButtonLink { get; set; }
    }

    public class Slides
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "quote")]
        public string Quote { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }
    }

    public class PageContentRequest
    {
        public string Name { get; set; }
        public Location Location { get; set; }
    }

    public class NameLocation
    {
        public NameLocation()
        {
            Location = new List<Location>();
        }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "location")]
        public List<Location> Location { get; set; }
    }
}