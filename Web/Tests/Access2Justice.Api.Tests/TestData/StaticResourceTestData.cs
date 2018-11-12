using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Api.Tests.TestData
{
    class StaticResourceTestData
    {
        public static JArray homePageData =
                  JArray.Parse(@"[{
                        'id': 'HomePage',
                        'hero': {
                            'overview': 'Get help with your legal questions in [state]',
                            'description': 'We will always protect your privacy. Read our ',
                            'routerText': 'Privacy promise.',
                            'routerLink': 'PrivacyPromise.link',
                            'image': {
                                'id': '',
                                'path': '/assets/images/primary-illustrations/hawaii.svg'
                            }
                        },
                        'guidedAssistant': {
                            'overview': 'Need a plan of action for your legal issue Our Guided Assistant can help.',
                            'description': {
                                'steps': [
                                    {
                                        'order': '1',
                                        'descritpion': 'Answer a few questions to help us understand your needs.'
                                    }
                                ],
                                'paragraph': {
                                    'text': 'Free and no account necessary. We never share any information you provide.',
                                    'routerText': 'Privacy Promise.',
                                    'routerLink': 'PrivacyPromise.link'
                                }
                            },
                            'buttonText': 'Start the Guided Assistant',
                            'buttonLink': '',
                            'image': {
                                'id': '',
                                'path': '/assets/images/secondary-illustrations/guided_assistant.svg'
                            }
                        },
                        'topicAndResources': {
                            'overview': 'More Information, Videos, and Links to Resources by Topic',
                            'buttonText': 'See More Topics',
                            'buttonLink': '/topics'
                        },
                        'carousel': {
                            'overview': [
                                {
                                    'description': 'Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel..',
                                    'byName': 'Robbie',
                                    'location': 'Anchorage, AK',
                                    'image': {
                                        'id': '',
                                        'path': '/assets/images/secondary-illustrations/guided_assistant.svg'
                                    }
                                },
                                {
                                    'description': 'Ut fusce varius nisl ac ipsum gravida vel pretium tellus tincidunt integer eu augue augue nunc elit dolor, luctus placerat.',
                                    'byName': 'Robbie',
                                    'location': 'Anchorage, AK',
                                    'image': {
                                        'id': '',
                                        'path': '/assets/images/secondary-illustrations/guided_assistant.svg'
                                    }
                                },
                                {
                                    'description': 'This site is here to provide equal access to legal information and resources for everyone. Legal disclaimer lorem ipsum dolor amet. It is brought to you by partnership of these nonproffit organizations.',
                                    'byName': 'Robbie',
                                    'location': 'Anchorage, AK',
                                    'image': {
                                        'id': '',
                                        'path': '/assets/images/secondary-illustrations/guided_assistant.svg'
                                    }
                                }
                            ]
                        },
                        'information': {
                            'overview': 'Information you can trust',
                            'description': 'This site is here to provide equal access to legal information and resources for everyone. Legal disclaimer lorem ipsum dolor amet. It is brought to you by partnership of these nonproffit organizations',
                            'buttonText': 'Learn more',
                            'buttonLink': 'url'
                        },
                        'privacy': {
                            'overview': 'Our Privacy Promise',
                            'description': 'We value your privacy and will never share your personal information with anyone without your lorem ipsum dolor sit amet.',
                            'buttonText': 'Learn more',
                            'buttonLink': 'url',
                            'image': {
                                'id': '',
                                'path': '/assets/images/secondary-illustrations/privacy_promise.svg'
                            }
                        },
                        '_rid': 'mwoSAIKx1tkBAAAAAAAAAA==',
                        '_self': 'dbs/mwoSAA==/colls/mwoSAIKx1tk=/docs/mwoSAIKx1tkBAAAAAAAAAA==/',
                        '_etag': '\'fc005f34-0000-0000-0000-5b5ac3b50000\'',
                        '_attachments': 'attachments/',
                        '_ts': 1532674997
                    }]");

        public static string updatedStaticNavigationContent = " {\"name\": \"Navigation\"," +
            "\"location\": [ { \"state\": \"Hawaii\" } ]," +
            "\"organizationalUnit\": \"Hawaii\"," +
            "\"locationNavContent\": {\"text\": \"Please Select Location\"," +
            "\"altText\": \"Please Select Location\"," +
            "\"button\": {\"buttonText\": \"Change\"," +
            "\"buttonAltText\": \"Change\"," +
            "\"buttonLink\": \"asd\"}}," +
            "\"id\":\"e02ba613-5005-c024-98ec-a092857068ba\"}";


        public static JArray staticNavigationContent = JArray.Parse(@"[  {
    'name': 'Navigation',
    'location': [ { 'state': 'Hawaii' } ],
    'organizationalUnit': 'Hawaii',
    'locationNavContent': {
      'text': 'Please Select Location',
      'altText': 'Please Select Location',
      'button': {
        'buttonText': 'Change',
        'buttonAltText': 'Change',
        'buttonLink': ''
      }},'id': 'e02ba613-5005-c024-98ec-a092857068ba'
  }]");


        public static IEnumerable<object[]> UpsertNavigationContent()
        {
            yield return new object[] { new Navigation { Location = new List<Location>{new Location(){ State="Hawaii"} }, Name = "Navigation", OrganizationalUnit = "Hawaii" },
             JArray.Parse(@"[  {
    'name': 'Navigation',
    'location': [ { 'state': 'Hawaii' } ],
    'organizationalUnit': 'Hawaii',
    'locationNavContent': {
      'text': 'Please Select Location',
      'altText': 'Please Select Location',
      'button': {
        'buttonText': 'Change',
        'buttonAltText': 'Change',
        'buttonLink': ''
      }},'id': 'e02ba613-5005-c024-98ec-a092857068ba'
  }]") };
        }
    }

}