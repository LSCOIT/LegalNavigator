using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

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
        public static JArray personalizedPlan = JArray.Parse(@"[{
    'id': 'addf41e9-1a27-4aeb-bcbb-7959f95094ba',
    'name': 'Family',
    'parentTopicID': '',
    'resourceType': 'Topics',
    'keywords': 'EVICTION',
    'location': [
      {
        'state': 'Hawaii',
        'county': 'Kalawao County',
        'city': 'Kalawao',
        'zipCode': '96742'
      },
      {
        'state': 'HI',
        'county': '',
        'city': '',
        'zipCode': ''
      },
      { 'zipCode': '96741' },
      {
        'state': 'Hawaii',
        'county': 'Honolulu County',
        'city': 'Honolulu'
      },
      {
        'state': 'Hawaii',
        'city': 'Hawaiian Beaches'
      },
      {
        'state': 'Hawaii',
        'city': 'Haiku-Pauwela'
      },
      { 'state': 'Alaska' }
    ],
    'jsonContent': '',
    'icon': './assets/images/topics/topic14.png',
    'quickLinks': [
      {
        'url': 'http://localhost/Family',
        'text': 'Quick Link for Family'
      },
      {
        'url': 'http://localhost/Family1',
        'text': 'Quick Link for Family'
      }
    ],
    'createdBy': '',
    'createdTimeStamp': '',
    'modifiedBy': '',
    'modifiedTimeStamp': '',
    '_rid': 'mwoSALHtpAEBAAAAAAAAAA==',
    '_self': 'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/',
    '_etag': '\'d100e424-0000-0000-0000-5b628b530000\'',
    '_attachments': 'attachments/',
    '_ts': 1533184851
  },
  {
    'id': '932abb0a-c6bb-46da-a3d8-5f52c2c914a0',
    'name': 'Child Custody',
    'parentTopicID': 'f102bfae-362d-4659-aaef-956c391f79de',
    'resourceType': 'Topics',
    'keywords': 'CHILD ABUSE|CHILD CUSTODY',
    'location': [
      {
        'state': 'Hawaii',
        'city': 'Kalawao',
        'zipCode': '96742'
      },
      { 'zipCode': '96741' },
      {
        'state': 'Hawaii',
        'city': 'Honolulu'
      },
      {
        'state': 'Hawaii',
        'city': 'Hawaiian Beaches'
      },
      {
        'state': 'Hawaii',
        'city': 'Haiku-Pauwela'
      },
      { 'state': 'Alaska' }
    ],
    'jsonContent': '',
    'icon': './assets/images/topics/topic14.png',
    'createdBy': '',
    'createdTimeStamp': '',
    'modifiedBy': '',
    'modifiedTimeStamp': '',
    '_rid': 'mwoSALHtpAELAAAAAAAAAA==',
    '_self': 'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAELAAAAAAAAAA==/',
    '_etag': '\'50000b15-0000-0000-0000-5b5020340000\'',
    '_attachments': 'attachments/',
    '_ts': 1531977780
  }
]");
        public static JArray topicDetails = JArray.Parse(@"[
  {
    {
      'id': 'addf41e9-1a27-4aeb-bcbb-7959f95094ba',
      'name': 'Family',
      'parentTopicID': '',
      'resourceType': 'Topics',
      'keywords': 'EVICTION',
      'location': [
        {
          'state': 'Hawaii',
          'county': 'Kalawao County',
          'city': 'Kalawao',
          'zipCode': '96742'
        },
        {
          'state': 'HI',
          'county': '',
          'city': '',
          'zipCode': ''
        },
        { 'zipCode': '96741' },
        {
          'state': 'Hawaii',
          'county': 'Honolulu County',
          'city': 'Honolulu'
        },
        {
          'state': 'Hawaii',
          'city': 'Hawaiian Beaches'
        },
        {
          'state': 'Hawaii',
          'city': 'Haiku-Pauwela'
        },
        { 'state': 'Alaska' }
      ],
      'jsonContent': '',
      'icon': './assets/images/topics/topic14.png',
      'quickLinks': [
        {
          'url': 'http://localhost/Family',
          'text': 'Quick Link for Family'
        },
        {
          'url': 'http://localhost/Family1',
          'text': 'Quick Link for Family'
        }
      ],
      'createdBy': '',
      'createdTimeStamp': '',
      'modifiedBy': '',
      'modifiedTimeStamp': ''
    }
  },
  {
    {
      'id': '932abb0a-c6bb-46da-a3d8-5f52c2c914a0',
      'name': 'Child Custody',
      'parentTopicID': 'f102bfae-362d-4659-aaef-956c391f79de',
      'resourceType': 'Topics',
      'keywords': 'CHILD ABUSE|CHILD CUSTODY',
      'location': [
        {
          'state': 'Hawaii',
          'city': 'Kalawao',
          'zipCode': '96742'
        },
        { 'zipCode': '96741' },
        {
          'state': 'Hawaii',
          'city': 'Honolulu'
        },
        {
          'state': 'Hawaii',
          'city': 'Hawaiian Beaches'
        },
        {
          'state': 'Hawaii',
          'city': 'Haiku-Pauwela'
        },
        { 'state': 'Alaska' }
      ],
      'jsonContent': '',
      'icon': './assets/images/topics/topic14.png',
      'createdBy': '',
      'createdTimeStamp': '',
      'modifiedBy': '',
      'modifiedTimeStamp': ''
    }
  }
]");


    }
}
