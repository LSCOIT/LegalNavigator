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
                        'name': 'HomePage',
                        '_rid': 'mwoSAIKx1tkBAAAAAAAAAA==',
                        '_self': 'dbs/mwoSAA==/colls/mwoSAIKx1tk=/docs/mwoSAIKx1tkBAAAAAAAAAA==/',
                        '_etag': '\'fc005f34-0000-0000-0000-5b5ac3b50000\'',
                        '_attachments': 'attachments/',
                        '_ts': 1532674997
                    }]");

        public static string updatedStaticNavigationContent = "{\"name\": \"Navigation\"," +
            "\"location\": [ { \"state\": \"Hawaii\" } ]," +
            "\"organizationalUnit\": \"Hawaii\"," +
            "\"locationNavContent\": {\"text\": \"Please Select Location\"," +
            "\"altText\": \"Please Select Location\"," +
            "\"button\": {\"buttonText\": \"Change\"," +
            "\"buttonAltText\": \"Change\"," +
            "\"buttonLink\": \"asd\"}}," +
            "\"id\":\"e02ba613-5005-c024-98ec-a092857068ba\"," +
            "\"_rid\": \"2NR6AMkFyF8UAAAAAAAAAA==\"," +
            "\"_self\": \"dbs/2NR6AA==/colls/2NR6AMkFyF8=/docs/2NR6AMkFyF8UAAAAAAAAAA==/\"," +
            "\"_etag\": \"8300edf3-0000-0000-0000-5bea47f10000\"," +
            "\"_attachments\": \"attachments/\"," +
            "\"_ts\": 1542080497}";

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
      }},'id': 'e02ba613-5005-c024-98ec-a092857068ba',
 '_rid': '2NR6AMkFyF8UAAAAAAAAAA==',
  '_self': 'dbs/2NR6AA==/colls/2NR6AMkFyF8=/docs/2NR6AMkFyF8UAAAAAAAAAA==/',
  '_etag': '\'8300edf3-0000-0000-0000-5bea47f10000\'',
  '_attachments': 'attachments/',
  '_ts': 1542080497
  }]");

        public static string updatedPrivacyPromiseContent = "{\"name\": \"PrivacyPromisePage\"," +
            "\"location\": [ { \"state\": \"Alaska\" } ]," +
            "\"organizationalUnit\": \"Alaska\"," +
            "\"description\": \"Lorem, ipsum dolor sit amet consectetur adipisicing elit. Asperiores illo totam iste rerum fuga sit facilis modi iusto sapiente saepe tenetur labore eaque esse quod alias maxime ut velit quisquam, doloremque perspiciatis harum omnis. Odit dolorem, reprehenderit cupiditate deleniti amet a aperiam voluptates quam facilis suscipit, vero error magnam voluptatum.\"," +
            "\"image\": {\"source\": \"/static-resource/alaska/assets/images/secondary-illustrations/privacy_promise_dark.svg\"," +
            "\"altText\": \"Privacy Promise Page\"}," +
            "\"details\": [{\"title\": \"Data Collection and Use\"," +
            "\"description\": \"Lorem ipsum dolor sit, amet consectetur adipisicing elit. Adipisci, distinctio possimus neque corrupti veritatis aspernatur odit quae sed voluptas minima, aliquam iste ex deleniti ut aliquid quaerat animi, similique accusamus?\"}]," +
            "\"id\": \"7b58a732-2f77-1db9-dfdf-9798fb564d6c\"}";

        public static JArray staticPrivacyPromiseContent = JArray.Parse(@"[{
    'description': 'Lorem, ipsum dolor sit amet consectetur adipisicing elit. Asperiores illo totam iste rerum fuga sit facilis modi iusto sapiente saepe tenetur labore eaque esse quod alias maxime ut velit quisquam, doloremque perspiciatis harum omnis. Odit dolorem, reprehenderit cupiditate deleniti amet a aperiam voluptates quam facilis suscipit, vero error magnam voluptatum.',
    'image': {
      'source': '/static-resource/alaska/assets/images/secondary-illustrations/privacy_promise_dark.svg',
      'altText': 'Privacy Promise Page'
    },
    'details': [
      {
        'title': 'Data Collection and Use',
        'description': 'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Adipisci, distinctio possimus neque corrupti veritatis aspernatur odit quae sed voluptas minima, aliquam iste ex deleniti ut aliquid quaerat animi, similique accusamus?'
      }
    ],
    'organizationalUnit': '',
    'id': '7b58a732-2f77-1db9-dfdf-9798fb564d6c',
    'name': 'PrivacyPromisePage',
    'location': [
      {
        'state': 'Default',
        'county': null,
        'city': null,
        'zipCode': null
      }]}]");

        public static string createdStaticResourcesContent = "{"+
    "\"description\": \"Lorem, ipsum dolor sit amet consectetur adipisicing elit. Asperiores illo totam iste rerum fuga sit facilis modi iusto sapiente saepe tenetur labore eaque esse quod alias maxime ut velit quisquam, doloremque perspiciatis harum omnis. Odit dolorem, reprehenderit cupiditate deleniti amet a aperiam voluptates quam facilis suscipit, vero error magnam voluptatum.\","+
    "\"image\": {" +
    "  \"source\": \"/static-resource/alaska/assets/images/secondary-illustrations/privacy_promise_dark.svg\"," +
    "  \"altText\": \"Privacy Promise Page\"" +
    "}," +
    "\"details\": [" +
    "  {"+
    "    \"title\": \"Data Collection and Use\","+
    "    \"description\": \"Lorem ipsum dolor sit, amet consectetur adipisicing elit. Adipisci, distinctio possimus neque corrupti veritatis aspernatur odit quae sed voluptas minima, aliquam iste ex deleniti ut aliquid quaerat animi, similique accusamus?\""+
    "  }"+
    "],"+
    "\"organizationalUnit\": \"Hawaii\","+
    "\"id\": \"7b58a732-2f77-1db9-dfdf-9798fb564d6c\","+
    "\"name\": \"PrivacyPromisePage\","+
    "\"location\": ["+
    "  {"+
    "    \"state\": \"Hawaii\","+
    "    \"county\": null,"+
    "    \"city\": null,"+
    "    \"zipCode\": null"+
    "  }]}";

        public static string updatedHelpAndFAQContent = "{\"description\": \"Lorem ipsum, dolor sit amet consectetur adipisicing elit. A non excepturi ullam magnam similique, nobis officiis cupiditate aperiam consectetur at labore commodi! Soluta, expedita fuga in quod corporis voluptatem reiciendis. \"," +
            "\"image\": {\"source\": \"/static-resource/alaska/assets/images/secondary-illustrations/help_faqs.svg\"," +
            "\"altText\": \"Help and FAQs Page\"}," +
            "\"faqs\": [{\"question\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Alias quo expedita, ab vero repellat voluptates molestiae? Ipsam, ducimus nam. Ducimus.\"," +
            "\"answer\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Repellat reprehenderit laborum dicta architecto iusto at sed iste fugit necessitatibus rem, fuga ad minima provident neque est autem dolore officiis, ab nihil? Consectetur, nostrum nam eligendi praesentium illo dignissimos magni maxime.\"}]," +
            "\"organizationalUnit\": \"Alaska\"," +
            "\"id\": \"e84dee21-d4d1-7f30-f4b6-b5bde92d4c94\"," +
            "\"name\": \"HelpAndFAQPage\"," +
            "\"location\": [{\"state\": \"Alaska\"," +
            "\"county\": null," +
            "\"city\": null," +
            "\"zipCode\": null}]}";

        public static JArray staticHelpAndFAQContent = JArray.Parse(@"[{
  'description': 'Lorem ipsum, dolor sit amet consectetur adipisicing elit. A non excepturi ullam magnam similique, nobis officiis cupiditate aperiam consectetur at labore commodi! Soluta, expedita fuga in quod corporis voluptatem reiciendis. ',
  'image': {
    'source': '/static-resource/alaska/assets/images/secondary-illustrations/help_faqs.svg',
    'altText': 'Help and FAQs Page'
  },
  'faqs': [
    {
      'question': 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Alias quo expedita, ab vero repellat voluptates molestiae? Ipsam, ducimus nam. Ducimus.',
      'answer': 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Repellat reprehenderit laborum dicta architecto iusto at sed iste fugit necessitatibus rem, fuga ad minima provident neque est autem dolore officiis, ab nihil? Consectetur, nostrum nam eligendi praesentium illo dignissimos magni maxime.'        
    }
  ],
  'organizationalUnit': 'Alaska',
  'id': 'e84dee21-d4d1-7f30-f4b6-b5bde92d4c94',
  'name': 'HelpAndFAQPage',
  'location': [
    {
      'state': 'Alaska',
      'county': null,
      'city': null,
      'zipCode': null
    }
  ]
}]");

        public static string updatedAboutContent = "{\"name\": \"AboutPage\"," +
            "\"location\": [ { \"state\": \"Alaska\" } ]," +
            "\"organizationalUnit\": \"Alaska\"," +
            "\"aboutImage\": {\"source\": \"/static-resource/alaska/assets/images/secondary-illustrations/about.svg\"," +
            "\"altText\": \"About Page\"}," +
            "\"mission\": {\"title\": \"Our Mission\"," +
            "\"description\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Harum eveniet beatae nam voluptatibus distinctio veritatis vel saepe nulla eum suscipit necessitatibus quae labore, quisquam ad, eius officia! Commodi, sapiente perferendis! Atque vitae natus quasi at culpa porro fugit eum laboriosam id earum quibusdam, explicabo nisi mollitia saepe tempora praesentium in sunt, exercitationem error, magni ratione ipsa numquam debitis consequatur. Repellendus asperiores dolor, quidem perspiciatis sapiente tenetur provident dolorem accusamus quibusdam eveniet placeat officiis, quod fugiat ullam minima rem. Molestias, quos inventore accusamus eligendi culpa necessitatibus rerum nostrum aspernatur eaque explicabo voluptatem architecto esse, debitis non earum autem, ipsam modi sit!\"}," +
            "\"id\": \"ab0ef241-5778-0147-062c-005e80946d38\"}";

        public static JArray staticAboutContent = JArray.Parse(@"[{
    'name': 'AboutPage',
    'location': [ { 'state': 'Alaska' } ],
    'organizationalUnit': 'Alaska',
    'aboutImage': {
      'source': '/static-resource/alaska/assets/images/secondary-illustrations/about.svg',
      'altText': 'About Page'
    },
    'mission': {
      'title': 'Our Mission',
      'description': 'Lorem ipsum dolor sit amet consectetur adipisicing elit. Harum eveniet beatae nam voluptatibus distinctio veritatis vel saepe nulla eum suscipit necessitatibus quae labore, quisquam ad, eius officia! Commodi, sapiente perferendis! Atque vitae natus quasi at culpa porro fugit eum laboriosam id earum quibusdam, explicabo nisi mollitia saepe tempora praesentium in sunt, exercitationem error, magni ratione ipsa numquam debitis consequatur. Repellendus asperiores dolor, quidem perspiciatis sapiente tenetur provident dolorem accusamus quibusdam eveniet placeat officiis, quod fugiat ullam minima rem. Molestias, quos inventore accusamus eligendi culpa necessitatibus rerum nostrum aspernatur eaque explicabo voluptatem architecto esse, debitis non earum autem, ipsam modi sit!',
      },
    'id': 'ab0ef241-5778-0147-062c-005e80946d38'
  }]");

        public static string updatedHomeContent = "{\"hero\":{\"heading\":\"Get help with your legal questions in\",\"description\":{\"text\":\"We will always protect your privacy. Read our\",\"textWithLink\":{\"urlText\":\"Privacy Promise\",\"url\":\"privacy\"}}," +
    "\"image\":{\"source\":\"/static-resource/alaska/assets/images/primary-illustrations/alaska.svg\",\"altText\":\"Illustration of Alaska\"}},\"guidedAssistantOverview\":{\"heading\":\"Need a plan of action for your legal issue? Our Guided Assistant can help.\"," +
    "\"description\":{\"steps\":[{\"order\":1,\"description\":\"Answer a few questions to help us understand your needs.\"},{\"order\":2,\"description\":\"We'll create a personalized plan with steps and resources.\"}," +
    "{\"order\":3,\"description\":\"Share, print, or create a profile to save your plan.\"}],\"text\":\"Free and no account necessary. We never share any information you provide.\",\"textWithLink\":{\"urlText\":\"Privacy Promise\"," +
    "\"url\":\"privacy\"}},\"button\":{\"buttonText\":\"Start the Guided Assistant\",\"buttonAltText\":\"Start the Guided Assistant\",\"buttonLink\":\"guidedassistant\"},\"image\":{\"source\":\"/static-resource/alaska/assets/images/secondary-illustrations/guided_assistant.svg\"," +
    "\"altText\":\"Guided Assistant Illustration\"}},\"topicAndResources\":{\"heading\":\"More Information, Videos, and Links to Resources by Topic\",\"button\":{\"buttonText\":\"See More Topics\",\"buttonAltText\":\"Alt text\"," +
    "\"buttonLink\":\"topics\"}},\"carousel\":{\"slides\":[{\"quote\":\"Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel..\"," +
    "\"author\":\"Robbie\",\"location\":\"Anchorage, AK\",\"image\":{\"source\":\"/static-resource/alaska/assets/images/sample-images/trees_0.JPG\",\"altText\":\"Image not available\"}},{\"quote\":\"Ut fusce varius nisl ac ipsum gravida vel pretium tellus tincidunt integer eu augue augue nunc elit dolor, luctus placerat.\"," +
    "\"author\":\"Robbie\",\"location\":\"Anchorage, AK\",\"image\":{\"source\":\"/static-resource/alaska/assets/images/sample-images/trees_1.JPG\",\"altText\":\"Image not available\"}},{\"quote\":\"This site is here to provide equal access to legal information and resources for everyone. Legal disclaimer lorem ipsum dolor amet. It is brought to you by partnership of these nonproffit organizations.\"," +
    "\"author\":\"Robbie\",\"location\":\"Anchorage, AK\",\"image\":{\"source\":\"/static-resource/alaska/assets/images/sample-images/trees_2.JPG\",\"altText\":\"Image not available\"}}]},\"sponsorOverview\":{\"heading\":\"Information you can trust\",\"description\":\"This site is here to provide equal access to legal information and resources for everyone. Legal disclaimer lorem ipsum dolor amet. It is brought to you by partnership of these nonproffit organizations\"," +
    "\"sponsors\":[{\"source\":\"\",\"altText\":\"Illustration of Hawaii\"},{\"source\":\"\",\"altText\":\"Illustration of Hawaii\"}],\"button\":{\"buttonText\":\"Learn More\",\"buttonAltText\":\"Learn More\",\"buttonLink\":\"about\"}},\"privacy\":{\"heading\":\"Our Privacy Promise\",\"description\":\"We value your privacy and will never share your personal information with anyone without your lorem ipsum dolor sit amet.\"," +
    "\"button\":{\"buttonText\":\"Learn More\",\"buttonAltText\":\"Alt text\",\"buttonLink\":\"privacy\"},\"image\":{\"source\":\"/static-resource/alaska/assets/images/secondary-illustrations/privacy_promise.svg\",\"altText\":\"Privacy Illustration\"}},\"helpText\":\"Are you safe? Call X-XXX-XXX-XXXX to get help.\"," +
    "\"organizationalUnit\":\"Alaska\",\"id\":null,\"name\":\"HomePage\"," +
    "\"location\":[{\"state\":\"Alaska\",\"county\":null,\"city\":null,\"zipCode\":null}]}";

        public static JArray staticHomeContent = JArray.Parse(@"[  {
    'hero': {
      'heading': 'Get help with your legal questions in',
      'description': {
        'text': 'We will always protect your privacy. Read our',
        'textWithLink': {
          'urlText': 'Privacy Promise',
          'url': 'privacy'
        }
      },
      'image': {
        'source': '/static-resource/alaska/assets/images/primary-illustrations/alaska.svg',
        'altText': 'Illustration of Alaska'
      }
    },
    'guidedAssistantOverview': {
      'heading': 'Need a plan of action for your legal issue? Our Guided Assistant can help.',
      'description': {
        'steps': [
          {
            'order': 1,
            'description': 'Answer a few questions to help us understand your needs.'
          },
          {
            'order': 2,
            'description': 'We will create a personalized plan with steps and resources.'
          },
          {
            'order': 3,
            'description': 'Share, print, or create a profile to save your plan.'
          }
        ],
        'text': 'Free and no account necessary. We never share any information you provide.',
        'textWithLink': {
          'urlText': 'Privacy Promise',
          'url': 'privacy'
        }
      },
      'button': {
        'buttonText': 'Start the Guided Assistant',
        'buttonAltText': 'Start the Guided Assistant',
        'buttonLink': 'guidedassistant'
      },
      'image': {
        'source': '/static-resource/alaska/assets/images/secondary-illustrations/guided_assistant.svg',
        'altText': 'Guided Assistant Illustration'
      }
    },
    'topicAndResources': {
      'heading': 'More Information, Videos, and Links to Resources by Topic',
      'button': {
        'buttonText': 'See More Topics',
        'buttonAltText': 'Alt text',
        'buttonLink': 'topics'
      }
    },
    'carousel': {
      'slides': [
        {
          'quote': 'Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel..',
          'author': 'Robbie',
          'location': 'Anchorage, AK',
          'image': {
            'source': '/static-resource/alaska/assets/images/sample-images/trees_0.JPG',
            'altText': 'Image not available'
          }
        },
        {
          'quote': 'Ut fusce varius nisl ac ipsum gravida vel pretium tellus tincidunt integer eu augue augue nunc elit dolor, luctus placerat.',
          'author': 'Robbie',
          'location': 'Anchorage, AK',
          'image': {
            'source': '/static-resource/alaska/assets/images/sample-images/trees_1.JPG',
            'altText': 'Image not available'
          }
        },
        {
          'quote': 'This site is here to provide equal access to legal information and resources for everyone. Legal disclaimer lorem ipsum dolor amet. It is brought to you by partnership of these nonproffit organizations.',
          'author': 'Robbie',
          'location': 'Anchorage, AK',
          'image': {
            'source': '/static-resource/alaska/assets/images/sample-images/trees_2.JPG',
            'altText': 'Image not available'
          }
        }
      ]
    },
    'sponsorOverview': {
      'heading': 'Information you can trust',
      'description': 'This site is here to provide equal access to legal information and resources for everyone. Legal disclaimer lorem ipsum dolor amet. It is brought to you by partnership of these nonproffit organizations',
      'sponsors': [
        {
          'source': '',
          'altText': 'Illustration of Hawaii'
        },
        {
          'source': '',
          'altText': 'Illustration of Hawaii'
        }
      ],
      'button': {
        'buttonText': 'Learn More',
        'buttonAltText': 'Learn More',
        'buttonLink': 'about'
      }
    },
    'privacy': {
      'heading': 'Our Privacy Promise',
      'description': 'We value your privacy and will never share your personal information with anyone without your lorem ipsum dolor sit amet.',
      'button': {
        'buttonText': 'Learn More',
        'buttonAltText': 'Alt text',
        'buttonLink': 'privacy'
      },
      'image': {
        'source': '/static-resource/alaska/assets/images/secondary-illustrations/privacy_promise.svg',
        'altText': 'Privacy Illustration'
      }
    },
    'helpText': {
             'beginningText': 'Are you safe? Call',
             'phoneNumber': '123-456-7890',
             'endText': 'to get help'
          },
    'organizationalUnit': 'Alaska',
    'id': 'c586534a-9d6e-4434-ad30-f7457804c51a',
    'name': 'HomePage',
    'location': [
      {
        'state': 'Alaska',
        'county': null,
        'city': null,
        'zipCode': null
      }
    ]
  }]");

        public static JArray staticPersonalizedPlanContent = JArray.Parse(@"[{
    'id': '0cc3a8c0-9d97-2b80-3fc9-a7a7a8fb315e',
    'name': 'PersonalizedActionPlanPage',
    'location': [
        {
            'state': 'HI',
            'county': null,
            'city': null,
            'zipCode': null
        }
    ],
    'organizationalUnit': 'HI',
    'description': 'the following information and resources are sponsored by the following organizations-Hawaii',
    'sponsors': [
        {
            'image': {
                'source': '',
                'altText': ''
            }
        }
    ]
  }]");

        public static string updatedPersonalizedPlanContent = @"{
    'id': '0cc3a8c0-9d97-2b80-3fc9-a7a7a8fb315e',
    'name': 'PersonalizedActionPlanPage',
    'location': [
        {
            'state': 'HI',
            'county': null,
            'city': null,
            'zipCode': null
        }
    ],
    'organizationalUnit': 'HI',
    'description': 'the following information and resources are sponsored by the following organizations-Hawaii',
    'sponsors': [
        {
            'image': {
                'source': '',
                'altText': ''
            }
        }
    ]
  }";


        public static IEnumerable<object[]> UpsertNavigationContent()
        {
            yield return new object[] { new Navigation { Location = new List<Location>{new Location(){ State="Hawaii"} }, Name = "Navigation", OrganizationalUnit = "Hawaii" },
             staticNavigationContent, staticNavigationContent };
            yield return new object[] { new Navigation { Location = new List<Location>{new Location(){ State="Hawaii"} }, Name = "Navigation", OrganizationalUnit = "Hawaii" },
             JArray.Parse(@"[]"), staticNavigationContent};
        }

        public static IEnumerable<object[]> UpsertPrivacyPromiseContent()
        {
            yield return new object[] { new PrivacyPromiseContent { Location = new List<Location>{new Location(){ State= "Alaska" } }, Name = "PrivacyPromisePage", OrganizationalUnit = "Alaska" },
             staticPrivacyPromiseContent, staticPrivacyPromiseContent };
            yield return new object[] { new PrivacyPromiseContent { Location = new List<Location>{new Location(){ State= "Alaska" } }, Name = "PrivacyPromisePage", OrganizationalUnit = "Alaska" },
             JArray.Parse(@"[]"), staticPrivacyPromiseContent};
        }

        public static IEnumerable<object[]> UpsertHelpAndFAQPageContent()
        {
            yield return new object[] { new HelpAndFaqsContent { Location = new List<Location>{new Location(){ State= "Alaska" } }, Name = "HelpAndFAQPage", OrganizationalUnit = "Alaska" },
             staticHelpAndFAQContent, staticHelpAndFAQContent };
            yield return new object[] { new HelpAndFaqsContent { Location = new List<Location>{new Location(){ State= "Alaska" } }, Name = "HelpAndFAQPage", OrganizationalUnit = "Alaska" },
             JArray.Parse(@"[]"), staticHelpAndFAQContent};
        }

        public static IEnumerable<object[]> UpsertAboutPageContent()
        {
            yield return new object[] { new AboutContent { Location = new List<Location>{new Location(){ State="Alaska"} }, Name = "AboutPage", OrganizationalUnit = "Alaska" },
             staticAboutContent, staticAboutContent };
            yield return new object[] { new AboutContent { Location = new List<Location>{new Location(){ State="Alaska"} }, Name = "AboutPage", OrganizationalUnit = "Alaska" },
             JArray.Parse(@"[]"), staticAboutContent};
        }
        public static IEnumerable<object[]> UpsertHomePageContent()
        {
            yield return new object[] { new HomeContent { Location = new List<Location>{new Location(){ State="Alaska"} }, Name = "HomePage", OrganizationalUnit = "Alaska" },
             staticHomeContent, staticHomeContent };
            yield return new object[] { new HomeContent { Location = new List<Location>{new Location(){ State="Alaska"} }, Name = "HomePage", OrganizationalUnit = "Alaska" },
             JArray.Parse(@"[]"), staticHomeContent};
        }    
        public static IEnumerable<object[]> UpsertStaticPersnalizedPlanContent()
        {
            yield return new object[] { new PersonalizedPlanContent { Id = "0cc3a8c0-9d97-2b80-3fc9-a7a7a8fb315e", Description = "the following information and resources are sponsored by the following organizations-Hawaii", Name = "PersonalizedActionPlanPage", Location = new List<Location> { new Location() { State = "HI" } }, OrganizationalUnit = "HI", Sponsors = new List<Sponsors> { new Sponsors() {  Source = "HI", AltText="Hawaii" } } },
                staticPersonalizedPlanContent, staticPersonalizedPlanContent };
            yield return new object[] { new PersonalizedPlanContent { Id = "0cc3a8c0-9d97-2b80-3fc9-a7a7a8fb315e", Description = "the following information and resources are sponsored by the following organizations-Hawaii", Name = "PersonalizedActionPlanPage", Location = new List<Location> { new Location() { State = "HI" } }, OrganizationalUnit = "HI", Sponsors = new List<Sponsors> { new Sponsors() {  Source = "HI", AltText="Hawaii" } } },
                JArray.Parse(@"[]"), staticPersonalizedPlanContent };
        }
    }

}