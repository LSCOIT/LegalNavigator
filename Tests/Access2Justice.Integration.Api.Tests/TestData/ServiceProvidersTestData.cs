using System.Threading.Tasks;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Models.Integration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace Access2Justice.Integration.Api.Tests
{
    class ServiceProvidersTestData
    {
        public ServiceProvidersTestData()
        {

        }
        public static string serviceProviderId = "fdf5b9ed-7f0a-4544-9a00-302053d15dbb";
        public static string externalId = "7063";
        public static JArray topic =
            JArray.Parse(@"[{
            'id': 'e1fdbbc6-d66a-4275-9cd2-2be84d303e12',
            'name': 'Family',
            'overview': '',
            'parentTopicId': null,
            'resourceType': 'Topics',
            'keywords': '',
            'organizationalUnit': 'Hawaii',
            'location': [
                {
                    'state': 'Hawaii',
                    'county': '',
                    'city': '',
                    'zipCode': ''
                }
            ],
            'icon': 'https://cs4892808efec24x447cx944.blob.core.windows.net/static-resource/assets/images/categories/family.svg',
            'createdBy': 'Admin Import tool',
            'createdTimeStamp': '2018-11-05T11:02:20.8308483Z',
            'modifiedBy': 'Admin Import tool',
            'modifiedTimeStamp': '2018-11-05T11:02:20.8308485Z'
            }]");
        public static JArray serviceProviderData =
            JArray.Parse(@"[{
            'externalId': '7063',
            'email': 'info@thecouplescenteralaska.org',
            'availability': {
                'regularBusinessHours': [
                    {
                        'day': 0,
                        'opensAt': '07:35:10',
                        'closesAt': '17:35:10'
                    }
                ],
                'holidayBusinessHours': [
                    {
                        'day': 0,
                        'opensAt': '10:35:10',
                        'closesAt': '17:35:10'
                    }
                ],
                'waitTime': '01:30:00'
            },
            'acceptanceCriteria': {
                'description': 'Female under age 25',
                'evaluatedRequirements': [
                    {
                        'condition': {
                            'displayLabel': 'Female',
                            'data': 'age',
                            'dataType': 2
                        },
                        'operatorName': 2,
                        'variable': '25'
                    },
                    {
                        'condition': {
                            'displayLabel': 'Female',
                            'data': 'age',
                            'dataType': 2
                        },
                        'operatorName': 0,
                        'variable': '25'
                    }
                ]
            },
            'onboardingInfo': {
                'userFields': [
                    {
                        'name': 'test1',
                        'value': 'val1'
                    },
                    {
                        'name': 'test2',
                        'value': 'val2'
                    }
                ]
            },
            'address': '880 H Street, Suite 202, Anchorage, AK, 99501',
            'telephone': '(907) 222-3699 Main | (907) 258-6631 Fax',
            'overview': null,
            'eligibilityInformation': null,
            'reviewer': [
                {
                    'reviewerFullName': 'John Doe',
                    'reviewerTitle': 'Director',
                    'reviewText': 'This is a my review of the organization',
                    'reviewerImage': 'url'
                }
            ],
            'specialties': null,
            'qualifications': null,
            'businessHours': null,
            'id': 'fdf5b9ed-7f0a-4544-9a00-302053d15dbb',
            'name': 'COUPLES CENTER OF ALASKA (THE)',
            'resourceCategory': null,
            'description': 'Assists homeless children and adolescents to enroll in the Anchorage School District.  Supports attendance and success in school via advocacy, community education, referrals, tutoring and transportation.',
            'resourceType': 'Service Providers',
            'url': 'www.thecouplescenteralaska.org/index.html',
            'topicTags': [
                {
                    'id': 'e1fdbbc6-d66a-4275-9cd2-2be84d303e12'
                }
            ],
            'organizationalUnit': 'AK',
            'location': [
                {
                    'state': 'AK',
                    'county': 'Anchorage',
                    'city': 'Anchorage',
                    'zipCode': '99501'
                }
            ],
            'createdBy': 'Integration API',
            'createdTimeStamp': '',
            'modifiedBy': 'Integration API',
            'modifiedTimeStamp': ''
        }]");
        public static List<TopicTag> topicTag = new List<TopicTag>
            {
                new TopicTag { TopicTags = "e1fdbbc6-d66a-4275-9cd2-2be84d303e12" }
            };
        public static JArray emptyData = JArray.Parse(@"[]");        
        public static string servicerProvidersData = "[{\"externalId\":\"7063\",\"email\":\"info@thecouplescenteralaska.org\",\"availability\":{\"regularBusinessHours\":[{\"day\":0,\"opensAt\":\"07:35:10\",\"closesAt\":\"17:35:10\"}],\"holidayBusinessHours\":[{\"day\":0,\"opensAt\":\"10:35:10\",\"closesAt\":\"17:35:10\"}],\"waitTime\":\"01:30:00\"},\"acceptanceCriteria\":{\"description\":\"Female under age 25\",\"evaluatedRequirements\":[{\"condition\":{\"displayLabel\":\"Female\",\"data\":\"age\",\"dataType\":2},\"operatorName\":2,\"variable\":\"25\"},{\"condition\":{\"displayLabel\":\"Female\",\"data\":\"age\",\"dataType\":2},\"operatorName\":0,\"variable\":\"25\"}]},\"onboardingInfo\":{\"userFields\":[{\"name\":\"test1\",\"value\":\"val1\"},{\"name\":\"test2\",\"value\":\"val2\"}]},\"address\":\"880 H Street, Suite 202, Anchorage, AK, 99501\",\"telephone\":\"(907) 222-3699 Main | (907) 258-6631 Fax\",\"overview\":null,\"eligibilityInformation\":null,\"reviewer\":[{\"reviewerFullName\":\"John Doe\",\"reviewerTitle\":\"Director\",\"reviewText\":\"This is a my review of the organization\",\"reviewerImage\":\"url\"}],\"specialties\":null,\"qualifications\":null,\"businessHours\":null,\"id\":\"fdf5b9ed-7f0a-4544-9a00-302053d15dbb\",\"name\":\"COUPLES CENTER OF ALASKA (THE)\",\"resourceCategory\":null,\"description\":\"Assists homeless children and adolescents to enroll in the Anchorage School District.  Supports attendance and success in school via advocacy, community education, referrals, tutoring and transportation.\",\"resourceType\":\"Service Providers\",\"url\":\"www.thecouplescenteralaska.org/index.html\",\"topicTags\":[{\"id\":\"e1fdbbc6-d66a-4275-9cd2-2be84d303e12\"}],\"organizationalUnit\":\"AK\",\"location\":[{\"state\":\"AK\",\"county\":\"Anchorage\",\"city\":\"Anchorage\",\"zipCode\":\"99501\"}],\"createdBy\":\"Integration API\",\"createdTimeStamp\":null,\"modifiedBy\":\"Integration API\",\"modifiedTimeStamp\":null}]";
        static List<ServiceProvider> serviceProviders = JsonConvert.DeserializeObject<List<ServiceProvider>>(servicerProvidersData);
        //expected data
        public static JArray expectedServiceProviderdata =
            JArray.Parse(@"[{  'externalId': '7063',  'email': 'info@thecouplescenteralaska.org',  'availability': {    'regularBusinessHours': [      {        'day': 0,        'opensAt': '07:35:10',        'closesAt': '17:35:10'      }    ],    'holidayBusinessHours': [      {        'day': 0,        'opensAt': '10:35:10',        'closesAt': '17:35:10'      }    ],    'waitTime': '01:30:00'  },  'acceptanceCriteria': {    'description': 'Female under age 25',    'evaluatedRequirements': [      {        'condition': {          'displayLabel': 'Female',          'data': 'age',          'dataType': 2        },        'operatorName': 2,        'variable': '25'      },      {        'condition': {          'displayLabel': 'Female',          'data': 'age',          'dataType': 2        },        'operatorName': 0,        'variable': '25'      }    ]  },  'onboardingInfo': {    'userFields': [      {        'name': 'test1',        'value': 'val1'      },      {        'name': 'test2',        'value': 'val2'      }    ]  },  'address': '880 H Street, Suite 202, Anchorage, AK, 99501',  'telephone': '(907) 222-3699 Main | (907) 258-6631 Fax',  'overview': null,  'eligibilityInformation': null,  'reviewer': [    {      'reviewerFullName': 'John Doe',      'reviewerTitle': 'Director',      'reviewText': 'This is a my review of the organization',      'reviewerImage': 'url'    }  ],  'specialties': null,  'qualifications': null,  'businessHours': null,  'id': 'fdf5b9ed-7f0a-4544-9a00-302053d15dbb',  'name': 'COUPLES CENTER OF ALASKA (THE)',  'resourceCategory': null,  'description': 'Assists homeless children and adolescents to enroll in the Anchorage School District.  Supports attendance and success in school via advocacy, community education, referrals, tutoring and transportation.',  'resourceType': 'Service Providers',  'url': 'www.thecouplescenteralaska.org/index.html',  'topicTags': [    {      'id': 'e1fdbbc6-d66a-4275-9cd2-2be84d303e12'    }  ],  'organizationalUnit': 'AK',  'location': [    {      'state': 'AK',      'county': 'Anchorage',      'city': 'Anchorage',      'zipCode': '99501'    }  ],  'createdBy': 'Integration API',  'createdTimeStamp': '',  'modifiedBy': 'Integration API',  'modifiedTimeStamp': ''}]");
        
        public static IEnumerable<object[]> ServiceProviderGetInputData()
        {
            yield return new object[] { serviceProviderId ,serviceProviderData, expectedServiceProviderdata };
            yield return new object[] { "test", JArray.Parse(@"[{}]"), JArray.Parse(@"[{}]") };
        }
        
        public static IEnumerable<object[]> ServiceProviderUpsertInputData()
        {
            yield return new object[] { "Family", topic, externalId, serviceProviderData, "fdf5b9ed-7f0a-4544-9a00-302053d15dbb", serviceProviderData, serviceProviders, expectedServiceProviderdata };
            yield return new object[] { "Family", topic, externalId, emptyData, "fdf5b9ed-7f0a-4544-9a00-302053d15dbb", serviceProviderData, serviceProviders, expectedServiceProviderdata };
        }
        
        public static IEnumerable<object[]> GetServiceProviderTopicTagsInputData()
        {
            yield return new object[] { "e1fdbbc6-d66a-4275-9cd2-2be84d303e12", JArray.Parse(@"[{ 'id': 'e1fdbbc6-d66a-4275-9cd2-2be84d303e12'}]") };
        }
    }
}
