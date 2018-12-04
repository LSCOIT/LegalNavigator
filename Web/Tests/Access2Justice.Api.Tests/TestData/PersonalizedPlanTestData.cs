using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Access2Justice.Shared.Models;
using System.Reflection.Metadata;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;

namespace Access2Justice.Api.Tests.TestData
{
    class PersonalizedPlanTestData
    {
        public static JArray curatedExperience = JArray.Parse(@"[{
  'id': '9a6a6131-657d-467d-b09b-c570b7dad242',
  'components': [
    {
      'componentId': '0173c6ca-2ba7-4d1d-94f5-7e3fec1f9dfc',
      'name': '1-Introduction',
      'text': 'This is the introduction. This A2J Guided Interview was created as part of a sample exercise. It is not intended for use by the general public.',
      'learn': '',
      'help': '',
      'tags': [],
      'buttons': [
        {
          'id': 'f3a1e369-edba-4152-bbc7-a52e8e77b56e',
          'label': 'Continue',
          'destination': '1-Name',
          'stepTitle': 'Make sure your summons is real',
          'stepDescription': 'Why you should do this dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo.    Call the court at 1-555-555-5555  If the summons is real prepare for your court date. Set up a reminder for your court date. Watch a video on how to approach this conversation:',
          'resourceIds': [
            '77d301e7-6df2-612e-4704-c04edf271806'
          ],
          'topicIds': [
            'addf41e9-1a27-4aeb-bcbb-7959f95094ba'
          ]
        }
      ],
      'fields': []
    },
    {
      'componentId': '4adec03b-4f9b-4bc9-bc44-27a8e84e30ae',
      'name': '1-Name',
      'text': 'Enter your name.',
      'learn': '',
      'help': '',
      'tags': [],
      'buttons': [
        {
          'id': '2b92e07b-a555-48e8-ad7b-90b99ebc5c96',
          'label': 'Continue',
          'destination': '3-Address',
          'stepTitle': 'Make sure your summons is real',
          'stepDescription': 'Why you should do this dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo.    Call the court at 1-555-555-5555  If the summons is real prepare for your court date. Set up a reminder for your court date. Watch a video on how to approach this conversation:',
          'resourceIds': [
            '752e3734-b9c2-4c86-bf9d-329e7d7ee820'
          ],
          'topicIds': [
            '932abb0a-c6bb-46da-a3d8-5f52c2c914a0'
          ]
        }
      ],
      'fields': [
        {
          'id': '22cbf2ac-a8d3-48c5-b230-297111e0e85c',
          'type': 'text',
          'label': 'First:',
          'isRequired': true,
          'min': '',
          'max': '',
          'invalidPrompt': 'You must type a response in the highlighted space before you can continue.'
        },
        {
          'id': '6c8312eb-131d-4cfb-a542-0e3f6d07a1d3',
          'type': 'text',
          'label': 'Middle:',
          'isRequired': false,
          'min': '',
          'max': '',
          'invalidPrompt': 'You must type a response in the highlighted space before you can continue.'
        },
        {
          'id': 'd2a935b4-bb07-494f-9c59-f3115b19d002',
          'type': 'text',
          'label': 'Last:',
          'isRequired': true,
          'min': '',
          'max': '',
          'invalidPrompt': 'You must type a response in the highlighted space before you can continue.'
        }
      ]
    },
    {
      'componentId': '3f2e2cdd-006e-4f4c-b0bf-32cea08f8e90',
      'name': '4-Phone number',
      'text': 'What is your phone number?',
      'learn': '',
      'help': '',
      'tags': [],
      'buttons': [
        {
          'id': 'aa3777d0-e7dd-48dd-afb3-6d61fc172768',
          'label': 'Continue',
          'destination': '1-Marital Status'
        }
      ],
      'fields': [
        {
          'id': 'd63e24a3-4a4d-4fec-a1aa-9c72b2fcab5e',
          'type': 'numberphone',
          'label': 'Phone number',
          'isRequired': false,
          'min': '',
          'max': '',
          'invalidPrompt': ''
        }
      ]
    },
    {
      'componentId': 'ff6fb136-1d62-4330-a82e-242e592d9d59',
      'name': '1-Purpose of form',
      'text': 'Why are you filling out this form?',
      'learn': '',
      'help': '',
      'tags': [],
      'buttons': [
        {
          'id': '98613482-53ad-458f-988c-5b473358c7bf',
          'label': 'Continue',
          'destination': '1-The End',
          'stepTitle': 'Submit a complaint for custody form',
          'stepDescription': 'Why you should do this dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo. Proin sodales pulvinar sic tempor.',
          'resourceIds': [
            '77d301e7-6df2-612e-4704-c04edf271806'
          ],
          'topicIds': [
            'addf41e9-1a27-4aeb-bcbb-7959f95094ba'
          ]
        }
      ],
      'fields': [
        {
          'id': 'cfd6fd1d-2974-43ef-8e3c-6fb0e9a0d8cf',
          'type': 'textlong',
          'label': '',
          'isRequired': false,
          'min': '',
          'max': '',
          'invalidPrompt': ''
        }
      ]
    }
  ]
}]");

        public static JArray userAnswers = JArray.Parse(@"[{
  'id': '288af4da-06bb-4655-aa91-41314e248d6b',
  'curatedExperienceId': '9a6a6131-657d-467d-b09b-c570b7dad242',
  'answers': [
    {
      'answerButtonId': 'f3a1e369-edba-4152-bbc7-a52e8e77b56e',
      'answerFields': []
    },
    {
      'answerButtonId': '2b92e07b-a555-48e8-ad7b-90b99ebc5c96',
      'answerFields': [
        {
          'fieldId': '22cbf2ac-a8d3-48c5-b230-297111e0e85c',
          'value': 'gh'
        },
        {
          'fieldId': '6c8312eb-131d-4cfb-a542-0e3f6d07a1d3',
          'value': 'fgh'
        },
        {
          'fieldId': 'd2a935b4-bb07-494f-9c59-f3115b19d002',
          'value': 'gh'
        }
      ]
    },
    {
      'answerButtonId': 'cafc4de0-91c5-4f67-a59f-bbffc2534236',
      'answerFields': [
        {
          'fieldId': '78da510a-180c-4757-b75d-fd87f470ae76',
          'value': 'fh'
        },
        {
          'fieldId': '6a81a78e-7692-4256-88d6-651dc4e9ce0f',
          'value': 'fh'
        },
        {
          'fieldId': '9471103f-7868-47fd-b1ab-581cc3934493',
          'value': '54'
        }
      ]
    },
    {
      'answerButtonId': 'aa3777d0-e7dd-48dd-afb3-6d61fc172768',
      'answerFields': [
        {
          'fieldId': 'd63e24a3-4a4d-4fec-a1aa-9c72b2fcab5e',
          'value': '46'
        }
      ]
    },
    {
      'answerButtonId': '5846cabb-1826-47bc-b2cd-6168f78fc4f2',
      'answerFields': [
        {
          'fieldId': '0bc5ccb7-e1d2-40b6-b20c-4fbd6cb85de9',
          'value': null
        }
      ]
    },
    {
      'answerButtonId': '98613482-53ad-458f-988c-5b473358c7bf',
      'answerFields': [
        {
          'fieldId': 'cfd6fd1d-2974-43ef-8e3c-6fb0e9a0d8cf',
          'value': '546h'
        }
      ]
    }
  ]
}]");

        public static JArray topicDetails = JArray.Parse(@"[
    {
      'id': 'e1fdbbc6-d66a-4275-9cd2-2be84d303e12',
      'name': 'Family',
      'overview': 'Overview of the Family topic',
      'quickLinks': [
        {
        'Text': 'Filing for Dissolution or Divorce - Ending Your Marriage',
        'Url': 'http://courts.alaska.gov/shc/family/shcstart.htm#issues'
        },
        {
        'Text': 'Spousal Support (Alimony)',
        'Url': 'https://alaskalawhelp.org/resource/spousal-support?ref=OEGQX'
    }
      ],
      'icon': 'https://cs4892808efec24x447cx944.blob.core.windows.net/static-resource/assets/images/categories/family.svg',
      'parentTopicId': [],
      'resourceType': 'Topics',
      'keywords': 'Family',
      'location': [
        {
          'state': 'Hawaii',
          'city': 'Kalawao',
          'zipCode': '96761'
        },
        {
          'state': 'Hawaii',
          'city': 'Honolulu',
          'zipCode': '96741'
        },
        {
          'state': 'Alaska',
          'city': 'Juneau',
          'zipCode': '96815'
        },
        {
          'state': 'Alaska',
          'city': 'Anchorage',
          'zipCode': '99507'
        }
      ]
    },
    {
      'id': 'd1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef',
      'name': 'Divorce',
      'overview': 'Overview of the Divorce topic',
      'quickLinks': [
        {
          'text': 'Filing for Dissolution or Divorce - Ending Your Marriage',
          'url': 'http://courts.alaska.gov/shc/family/shcstart.htm#issues'
        },
        {
          'text': 'Spousal Support (Alimony)',
          'url': 'https://alaskalawhelp.org/resource/spousal-support?ref=OEGQX'
        }
      ],
      'parentTopicId': [ { 'id': 'e1fdbbc6-d66a-4275-9cd2-2be84d303e12' } ],
      'resourceType': 'Topics',
      'keywords': 'Divorce | Child',
      'location': [
        {
          'state': 'Hawaii',
          'city': 'Kalawao',
          'zipCode': '96761'
        },
        {
          'state': 'Hawaii',
          'city': 'Honolulu',
          'zipCode': '96741'
        },
        {
          'state': 'Alaska',
          'city': 'Juneau',
          'zipCode': '96815'
        },
        {
          'state': 'Alaska',
          'city': 'Anchorage',
          'zipCode': '99507'
        }
      ]
    }
]");

        public static JArray planSteps = JArray.Parse(@"[
  {
    'stepId': '296ee8be-5b9e-43b4-8d86-52a897afe580',
    'title': 'File a motion to modify if there has been a change of circumstances.',
    'description': 'The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.',
    'order': 1,
    'isComplete': false,
    'resources': [
      '19a02209-ca38-4b74-bd67-6ea941d41518',
      '9ca4cf73-f6c0-4f63-a1e8-2a3774961df5',
      '49779468-1fe0-4183-850b-ff365e05893e'
    ],
'topicIds': [
            'e1fdbbc6-d66a-4275-9cd2-2be84d303e12'
          ]
  },
  {
    'stepId': '9dffb34a-f62e-40cb-a29e-cf28cee83955',
    'title': 'Jurisdiction',
    'description': 'Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:',
    'order': 1,
    'isComplete': false,
    'resources': [
      '4f327a5b-9a79-466e-98be-2542eb28bb28',
      'be0cb3e1-7054-403a-baac-d119ea5be007',
      '2cb89c44-f989-42ea-b038-419e5006e8ff',
      '2fe9f117-bfb5-469f-b80c-877640a29f75'
    ],
    'topicIds': [
            'e1fdbbc6-d66a-4275-9cd2-2be84d303e12'
          ]
  },
  {
    'stepId': 'afe73385-7e6c-4eac-b50f-2374d457a239',
    'title': 'Pre-Decree Relief',
    'description': 'Read instructions on how to fill out a request for pre-decree relief. Fill out the form and serve the other side with a copy. Either a family court clerk or a family court judge will sign the Order for Pre-Decree Relief depending on the types of relief being requested.',
    'order': 1,
    'isComplete': false,
    'resources': [
      'b0dc6222-a0a8-4927-a398-ccf0d6524593',
      '78a4da0f-3087-4513-8b45-a486919d2ed5',
      'ce2c727a-1035-4614-b6e8-a161141f245e',
      '9b8bd927-ad8f-416f-9d5f-82ee96925357',
      'bbb88efb-3816-218b-5b38-1281e1b77f5a'
    ],
    'topicIds': [
            'e1fdbbc6-d66a-4275-9cd2-2be84d303e12'
          ]
  }
]");

        public static JArray personalizedPlan = JArray.Parse(@"[{
  'id': '86e693bd-c673-419f-95c4-9ee76cccab3c',
  'topics': [
    {
      'topicId': 'd1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef',
      'steps': [
        {
          'stepId': '5f1cdda9-0b04-4058-b1bc-cee053795879',
          'title': 'File a motion to modify if there has been a change of circumstances.',
          'description': 'The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.',
          'order': 1,
          'isComplete': false,
          'resources': [ '19a02209-ca38-4b74-bd67-6ea941d41518', '9ca4cf73-f6c0-4f63-a1e8-2a3774961df5', '49779468-1fe0-4183-850b-ff365e05893e' ]
        },
        {
          'stepId': 'dc10ae41-cdae-43d3-bee2-e43aef692d1e',
          'title': 'Jurisdiction',
          'description': 'Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:',
          'order': 2,
          'isComplete': false,
          'resources': [ '4f327a5b-9a79-466e-98be-2542eb28bb28', 'be0cb3e1-7054-403a-baac-d119ea5be007', '2cb89c44-f989-42ea-b038-419e5006e8ff', '2fe9f117-bfb5-469f-b80c-877640a29f75' ]
        },
        {
          'stepId': 'ee872b29-5b71-4687-8f66-4bfff8f209f4',
          'title': 'Pre-Decree Relief',
          'description': 'Read instructions on how to fill out a request for pre-decree relief. Fill out the form and serve the other side with a copy. Either a family court clerk or a family court judge will sign the Order for Pre-Decree Relief depending on the types of relief being requested.',
          'order': 3,
          'isComplete': false,
          'resources': [ 'b0dc6222-a0a8-4927-a398-ccf0d6524593', '78a4da0f-3087-4513-8b45-a486919d2ed5', 'ce2c727a-1035-4614-b6e8-a161141f245e', '9b8bd927-ad8f-416f-9d5f-82ee96925357', 'bbb88efb-3816-218b-5b38-1281e1b77f5a' ]
        }
      ]
    }
  ],
  'isShared': false
}]");

        public static JArray dynamicObject = JArray.Parse(@"[
  {
    'id': 'be05326c-c9d0-46cb-854b-d0a36c8b3c73',
    'topics': [
      {
        'topicId': 'd1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef',
        'steps': [
          {
            'stepId': '603009e3-19da-44cc-b590-ff93ca1a8a71',
            'title': 'File a motion to modify if there has been a change of circumstances.',
            'description': 'The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.',
            'order': 1,
            'isComplete': false,
            'resources': [ '19a02209-ca38-4b74-bd67-6ea941d41518', '9ca4cf73-f6c0-4f63-a1e8-2a3774961df5', '49779468-1fe0-4183-850b-ff365e05893e' ]
          },
          {
            'stepId': 'aaad6c38-2681-43b5-95f7-35d16f22f47c',
            'title': 'Jurisdiction',
            'description': 'Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:',
            'order': 2,
            'isComplete': false,
            'resources': [ '4f327a5b-9a79-466e-98be-2542eb28bb28', 'be0cb3e1-7054-403a-baac-d119ea5be007', '2cb89c44-f989-42ea-b038-419e5006e8ff', '2fe9f117-bfb5-469f-b80c-877640a29f75' ]
          },
          {
            'stepId': '76fd7499-6797-4791-af5f-4bb4c0e3846e',
            'title': 'Pre-Decree Relief',
            'description': 'Read instructions on how to fill out a request for pre-decree relief. Fill out the form and serve the other side with a copy. Either a family court clerk or a family court judge will sign the Order for Pre-Decree Relief depending on the types of relief being requested.',
            'order': 3,
            'isComplete': false,
            'resources': [ 'b0dc6222-a0a8-4927-a398-ccf0d6524593', '78a4da0f-3087-4513-8b45-a486919d2ed5', 'ce2c727a-1035-4614-b6e8-a161141f245e', '9b8bd927-ad8f-416f-9d5f-82ee96925357', 'bbb88efb-3816-218b-5b38-1281e1b77f5a' ]
          }
        ]
      }
    ],
    'isShared': false
  }
]");

        public static JArray convertedPersonalizedPlanSteps = JArray.Parse(@"[{
  'id': 'be05326c-c9d0-46cb-854b-d0a36c8b3c73',
  'topics': [
    {
      'topicId': 'd1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef',
      'steps': [
        {
          'stepId': '603009e3-19da-44cc-b590-ff93ca1a8a71',
          'title': 'File a motion to modify if there has been a change of circumstances.',
          'description': 'The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.',
          'order': 1,
          'isComplete': false,
          'resources': [ '19a02209-ca38-4b74-bd67-6ea941d41518', '9ca4cf73-f6c0-4f63-a1e8-2a3774961df5', '49779468-1fe0-4183-850b-ff365e05893e' ],
          'topicIds': ['d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef']
        },
        {
          'stepId': 'aaad6c38-2681-43b5-95f7-35d16f22f47c',
          'title': 'Jurisdiction',
          'description': 'Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:',
          'order': 2,
          'isComplete': false,
          'resources': [ '4f327a5b-9a79-466e-98be-2542eb28bb28', 'be0cb3e1-7054-403a-baac-d119ea5be007', '2cb89c44-f989-42ea-b038-419e5006e8ff', '2fe9f117-bfb5-469f-b80c-877640a29f75' ],
          'topicIds': ['d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef']
        },
        {
          'stepId': '76fd7499-6797-4791-af5f-4bb4c0e3846e',
          'title': 'Pre-Decree Relief',
          'description': 'Read instructions on how to fill out a request for pre-decree relief. Fill out the form and serve the other side with a copy. Either a family court clerk or a family court judge will sign the Order for Pre-Decree Relief depending on the types of relief being requested.',
          'order': 3,
          'isComplete': false,
          'resources': [ 'b0dc6222-a0a8-4927-a398-ccf0d6524593', '78a4da0f-3087-4513-8b45-a486919d2ed5', 'ce2c727a-1035-4614-b6e8-a161141f245e', '9b8bd927-ad8f-416f-9d5f-82ee96925357', 'bbb88efb-3816-218b-5b38-1281e1b77f5a' ],
          'topicIds': ['d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef']
        }   
      ]
    }
  ],
  'isShared': false
}]");

        public static JArray personalizedPlanView = JArray.Parse(@"[{
  'id': 'a9f6c0db-093b-4fa8-9cde-2495087b31f6',
  'topics': [
    {
      'topicId': 'd1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef',
      'name': 'Divorce',
      'quickLinks': [
        {
          'Text': 'Filing for Dissolution or Divorce - Ending Your Marriage',
          'Url': 'http://courts.alaska.gov/shc/family/shcstart.htm#issues'
        },
        {
          'Text': 'Spousal Support (Alimony)',
          'Url': 'https://alaskalawhelp.org/resource/spousal-support?ref=OEGQX'
        }
      ],
      'icon': 'https://cs4892808efec24x447cx944.blob.core.windows.net/static-resource/assets/images/topics/housing.svg',
      'steps': [
        {
          'stepId': '66a1e288-4ab1-480f-9d29-eb16abd3ae69',
          'type': 'steps',
          'title': 'File a motion to modify if there has been a change of circumstances.',
          'description': 'The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.',
          'order': 1,
          'isComplete': false,
          'resources': [
            {
              'id': '19a02209-ca38-4b74-bd67-6ea941d41518',
              'name': 'Alaska Law Help',
              'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
              'resourceType': 'Organizations',
              'externalUrl': null,
              'url': 'https://alaskalawhelp.org/',
              'topicTags': null,
              'location': [
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Kalawao',
                  'zipCode': '96761'
                },
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Honolulu',
                  'zipCode': '96741'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Juneau',
                  'zipCode': '96815'
                },
                {
                  'state': 'Alaska',
                  'county': 'Matanuska-Susitna Borough',
                  'city': 'Wasilla',
                  'zipCode': null
                }
              ],
              'icon': './assets/images/resources/resource.png',
              'createdBy': '',
              'createdTimeStamp': null,
              'modifiedBy': '',
              'modifiedTimeStamp': '2018-04-01T04:18:00Z'
            },
            {
              'id': '9ca4cf73-f6c0-4f63-a1e8-2a3774961df5',
              'name': 'Alaska Legal Services - Bristol Bay Office',
              'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
              'resourceType': 'Organizations',
              'externalUrl': null,
              'url': 'https://alaskalawhelp.org/',
              'topicTags': null,
              'location': [
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Kalawao',
                  'zipCode': '96761'
                },
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Honolulu',
                  'zipCode': '96741'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Juneau',
                  'zipCode': '96815'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Anchorage',
                  'zipCode': '99507'
                }
              ],
              'icon': './assets/images/resources/resource.png',
              'createdBy': '',
              'createdTimeStamp': null,
              'modifiedBy': '',
              'modifiedTimeStamp': '2018-04-01T04:18:00Z'
            },
            {
              'id': '49779468-1fe0-4183-850b-ff365e05893e',
              'name': 'Alaska Native Justice Center',
              'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
              'resourceType': 'Organizations',
              'externalUrl': null,
              'url': 'https://alaskalawhelp.org/',
              'topicTags': null,
              'location': [
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Kalawao',
                  'zipCode': '96761'
                },
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Honolulu',
                  'zipCode': '96741'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Juneau',
                  'zipCode': '96815'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Anchorage',
                  'zipCode': '99507'
                }
              ],
              'icon': './assets/images/resources/resource.png',
              'createdBy': '',
              'createdTimeStamp': null,
              'modifiedBy': '',
              'modifiedTimeStamp': '2018-04-01T04:18:00Z'
            }
          ]
        },
        {
          'stepId': '38f04325-af65-48ea-a418-d34dc477b753',
          'type': 'steps',
          'title': 'Jurisdiction',
          'description': 'Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:',
          'order': 2,
          'isComplete': false,
          'resources': [
            {
              'id': '4f327a5b-9a79-466e-98be-2542eb28bb28',
              'name': 'Guardian ad Litem or Custody Investigator?',
              'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
              'resourceType': 'Articles',
              'externalUrl': null,
              'url': 'www.courtrecords.alaska.gov',
              'topicTags': null,
              'location': [
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Kalawao',
                  'zipCode': '96761'
                },
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Honolulu',
                  'zipCode': '96741'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Juneau',
                  'zipCode': '96815'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Anchorage',
                  'zipCode': '99507'
                }
              ],
              'icon': './assets/images/resources/resource.png',
              'createdBy': 'Alaska Court System Administrative Office',
              'createdTimeStamp': null,
              'modifiedBy': '',
              'modifiedTimeStamp': '2018-04-01T04:18:00Z'
            },
            {
              'id': 'be0cb3e1-7054-403a-baac-d119ea5be007',
              'name': 'Filing Documents by Mail',
              'description': 'Formal Application Process',
              'resourceType': 'Videos',
              'externalUrl': null,
              'url': 'https://www.youtube.com/embed/pCPGSTYsYoU',
              'topicTags': null,
              'location': [
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Kalawao',
                  'zipCode': '96761'
                },
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Honolulu',
                  'zipCode': '96741'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Juneau',
                  'zipCode': '96815'
                },
                {
                  'state': 'Alaska',
                  'county': 'Matanuska-Susitna Borough',
                  'city': 'Wasilla',
                  'zipCode': '99654'
                }
              ],
              'icon': './assets/images/resources/resource.png',
              'createdBy': '',
              'createdTimeStamp': null,
              'modifiedBy': '',
              'modifiedTimeStamp': '2018-04-01T04:18:00Z'
            },
            {
              'id': '2cb89c44-f989-42ea-b038-419e5006e8ff',
              'name': 'Innocent Spouse Relief',
              'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
              'resourceType': 'Articles',
              'externalUrl': null,
              'url': 'www.courtrecords.alaska.gov',
              'topicTags': null,
              'location': [
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Kalawao',
                  'zipCode': '96761'
                },
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Honolulu',
                  'zipCode': '96741'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Juneau',
                  'zipCode': '96815'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Anchorage',
                  'zipCode': '99507'
                }
              ],
              'icon': './assets/images/resources/resource.png',
              'createdBy': 'Alaska Court System Administrative Office',
              'createdTimeStamp': null,
              'modifiedBy': '',
              'modifiedTimeStamp': '2018-04-01T04:18:00Z'
            },
            {
              'id': '2fe9f117-bfb5-469f-b80c-877640a29f75',
              'name': 'Behavior in Court',
              'description': 'Courtroom Etiquette',
              'resourceType': 'Videos',
              'externalUrl': null,
              'url': 'https://www.youtube.com/embed/pCPGSTYsYoU',
              'topicTags': null,
              'location': [
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Kalawao',
                  'zipCode': '96761'
                },
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Honolulu',
                  'zipCode': '96741'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Juneau',
                  'zipCode': '96815'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Anchorage',
                  'zipCode': '99507'
                }
              ],
              'icon': './assets/images/resources/resource.png',
              'createdBy': '',
              'createdTimeStamp': null,
              'modifiedBy': '',
              'modifiedTimeStamp': '2018-04-01T04:18:00Z'
            }
          ]
        },
        {
          'stepId': 'da519dde-3710-4515-aac1-65a576cdcc8d',
          'type': 'steps',
          'title': 'Pre-Decree Relief',
          'description': 'Read instructions on how to fill out a request for pre-decree relief. Fill out the form and serve the other side with a copy. Either a family court clerk or a family court judge will sign the Order for Pre-Decree Relief depending on the types of relief being requested.',
          'order': 3,
          'isComplete': false,
          'resources': [
            {
              'id': 'b0dc6222-a0a8-4927-a398-ccf0d6524593',
              'name': 'Common Questions About Child Support',
              'description': 'FAQs About Child Support',
              'resourceType': 'Additional Readings',
              'externalUrl': null,
              'url': 'https://alaskalawhelp.org/resource/common-questions-about-child-support?ref=vhU5v',
              'topicTags': null,
              'location': [
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Kalawao',
                  'zipCode': '96761'
                },
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Honolulu',
                  'zipCode': '96741'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Juneau',
                  'zipCode': '96815'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Anchorage',
                  'zipCode': '99507'
                }
              ],
              'icon': './assets/images/resources/resource.png',
              'createdBy': '',
              'createdTimeStamp': null,
              'modifiedBy': '',
              'modifiedTimeStamp': '2018-04-01T04:18:00Z'
            },
            {
              'id': '78a4da0f-3087-4513-8b45-a486919d2ed5',
              'name': 'My PFD Was Taken for Child Support, But I Have Custody of the Child',
              'description': 'FAQs About Child Support',
              'resourceType': 'Additional Readings',
              'externalUrl': null,
              'url': 'https://alaskalawhelp.org/resource/my-pfd-was-taken-for-child-support-but-i-have?ref=vhU5v',
              'topicTags': null,
              'location': [
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Kalawao',
                  'zipCode': '96761'
                },
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Honolulu',
                  'zipCode': '96741'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Juneau',
                  'zipCode': '96815'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Anchorage',
                  'zipCode': '99507'
                }
              ],
              'icon': './assets/images/resources/resource.png',
              'createdBy': '',
              'createdTimeStamp': null,
              'modifiedBy': '',
              'modifiedTimeStamp': '2018-04-01T04:18:00Z'
            },
            {
              'id': 'ce2c727a-1035-4614-b6e8-a161141f245e',
              'name': 'Who is the Father?',
              'description': 'FAQs About Child Support',
              'resourceType': 'Additional Readings',
              'externalUrl': null,
              'url': 'https://alaskalawhelp.org/resource/how-do-i-establish-paternity-of-a-child?ref=vhU5v',
              'topicTags': null,
              'location': [
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Kalawao',
                  'zipCode': '96761'
                },
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Honolulu',
                  'zipCode': '96741'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Juneau',
                  'zipCode': '96815'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Anchorage',
                  'zipCode': '99507'
                }
              ],
              'icon': './assets/images/resources/resource.png',
              'createdBy': '',
              'createdTimeStamp': null,
              'modifiedBy': '',
              'modifiedTimeStamp': '2018-04-01T04:18:00Z'
            },
            {
              'id': '9b8bd927-ad8f-416f-9d5f-82ee96925357',
              'name': 'Jurisdiction of my interview',
              'description': 'This A2J Guided Interview was created to demonstrate that A2J Author can be used to create a curated experience around divorce for Alaska and Hawaii',
              'resourceType': 'Guided Assistant',
              'externalUrl': '9a6a6131-657d-467d-b09b-c570b7dad242',
              'url': null,
              'topicTags': null,
              'location': [
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Kalawao',
                  'zipCode': '96761'
                },
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Honolulu',
                  'zipCode': '96741'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Juneau',
                  'zipCode': '96815'
                },
                {
                  'state': 'Alaska',
                  'county': null,
                  'city': 'Anchorage',
                  'zipCode': '99507'
                },
                {
                  'state': 'New York',
                  'county': null,
                  'city': 'EAST MEADOW',
                  'zipCode': '11554'
                },
                {
                  'state': 'NY',
                  'county': null,
                  'city': 'EAST MEADOW',
                  'zipCode': '11554'
                },
                {
                  'state': 'Washington',
                  'county': null,
                  'city': 'Redmond',
                  'zipCode': '98052'
                }
              ],
              'icon': null,
              'createdBy': null,
              'createdTimeStamp': null,
              'modifiedBy': null,
              'modifiedTimeStamp': null
            },
            {
              'id': 'bbb88efb-3816-218b-5b38-1281e1b77f5a',
              'name': 'Hawaii Organization Test',
              'description': 'HawaiiLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
              'resourceType': 'Organizations',
              'externalUrl': null,
              'url': 'https://alaskalawhelp.org/',
              'topicTags': null,
              'location': [
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Kalawao',
                  'zipCode': '96761'
                },
                {
                  'state': 'Hawaii',
                  'county': null,
                  'city': 'Honolulu',
                  'zipCode': '96741'
                }
              ],
              'icon': './assets/images/resources/resource.png',
              'createdBy': '',
              'createdTimeStamp': null,
              'modifiedBy': '',
              'modifiedTimeStamp': '2018-04-01T04:18:00Z'
            }
          ]
        }
      ]
    }
  ],
  'isShared': false
}]");

        public static JArray quickLinks = JArray.Parse(@"[
  {
    'Text': 'Filing for Dissolution or Divorce - Ending Your Marriage',
    'Url': 'http://courts.alaska.gov/shc/family/shcstart.htm#issues'
  },
  {
    'Text': 'Spousal Support (Alimony)',
    'Url': 'https://alaskalawhelp.org/resource/spousal-support?ref=OEGQX'
  }
]");

        public static JArray resourceDetails = JArray.Parse(@"[
  {
    'id': '19a02209-ca38-4b74-bd67-6ea941d41518',
    'name': 'Alaska Law Help',
    'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
    'resourceType': 'Organizations',
    'externalUrl': null,
    'url': 'https://alaskalawhelp.org/',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': 'Matanuska-Susitna Borough',
        'city': 'Wasilla',
        'zipCode': null
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': '',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  },
  {
    'id': '9ca4cf73-f6c0-4f63-a1e8-2a3774961df5',
    'name': 'Alaska Legal Services - Bristol Bay Office',
    'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
    'resourceType': 'Organizations',
    'externalUrl': null,
    'url': 'https://alaskalawhelp.org/',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Anchorage',
        'zipCode': '99507'
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': '',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  },
  {
    'id': '49779468-1fe0-4183-850b-ff365e05893e',
    'name': 'Alaska Native Justice Center',
    'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
    'resourceType': 'Organizations',
    'externalUrl': null,
    'url': 'https://alaskalawhelp.org/',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Anchorage',
        'zipCode': '99507'
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': '',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  },
  {
    'id': '4f327a5b-9a79-466e-98be-2542eb28bb28',
    'name': 'Guardian ad Litem or Custody Investigator?',
    'description': 'HawaiiLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
    'resourceType': 'Articles',
    'externalUrl': null,
    'url': 'www.courtrecords.alaska.gov',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Anchorage',
        'zipCode': '99507'
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': 'Alaska Court System Administrative Office',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  },
  {
    'id': '2cb89c44-f989-42ea-b038-419e5006e8ff',
    'name': 'Innocent Spouse Relief',
    'description': 'HawaiiLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
    'resourceType': 'Articles',
    'externalUrl': null,
    'url': 'www.courtrecords.alaska.gov',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Anchorage',
        'zipCode': '99507'
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': 'Alaska Court System Administrative Office',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  },
  {
    'id': '2fe9f117-bfb5-469f-b80c-877640a29f75',
    'name': 'Behavior in Court',
    'description': 'Courtroom Etiquette',
    'resourceType': 'Videos',
    'externalUrl': null,
    'url': 'https://www.youtube.com/embed/pCPGSTYsYoU',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Anchorage',
        'zipCode': '99507'
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': '',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  },
  {
    'id': 'be0cb3e1-7054-403a-baac-d119ea5be007',
    'name': 'Filing Documents by Mail',
    'description': 'Formal Application Process',
    'resourceType': 'Videos',
    'externalUrl': null,
    'url': 'https://www.youtube.com/embed/pCPGSTYsYoU',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': 'Matanuska-Susitna Borough',
        'city': 'Wasilla',
        'zipCode': '99654'
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': '',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  },
  {
    'id': 'b0dc6222-a0a8-4927-a398-ccf0d6524593',
    'name': 'Common Questions About Child Support',
    'description': 'FAQs About Child Support',
    'resourceType': 'Additional Readings',
    'externalUrl': null,
    'url': 'https://alaskalawhelp.org/resource/common-questions-about-child-support?ref=vhU5v',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Anchorage',
        'zipCode': '99507'
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': '',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  },
  {
    'id': '78a4da0f-3087-4513-8b45-a486919d2ed5',
    'name': 'My PFD Was Taken for Child Support, But I Have Custody of the Child',
    'description': 'FAQs About Child Support',
    'resourceType': 'Additional Readings',
    'externalUrl': null,
    'url': 'https://alaskalawhelp.org/resource/my-pfd-was-taken-for-child-support-but-i-have?ref=vhU5v',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Anchorage',
        'zipCode': '99507'
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': '',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  },
  {
    'id': 'ce2c727a-1035-4614-b6e8-a161141f245e',
    'name': 'Who is the Father?',
    'description': 'FAQs About Child Support',
    'resourceType': 'Additional Readings',
    'externalUrl': null,
    'url': 'https://alaskalawhelp.org/resource/how-do-i-establish-paternity-of-a-child?ref=vhU5v',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Anchorage',
        'zipCode': '99507'
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': '',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  },
  {
    'id': '9b8bd927-ad8f-416f-9d5f-82ee96925357',
    'name': 'Jurisdiction of my interview',
    'description': 'This A2J Guided Interview was created to demonstrate that A2J Author can be used to create a curated experience around divorce for Alaska and Hawaii',
    'resourceType': 'Guided Assistant',
    'externalUrl': '9a6a6131-657d-467d-b09b-c570b7dad242',
    'url': null,
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Anchorage',
        'zipCode': '99507'
      },
      {
        'state': 'New York',
        'county': null,
        'city': 'EAST MEADOW',
        'zipCode': '11554'
      },
      {
        'state': 'NY',
        'county': null,
        'city': 'EAST MEADOW',
        'zipCode': '11554'
      },
      {
        'state': 'Washington',
        'county': null,
        'city': 'Redmond',
        'zipCode': '98052'
      }
    ],
    'icon': null,
    'createdBy': null,
    'createdTimeStamp': null,
    'modifiedBy': null,
    'modifiedTimeStamp': null
  },
  {
    'id': 'bbb88efb-3816-218b-5b38-1281e1b77f5a',
    'name': 'Hawaii Organization Test',
    'description': 'HawaiiLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
    'resourceType': 'Organizations',
    'externalUrl': null,
    'url': 'https://alaskalawhelp.org/',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': '',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  }
]");
        public static JArray planStepsWithResourceDetails = JArray.Parse(@"[
  {
    'stepId': '300ff138-a83f-4898-ab3a-d24cb48d357b',
    'type': 'steps',
    'title': 'File a motion to modify if there has been a change of circumstances.',
    'description': 'The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.',
    'order': 1,
    'isComplete': false,
    'resources': [
      {
        'id': '19a02209-ca38-4b74-bd67-6ea941d41518',
        'name': 'Alaska Law Help',
        'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
        'resourceType': 'Organizations',
        'externalUrl': null,
        'url': 'https://alaskalawhelp.org/',
        'topicTags': null,
        'location': [
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Kalawao',
            'zipCode': '96761'
          },
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Honolulu',
            'zipCode': '96741'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Juneau',
            'zipCode': '96815'
          },
          {
            'state': 'Alaska',
            'county': 'Matanuska-Susitna Borough',
            'city': 'Wasilla',
            'zipCode': null
          }
        ],
        'icon': './assets/images/resources/resource.png',
        'createdBy': '',
        'createdTimeStamp': null,
        'modifiedBy': '',
        'modifiedTimeStamp': '2018-04-01T04:18:00Z'
      },
      {
        'id': '9ca4cf73-f6c0-4f63-a1e8-2a3774961df5',
        'name': 'Alaska Legal Services - Bristol Bay Office',
        'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
        'resourceType': 'Organizations',
        'externalUrl': null,
        'url': 'https://alaskalawhelp.org/',
        'topicTags': null,
        'location': [
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Kalawao',
            'zipCode': '96761'
          },
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Honolulu',
            'zipCode': '96741'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Juneau',
            'zipCode': '96815'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Anchorage',
            'zipCode': '99507'
          }
        ],
        'icon': './assets/images/resources/resource.png',
        'createdBy': '',
        'createdTimeStamp': null,
        'modifiedBy': '',
        'modifiedTimeStamp': '2018-04-01T04:18:00Z'
      },
      {
        'id': '49779468-1fe0-4183-850b-ff365e05893e',
        'name': 'Alaska Native Justice Center',
        'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
        'resourceType': 'Organizations',
        'externalUrl': null,
        'url': 'https://alaskalawhelp.org/',
        'topicTags': null,
        'location': [
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Kalawao',
            'zipCode': '96761'
          },
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Honolulu',
            'zipCode': '96741'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Juneau',
            'zipCode': '96815'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Anchorage',
            'zipCode': '99507'
          }
        ],
        'icon': './assets/images/resources/resource.png',
        'createdBy': '',
        'createdTimeStamp': null,
        'modifiedBy': '',
        'modifiedTimeStamp': '2018-04-01T04:18:00Z'
      }
    ]
  },
  {
    'stepId': 'd6b123e7-b1d3-4193-ac59-2aa27241edf4',
    'type': 'steps',
    'title': 'Jurisdiction',
    'description': 'Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:',
    'order': 2,
    'isComplete': false,
    'resources': [
      {
        'id': '4f327a5b-9a79-466e-98be-2542eb28bb28',
        'name': 'Guardian ad Litem or Custody Investigator?',
        'description': 'HawaiiLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
        'resourceType': 'Articles',
        'externalUrl': null,
        'url': 'www.courtrecords.alaska.gov',
        'topicTags': null,
        'location': [
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Kalawao',
            'zipCode': '96761'
          },
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Honolulu',
            'zipCode': '96741'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Juneau',
            'zipCode': '96815'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Anchorage',
            'zipCode': '99507'
          }
        ],
        'icon': './assets/images/resources/resource.png',
        'createdBy': 'Alaska Court System Administrative Office',
        'createdTimeStamp': null,
        'modifiedBy': '',
        'modifiedTimeStamp': '2018-04-01T04:18:00Z'
      },
      {
        'id': 'be0cb3e1-7054-403a-baac-d119ea5be007',
        'name': 'Filing Documents by Mail',
        'description': 'Formal Application Process',
        'resourceType': 'Videos',
        'externalUrl': null,
        'url': 'https://www.youtube.com/embed/pCPGSTYsYoU',
        'topicTags': null,
        'location': [
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Kalawao',
            'zipCode': '96761'
          },
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Honolulu',
            'zipCode': '96741'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Juneau',
            'zipCode': '96815'
          },
          {
            'state': 'Alaska',
            'county': 'Matanuska-Susitna Borough',
            'city': 'Wasilla',
            'zipCode': '99654'
          }
        ],
        'icon': './assets/images/resources/resource.png',
        'createdBy': '',
        'createdTimeStamp': null,
        'modifiedBy': '',
        'modifiedTimeStamp': '2018-04-01T04:18:00Z'
      },
      {
        'id': '2cb89c44-f989-42ea-b038-419e5006e8ff',
        'name': 'Innocent Spouse Relief',
        'description': 'HawaiiLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
        'resourceType': 'Articles',
        'externalUrl': null,
        'url': 'www.courtrecords.alaska.gov',
        'topicTags': null,
        'location': [
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Kalawao',
            'zipCode': '96761'
          },
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Honolulu',
            'zipCode': '96741'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Juneau',
            'zipCode': '96815'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Anchorage',
            'zipCode': '99507'
          }
        ],
        'icon': './assets/images/resources/resource.png',
        'createdBy': 'Alaska Court System Administrative Office',
        'createdTimeStamp': null,
        'modifiedBy': '',
        'modifiedTimeStamp': '2018-04-01T04:18:00Z'
      },
      {
        'id': '2fe9f117-bfb5-469f-b80c-877640a29f75',
        'name': 'Behavior in Court',
        'description': 'Courtroom Etiquette',
        'resourceType': 'Videos',
        'externalUrl': null,
        'url': 'https://www.youtube.com/embed/pCPGSTYsYoU',
        'topicTags': null,
        'location': [
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Kalawao',
            'zipCode': '96761'
          },
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Honolulu',
            'zipCode': '96741'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Juneau',
            'zipCode': '96815'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Anchorage',
            'zipCode': '99507'
          }
        ],
        'icon': './assets/images/resources/resource.png',
        'createdBy': '',
        'createdTimeStamp': null,
        'modifiedBy': '',
        'modifiedTimeStamp': '2018-04-01T04:18:00Z'
      }
    ]
  },
  {
    'stepId': '8fbd7a8c-dacf-4a2d-9d58-04dd6049d717',
    'type': 'steps',
    'title': 'Pre-Decree Relief',
    'description': 'Read instructions on how to fill out a request for pre-decree relief. Fill out the form and serve the other side with a copy. Either a family court clerk or a family court judge will sign the Order for Pre-Decree Relief depending on the types of relief being requested.',
    'order': 3,
    'isComplete': false,
    'resources': [
      {
        'id': 'b0dc6222-a0a8-4927-a398-ccf0d6524593',
        'name': 'Common Questions About Child Support',
        'description': 'FAQs About Child Support',
        'resourceType': 'Additional Readings',
        'externalUrl': null,
        'url': 'https://alaskalawhelp.org/resource/common-questions-about-child-support?ref=vhU5v',
        'topicTags': null,
        'location': [
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Kalawao',
            'zipCode': '96761'
          },
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Honolulu',
            'zipCode': '96741'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Juneau',
            'zipCode': '96815'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Anchorage',
            'zipCode': '99507'
          }
        ],
        'icon': './assets/images/resources/resource.png',
        'createdBy': '',
        'createdTimeStamp': null,
        'modifiedBy': '',
        'modifiedTimeStamp': '2018-04-01T04:18:00Z'
      },
      {
        'id': '78a4da0f-3087-4513-8b45-a486919d2ed5',
        'name': 'My PFD Was Taken for Child Support, But I Have Custody of the Child',
        'description': 'FAQs About Child Support',
        'resourceType': 'Additional Readings',
        'externalUrl': null,
        'url': 'https://alaskalawhelp.org/resource/my-pfd-was-taken-for-child-support-but-i-have?ref=vhU5v',
        'topicTags': null,
        'location': [
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Kalawao',
            'zipCode': '96761'
          },
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Honolulu',
            'zipCode': '96741'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Juneau',
            'zipCode': '96815'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Anchorage',
            'zipCode': '99507'
          }
        ],
        'icon': './assets/images/resources/resource.png',
        'createdBy': '',
        'createdTimeStamp': null,
        'modifiedBy': '',
        'modifiedTimeStamp': '2018-04-01T04:18:00Z'
      },
      {
        'id': 'ce2c727a-1035-4614-b6e8-a161141f245e',
        'name': 'Who is the Father?',
        'description': 'FAQs About Child Support',
        'resourceType': 'Additional Readings',
        'externalUrl': null,
        'url': 'https://alaskalawhelp.org/resource/how-do-i-establish-paternity-of-a-child?ref=vhU5v',
        'topicTags': null,
        'location': [
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Kalawao',
            'zipCode': '96761'
          },
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Honolulu',
            'zipCode': '96741'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Juneau',
            'zipCode': '96815'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Anchorage',
            'zipCode': '99507'
          }
        ],
        'icon': './assets/images/resources/resource.png',
        'createdBy': '',
        'createdTimeStamp': null,
        'modifiedBy': '',
        'modifiedTimeStamp': '2018-04-01T04:18:00Z'
      },
      {
        'id': '9b8bd927-ad8f-416f-9d5f-82ee96925357',
        'name': 'Jurisdiction of my interview',
        'description': 'This A2J Guided Interview was created to demonstrate that A2J Author can be used to create a curated experience around divorce for Alaska and Hawaii',
        'resourceType': 'Guided Assistant',
        'externalUrl': '9a6a6131-657d-467d-b09b-c570b7dad242',
        'url': null,
        'topicTags': null,
        'location': [
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Kalawao',
            'zipCode': '96761'
          },
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Honolulu',
            'zipCode': '96741'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Juneau',
            'zipCode': '96815'
          },
          {
            'state': 'Alaska',
            'county': null,
            'city': 'Anchorage',
            'zipCode': '99507'
          },
          {
            'state': 'New York',
            'county': null,
            'city': 'EAST MEADOW',
            'zipCode': '11554'
          },
          {
            'state': 'NY',
            'county': null,
            'city': 'EAST MEADOW',
            'zipCode': '11554'
          },
          {
            'state': 'Washington',
            'county': null,
            'city': 'Redmond',
            'zipCode': '98052'
          }
        ],
        'icon': null,
        'createdBy': null,
        'createdTimeStamp': null,
        'modifiedBy': null,
        'modifiedTimeStamp': null
      },
      {
        'id': 'bbb88efb-3816-218b-5b38-1281e1b77f5a',
        'name': 'Hawaii Organization Test',
        'description': 'HawaiiLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
        'resourceType': 'Organizations',
        'externalUrl': null,
        'url': 'https://alaskalawhelp.org/',
        'topicTags': null,
        'location': [
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Kalawao',
            'zipCode': '96761'
          },
          {
            'state': 'Hawaii',
            'county': null,
            'city': 'Honolulu',
            'zipCode': '96741'
          }
        ],
        'icon': './assets/images/resources/resource.png',
        'createdBy': '',
        'createdTimeStamp': null,
        'modifiedBy': '',
        'modifiedTimeStamp': '2018-04-01T04:18:00Z'
      }
    ]
  }
]");

        public static JArray resourceIds = JArray.Parse(@"[ '19a02209-ca38-4b74-bd67-6ea941d41518', '9ca4cf73-f6c0-4f63-a1e8-2a3774961df5', '49779468-1fe0-4183-850b-ff365e05893e' ]");

        public static JArray resourceDetailsForResourceIds = JArray.Parse(@"[
  {
    'id': '19a02209-ca38-4b74-bd67-6ea941d41518',
    'name': 'Alaska Law Help',
    'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
    'resourceType': 'Organizations',
    'externalUrl': null,
    'url': 'https://alaskalawhelp.org/',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': 'Matanuska-Susitna Borough',
        'city': 'Wasilla',
        'zipCode': null
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': '',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  },
  {
    'id': '9ca4cf73-f6c0-4f63-a1e8-2a3774961df5',
    'name': 'Alaska Legal Services - Bristol Bay Office',
    'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
    'resourceType': 'Organizations',
    'externalUrl': null,
    'url': 'https://alaskalawhelp.org/',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Anchorage',
        'zipCode': '99507'
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': '',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  },
  {
    'id': '49779468-1fe0-4183-850b-ff365e05893e',
    'name': 'Alaska Native Justice Center',
    'description': 'AlaskaLawHelp is a guide to civil legal services and resources for low-income persons and seniors in Alaska.  This site provides information on various free and low-cost legal programs in Alaska, including basic eligibility and contact information.  Here you can find links to web sites offering helpful information, schedules of free self-help clinics, forms, and legal education documents that give you basic information on a number of legal problems.',
    'resourceType': 'Organizations',
    'externalUrl': null,
    'url': 'https://alaskalawhelp.org/',
    'topicTags': null,
    'location': [
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Kalawao',
        'zipCode': '96761'
      },
      {
        'state': 'Hawaii',
        'county': null,
        'city': 'Honolulu',
        'zipCode': '96741'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Juneau',
        'zipCode': '96815'
      },
      {
        'state': 'Alaska',
        'county': null,
        'city': 'Anchorage',
        'zipCode': '99507'
      }
    ],
    'icon': './assets/images/resources/resource.png',
    'createdBy': '',
    'createdTimeStamp': null,
    'modifiedBy': '',
    'modifiedTimeStamp': '2018-04-01T04:18:00Z'
  }

]");

        public static JArray planStepsByTopicId = JArray.Parse(@"[
  {
    'stepId': '296ee8be-5b9e-43b4-8d86-52a897afe580',
    'title': 'File a motion to modify if there has been a change of circumstances.',
    'description': 'The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.',
    'order': 1,
    'isComplete': false,
    'resources': [
      '19a02209-ca38-4b74-bd67-6ea941d41518',
      '9ca4cf73-f6c0-4f63-a1e8-2a3774961df5',
      '49779468-1fe0-4183-850b-ff365e05893e'
    ],
    'topicIds':['e1fdbbc6-d66a-4275-9cd2-2be84d303e12']
  },
  {
    'stepId': '9dffb34a-f62e-40cb-a29e-cf28cee83955',
    'title': 'Jurisdiction',
    'description': 'Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:',
    'order': 1,
    'isComplete': false,
    'resources': [
      '4f327a5b-9a79-466e-98be-2542eb28bb28',
      'be0cb3e1-7054-403a-baac-d119ea5be007',
      '2cb89c44-f989-42ea-b038-419e5006e8ff',
      '2fe9f117-bfb5-469f-b80c-877640a29f75'
    ],
    'topicIds':['e1fdbbc6-d66a-4275-9cd2-2be84d303e12']
  },
  {
    'stepId': 'afe73385-7e6c-4eac-b50f-2374d457a239',
    'title': 'Pre-Decree Relief',
    'description': 'Read instructions on how to fill out a request for pre-decree relief. Fill out the form and serve the other side with a copy. Either a family court clerk or a family court judge will sign the Order for Pre-Decree Relief depending on the types of relief being requested.',
    'order': 1,
    'isComplete': false,
    'resources': [
      'b0dc6222-a0a8-4927-a398-ccf0d6524593',
      '78a4da0f-3087-4513-8b45-a486919d2ed5',
      'ce2c727a-1035-4614-b6e8-a161141f245e',
      '9b8bd927-ad8f-416f-9d5f-82ee96925357',
      'bbb88efb-3816-218b-5b38-1281e1b77f5a'
    ],
    'topicIds':['e1fdbbc6-d66a-4275-9cd2-2be84d303e12']
  }
]");


        public static string TopicDetailsJArray
        {
            get
            {
                return "[\r\n    {\r\n      'id': 'e1fdbbc6-d66a-4275-9cd2-2be84d303e12',\r\n      'name': 'Family',\r\n      'overview': 'Overview of the Family topic',\r\n      'quickLinks': [\r\n        {\r\n          'text': 'Family Law Self-Help Center',\r\n          'url': 'http:////courts.alaska.gov//shc//family//selfhelp.htm'\r\n        },\r\n        {\r\n          'text': 'Introduction to Family Law Video Series',\r\n          'url': 'https:////alaskalawhelp.org/resource//introduction-to-family-law-video-series?ref=OEGQX'\r\n        }\r\n      ],\r\n      'parentTopicId': [],\r\n      'resourceType': 'Topics',\r\n      'keywords': 'Family',\r\n      'location': [\r\n        {\r\n          'state': 'Hawaii',\r\n          'city': 'Kalawao',\r\n          'zipCode': '96761'\r\n        },\r\n        {\r\n          'state': 'Hawaii',\r\n          'city': 'Honolulu',\r\n          'zipCode': '96741'\r\n        },\r\n        {\r\n          'state': 'Alaska',\r\n          'city': 'Juneau',\r\n          'zipCode': '96815'\r\n        },\r\n        {\r\n          'state': 'Alaska',\r\n          'city': 'Anchorage',\r\n          'zipCode': '99507'\r\n        }\r\n      ]";
            }
        }
    }
}


