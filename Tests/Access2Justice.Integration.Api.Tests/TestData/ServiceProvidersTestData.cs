using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
namespace Access2Justice.Integration.Api.Tests
{
    class ServiceProvidersTestData
    {
        public static string serviceProviderId = "fdf5b9ed-7f0a-4544-9a00-302053d15dbb";
        public static string siteId = "7063";
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
            'siteId': '7063',
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

        public static JArray serviceProviderJson =
            JArray.Parse(@"[ 	{
		'Key': 'UWAK7062',
		'Name': 'COUPLES CENTER OF ALASKA (THE)',
		'Sites': [
			{
        'availability': {
                'regularBusinessHours': [
                    {
                        'day': 0,
                        'opensAt': '07:35:10',
                        'closesAt': '17:35:10'
                    },
                    {
                        'day': 0,
                        'opensAt': '10:35:10',
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
            'reviewer': [
                {
                    'reviewerFullName': 'John Doe',
                    'reviewerTitle': 'Director',
                    'reviewText': 'This is a my review of the organization',
                    'reviewerImage': 'url'
                }
            ],
				        'ID': '7063',//sitekey in provider detail
				        'Name': 'COUPLES CENTER OF ALASKA (THE)',//name
				        'Email': 'info@thecouplescenteralaska.org',//need to add for service provider
				        'URL': 'www.thecouplescenteralaska.org/index.html',//url
				        'Distance': '4',
				        'Address': [
					        {
						        'Type': 's',
						        'Conf': '0',
						        'Line1': '880 H Street',//
						        'Line2': 'Suite 202',//
						        'Line3': '',//
						        'City': 'Anchorage',//
						        'State': 'AK',//full state name mapping --need to create bug
						        'Zip': '99501',//
						        'County': 'Anchorage'
					        }
				        ],//pipe delimited
				        'Phones': [
					        {
						        'Type': 'st',
						        'Phone': '(907) 222-3699 Main'
					        },
					        {
						        'Type': 'st',
						        'Phone': '(907) 258-6631 Fax'//need to check on type to main or fax
					        }
				        ],//pipe delimited
				        'ServiceGroup': [
					        {
						        'ID': '9241',
						        'Name': 'EMPLOYMENT/VOCATIONAL SERVICES',
						        'Program': 'HEAD START',
						        'Email': '',
						        'URL': '',
						        'Phones': [],
						        'ServiceSites': [
							        {
								        'ID': '53493',
								        'Name': 'Workshops/Symposiums',
								        'Note1': '',
								        'Note2': '',
								        'Email': '',
								        'URL': '',
								        'Phones': []
							        }
						        ],
						        'MLText': ''
					        }
				        ],
				        'Logos': []
			        }
		        ],
		        'AuxLists': []
	        }]");
        public static JArray providerDetailJson =
            JArray.Parse(@"[
	        {
		        'SiteKey': '7063',
		        'ServiceGroup': [
			        {
				        'ID': '188',
				        'Name': 'FAMILY SUPPORT SERVICES',
				        'Program': 'CHILD IN TRANSITION/HOMELESS PROJECT',
				        'Email': '',
				        'URL': 'www.asdk12.org',
				        'Phones': [
					        {
						        'Type': 'sv',
						        'Phone': '(907) 742-3833 Service/Intake; Child In Transition/Homeless Project'
					        },
					        {
						        'Type': 'sv',
						        'Phone': '(907) 742-4000 Main'
					        },
					        {
						        'Type': 'sv',
						        'Phone': '(907) 742-4175 Fax'
					        }
				        ],
				        'ServiceSites': [],
				        'MLText': ''
			        }
		        ],
		        'DetailText': [
			        {
				        'Label': 'SERVICE DESCRIPTION',
				        'Text': 'Assists homeless children and adolescents to enroll in the Anchorage School District.  Supports attendance and success in school via advocacy, community education, referrals, tutoring and transportation.'
			        },
			        {
				        'Label': 'DOCUMENTS TO BRING',
				        'Text': 'Call for more information.'
			        },
			        {
				        'Label': 'FEES',
				        'Text': 'None.'
			        },
			        {
				        'Label': 'INTAKE',
				        'Text': 'Call or walk in.'
			        },
			        {
				        'Label': 'ELIGIBILITY',
				        'Text': 'Children and youth age 3 to 21 of families who are homeless.'
			        },
			        {
				        'Label': 'SITE HOURS',
				        'Text': 'Mon-Fri 8am-5pm.'
			        },
			        {
				        'Label': 'AREA SERVED',
				        'Text': 'Municipality of Anchorage, primarily.'
			        }
		        ],
		        'Services': [
			        {
				        'ID': '45608',
				        'Name': 'Homeless School Transition Programs',
				        'Note1': '',
				        'Note2': '',
				        'Email': '',
				        'URL': 'www.asdk12.org',
				        'Phones': [
					        {
						        'Type': 'ss',
						        'Phone': '(907) 742-3833 Service/Intake Child In Transition/Homeless Project'
					        },
					        {
						        'Type': 'ss',
						        'Phone': '(907) 742-4000 Main'
					        },
					        {
						        'Type': 'ss',
						        'Phone': '(907) 742-4175 Fax'
					        }
				        ],
				        'Filters': '',
				        'Refer': '1'
			        }
		        ],
		        'OtherServiceGroups': [
			        {
				        'ID': '5034',
				        'Name': 'EDUCATION AND TRAINING'
			        },
			        {
				        'ID': '5298',
				        'Name': 'EDUCATION AND TRAINING - ALASKA STATE SCHOOL FOR DEAF AND HARD OF HEARING'
			        },
			        {
				        'ID': '9304',
				        'Name': 'FOOD SERVICES - FREE AND REDUCED-PRICE MEALS PROGRAM'
			        },
			        {
				        'ID': '324',
				        'Name': 'INFORMATION AND REFERRAL - STUDENTS, EDUCATORS, AND PARENTS CENTER AND LENDING LIBRARY'
			        }
		        ],
		        'OtherLocations': [],
		        'Logos': [],
		        'Documents': []
	        }
        ]");
        private static JArray siteData =
            JArray.Parse(@"[{
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
          'reviewer': [
            {
              'reviewerFullName': 'John Doe',
              'reviewerTitle': 'Director',
              'reviewText': 'This is a my review of the organization',
              'reviewerImage': 'url'
            }
          ],
          'ID': '7063',
          'Name': 'COUPLES CENTER OF ALASKA (THE)',
          'Email': 'info@thecouplescenteralaska.org',
          'URL': 'www.thecouplescenteralaska.org/index.html',
          'Distance': '4',
          'Address': [
            {
              'Type': 's',
              'Conf': '0',
              'Line1': '880 H Street',
              'Line2': 'Suite 202',
              'Line3': '',
              'City': 'Anchorage',
              'State': 'AK',
              'Zip': '99501',
              'County': 'Anchorage'
            }
          ],
          'Phones': [
            {
              'Type': 'st',
              'Phone': '(907) 222-3699 Main'
            },
            {
              'Type': 'st',
              'Phone': '(907) 258-6631 Fax'
            }
          ],
          'ServiceGroup': [
            {
              'ID': '9241',
              'Name': 'EMPLOYMENT/VOCATIONAL SERVICES',
              'Program': 'HEAD START',
              'Email': '',
              'URL': '',
              'Phones': [],
              'ServiceSites': [
                {
                  'ID': '53493',
                  'Name': 'Workshops/Symposiums',
                  'Note1': '',
                  'Note2': '',
                  'Email': '',
                  'URL': '',
                  'Phones': []
                }
              ],
              'MLText': ''
            }
          ],
          'Logos': []
        }]");
        public static JArray siteAddress =
            JArray.Parse(@"[
            {
              'Type': 's',
              'Conf': '0',
              'Line1': '880 H Street',
              'Line2': 'Suite 202',
              'Line3': '',
              'City': 'Anchorage',
              'State': 'AK',
              'Zip': '99501',
              'County': 'Anchorage'
            }
            ]");
        public static JArray sitePhone =
            JArray.Parse(@"[
                {
                  'Type': 'st',
                  'Phone': '(907) 222-3699 Main'
                },
                {
                  'Type': 'st',
                  'Phone': '(907) 258-6631 Fax'
                }
              ]");
        public static List<TopicTag> topicTag = new List<TopicTag>
            {
                new TopicTag { TopicTags = "e1fdbbc6-d66a-4275-9cd2-2be84d303e12" }
            };
        public static JArray providerJson =
            JArray.Parse(@"[{
		        'SiteKey': '7063',
		        'DetailText': [
			        {
				        'Label': 'SERVICE DESCRIPTION',
				        'Text': 'Assists homeless children and adolescents to enroll in the Anchorage School District.  Supports attendance and success in school via advocacy, community education, referrals, tutoring and transportation.'
			     }]}]");
        public static JArray reviewerData =
            JArray.Parse(@"[
                {
                    'reviewerFullName': 'John Doe',
                    'reviewerTitle': 'Director',
                    'reviewText': 'This is a my review of the organization',
                    'reviewerImage': 'url'
                }
            ]");
        private static JArray emptyData = JArray.Parse(@"[]");
        public static JArray emptyLocationData =
            JArray.Parse(@"[{'state':'','county':'','city':'','zipCode':''}]");
        public static JArray emptyReviewerData =
            JArray.Parse(@"[{'reviewerFullName':'','reviewerTitle':'','reviewText':'','reviewerImage':''}]");
        public static JArray availability =
            JArray.Parse(@"[{
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
            }]");
        public static JArray acceptanceCriteria =
            JArray.Parse(@"[{
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
            }]");
        public static JArray evaluatedRequirements =
            JArray.Parse(@"[
                    {
                        'condition': {
                            'displayLabel': 'Female',
                            'data': 'age',
                            'dataType': 2
                        },
                        'operatorName': 2,
                        'variable': '25'
                    }
                ]");
        public static JArray onboardingInfo =
            JArray.Parse(@"[{
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
          }]");
        //expected data
        public static JArray expectedServiceProviderdata =
            JArray.Parse(@"[{  'siteId': '7063',  'email': 'info@thecouplescenteralaska.org',  'availability': {    'regularBusinessHours': [      {        'day': 0,        'opensAt': '07:35:10',        'closesAt': '17:35:10'      }    ],    'holidayBusinessHours': [      {        'day': 0,        'opensAt': '10:35:10',        'closesAt': '17:35:10'      }    ],    'waitTime': '01:30:00'  },  'acceptanceCriteria': {    'description': 'Female under age 25',    'evaluatedRequirements': [      {        'condition': {          'displayLabel': 'Female',          'data': 'age',          'dataType': 2        },        'operatorName': 2,        'variable': '25'      },      {        'condition': {          'displayLabel': 'Female',          'data': 'age',          'dataType': 2        },        'operatorName': 0,        'variable': '25'      }    ]  },  'onboardingInfo': {    'userFields': [      {        'name': 'test1',        'value': 'val1'      },      {        'name': 'test2',        'value': 'val2'      }    ]  },  'address': '880 H Street, Suite 202, Anchorage, AK, 99501',  'telephone': '(907) 222-3699 Main | (907) 258-6631 Fax',  'overview': null,  'eligibilityInformation': null,  'reviewer': [    {      'reviewerFullName': 'John Doe',      'reviewerTitle': 'Director',      'reviewText': 'This is a my review of the organization',      'reviewerImage': 'url'    }  ],  'specialties': null,  'qualifications': null,  'businessHours': null,  'id': 'fdf5b9ed-7f0a-4544-9a00-302053d15dbb',  'name': 'COUPLES CENTER OF ALASKA (THE)',  'resourceCategory': null,  'description': 'Assists homeless children and adolescents to enroll in the Anchorage School District.  Supports attendance and success in school via advocacy, community education, referrals, tutoring and transportation.',  'resourceType': 'Service Providers',  'url': 'www.thecouplescenteralaska.org/index.html',  'topicTags': [    {      'id': 'e1fdbbc6-d66a-4275-9cd2-2be84d303e12'    }  ],  'organizationalUnit': 'AK',  'location': [    {      'state': 'AK',      'county': 'Anchorage',      'city': 'Anchorage',      'zipCode': '99501'    }  ],  'createdBy': 'Integration API',  'createdTimeStamp': '',  'modifiedBy': 'Integration API',  'modifiedTimeStamp': ''}]");
        public static JArray expectedReferencesData =
            JArray.Parse(@"[
          [
            {
              'reviewerFullName': 'John Doe',
              'reviewerTitle': 'Director',
              'reviewText': 'This is a my review of the organization',
              'reviewerImage': 'url'
            }
          ],
          {
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
          {
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
          {
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
          '880 H Street, Suite 202, Anchorage, AK, 99501',
          '(907) 222-3699 Main | (907) 258-6631 Fax',
          [
            {
              'state': 'AK',
              'county': 'Anchorage',
              'city': 'Anchorage',
              'zipCode': '99501'
            }
          ],
          'AK'
        ]");
        public static JArray expectedLocationData =
            JArray.Parse(@"[
              {
                'state': 'AK',
                'county': 'Anchorage',
                'city': 'Anchorage',
                'zipCode': '99501'
              }
            ]");        
        public static JArray expectedReviewer =
            JArray.Parse(@"[{
              'reviewerFullName': 'John Doe',
              'reviewerTitle': 'Director',
              'reviewText': 'This is a my review of the organization',
              'reviewerImage': 'url'
            }]");
        public static JArray expectedAvailability =
            JArray.Parse(@"[{
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
        }]");
        public static JArray expectedAcceptanceCriteria =
            JArray.Parse(@"[{
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
        }]");
        public static JArray expectedEvaluatedRequirements =
            JArray.Parse(@"[
            {
              'condition': {
                'displayLabel': 'Female',
                'data': 'age',
                'dataType': 2
              },
              'operatorName': 2,
              'variable': '25'
            }]");
        public static JArray expectedOnboardingInfo =
            JArray.Parse(@"[{
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
            }]");
        public static IEnumerable<object[]> ServiceProviderGetInputData()
        {
            yield return new object[] { serviceProviderId ,serviceProviderData, expectedServiceProviderdata };
            yield return new object[] { "test", JArray.Parse(@"[{}]"), JArray.Parse(@"[{}]") };
        }

        public static IEnumerable<object[]> ServiceProviderDeleteInputData()
        {
            yield return new object[] { serviceProviderId, "NoContent", "Record deleted successfully" };
            yield return new object[] { serviceProviderId, "Data not deleted", "Data not deleted" };
        }
        
        public static IEnumerable<object[]> ServiceProviderUpsertInputData()
        {
            yield return new object[] { "Family", topic, siteId, serviceProviderData, "fdf5b9ed-7f0a-4544-9a00-302053d15dbb", serviceProviderData, serviceProviderJson, providerDetailJson, expectedServiceProviderdata };
            yield return new object[] { "Family", topic, siteId, emptyData, "fdf5b9ed-7f0a-4544-9a00-302053d15dbb", serviceProviderData, serviceProviderJson, providerDetailJson, expectedServiceProviderdata };
        }
                       
        public static IEnumerable<object[]> ServiceProviderUpsertMethodInputData()
        {
            yield return new object[] { siteData, "fdf5b9ed-7f0a-4544-9a00-302053d15dbb", topicTag, "Assists homeless children and adolescents to enroll in the Anchorage School District.  Supports attendance and success in school via advocacy, community education, referrals, tutoring and transportation.", expectedServiceProviderdata };           
        }

        public static IEnumerable<object[]> GetServiceProviderReferencesInputData()
        {
            yield return new object[] { siteData, expectedReferencesData };
        }

        public static IEnumerable<object[]> GetServiceProviderAddressInputData()
        {
            yield return new object[] { siteAddress, "880 H Street, Suite 202, Anchorage, AK, 99501" };
        }

        public static IEnumerable<object[]> FormAddressInputData()
        {
            yield return new object[] { "880 H Street", "880 H Street, " };
        }

        public static IEnumerable<object[]> GetServiceProviderLocationInputData()
        {
            yield return new object[] { siteAddress, expectedLocationData };
        }

        public static IEnumerable<object[]> GetServiceProviderOrgUnitInputData()
        {
            yield return new object[] { siteAddress, "AK" };
        }

        public static IEnumerable<object[]> GetServiceProviderPhoneInputData()
        {
            yield return new object[] { sitePhone, "(907) 222-3699 Main | (907) 258-6631 Fax" };
        }

        public static IEnumerable<object[]> GetServiceProviderTopicTagsInputData()
        {
            yield return new object[] { "e1fdbbc6-d66a-4275-9cd2-2be84d303e12", JArray.Parse(@"[{ 'id': 'e1fdbbc6-d66a-4275-9cd2-2be84d303e12'}]") };
        }

        public static IEnumerable<object[]> GetServiceProviderDescriptionInputData()
        {
            yield return new object[] { siteId, providerJson, "Assists homeless children and adolescents to enroll in the Anchorage School District.  Supports attendance and success in school via advocacy, community education, referrals, tutoring and transportation." };
        }

        public static IEnumerable<object[]> GetReviewerInputData()
        {
            yield return new object[] { reviewerData, expectedReviewer };
        }

        public static IEnumerable<object[]> GetAvailabilityInputData()
        {
            yield return new object[] { availability, expectedAvailability };
        }

        public static IEnumerable<object[]> GetBusinessHoursInputData()
        {
            yield return new object[] { JArray.Parse(@"[{'day': 0,'opensAt': '07:35:10','closesAt': '17:35:10'}]"), JArray.Parse(@"[{'day': 0,'opensAt': '07:35:10','closesAt': '17:35:10'}]") };
        }

        public static IEnumerable<object[]> GetAcceptanceCriteriaInputData()
        {
            yield return new object[] { acceptanceCriteria, expectedAcceptanceCriteria };
        }

        public static IEnumerable<object[]> GetEvaluatedRequirementsInputData()
        {
            yield return new object[] { evaluatedRequirements, expectedEvaluatedRequirements };
        }

        public static IEnumerable<object[]> GetOnboardingInfoInputData()
        {
            yield return new object[] { onboardingInfo, expectedOnboardingInfo };
        }
    }
}
