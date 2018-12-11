using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models;
using System;
using System.Collections.Generic;

namespace Access2Justice.Api.Tests.TestData
{
    public static class CuratedExperienceTestData
    {
        public static string CuratedExperienceSampleSchema
        {
            get
            {
                return "  {\r\n    \"id\": \"f3daa1e4-5f20-47ce-ab6d-f59829f936fe\",\r\n    \"components\": " +
                    "[\r\n        {\r\n            \"componentId\": \"0173c6ca-2ba7-4d1d-94f5-7e3fec1f9dfc\"" +
                    ",\r\n            \"name\": \"1-Introduction\",\r\n            \"text\": \"This is the " +
                    "introduction. This A2J Guided Interview was created as part of a sample exercise. It i" +
                    "s not intended for use by the general public.\u00A0\",\r\n            \"learn\": \"\"," +
                    "\r\n            \"help\": \"\",\r\n            \"tags\": [],\r\n            \"buttons\":" +
                    " [\r\n                {\r\n                    \"id\": \"f3a1e369-edba-4152-bbc7-a52e8e7" +
                    "7b56e\",\r\n                    \"label\": \"Continue\",\r\n                    \"desti" +
                    "nation\": \"1-Name\"\r\n                }\r\n            ],\r\n            \"fields\":" +
                    " []\r\n        },\r\n        {\r\n            \"componentId\": \"4adec03b-4f9b-4bc9-bc44" +
                    "-27a8e84e30ae\",\r\n            \"name\": \"1-Name\",\r\n            \"text\": \"Enter y" +
                    "our name.\",\r\n            \"learn\": \"\",\r\n            \"help\": \"\",\r\n         " +
                    "   \"tags\": [],\r\n            \"buttons\": [\r\n                {\r\n                 " +
                    "   \"id\": \"2b92e07b-a555-48e8-ad7b-90b99ebc5c96\",\r\n                    \"label\": " +
                    "\"Continue\",\r\n                    \"destination\": \"2-Gender\"\r\n                }\r\n" +
                    "            ],\r\n            \"fields\": [\r\n                {\r\n                    " +
                    "\"id\": \"22cbf2ac-a8d3-48c5-b230-297111e0e85c\",\r\n                    \"type\": \"text\"" +
                    ",\r\n                    \"label\": \"First:\",\r\n                    \"isRequired\": true" +
                    ",\r\n                    \"min\": \"\",\r\n                    \"max\": \"\",\r\n         " +
                    "           \"invalidPrompt\": \"You must type a response in the highlighted space before " +
                    "you can continue.\"\r\n                },\r\n                {\r\n                    \"id\"" +
                    ": \"6c8312eb-131d-4cfb-a542-0e3f6d07a1d3\",\r\n                    \"type\": \"text\",\r\n  " +
                    "                  \"label\": \"Middle:\",\r\n                    \"isRequired\": false,\r\n " +
                    "                   \"min\": \"\",\r\n                    \"max\": \"\",\r\n                " +
                    "    \"invalidPrompt\": \"You must type a response in the highlighted space before you can" +
                    " continue.\"\r\n                },\r\n                {\r\n                    \"id\": \"d2" +
                    "a935b4-bb07-494f-9c59-f3115b19d002\",\r\n                    \"type\": \"text\",\r\n       " +
                    "             \"label\": \"Last:\",\r\n                    \"isRequired\": true,\r\n        " +
                    "            \"min\": \"\",\r\n                    \"max\": \"\",\r\n                    \"in" +
                    "validPrompt\": \"You must type a response in the highlighted space before you can continue." +
                    "\"\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"compone" +
                    "ntId\": \"29d06a1d-0898-439e-b208-2774d523ea0a\",\r\n            \"name\": \"2-Gender\"" +
                    ",\r\n            \"text\": \"Choose your gender.\",\r\n            \"learn\": \"Why are yo" +
                    "u asking this question?\",\r\n            \"help\": \"This question is used to populate t" +
                    "he avatar that will represent you through the rest of this A2J Guided Interview.\u00A0\"" +
                    ",\r\n            \"tags\": [],\r\n            \"buttons\": [\r\n                {\r\n     " +
                    "               \"id\": \"1d34317b-4b2e-4de4-92c0-61407d0b70da\",\r\n                    " +
                    "\"label\": \"Continue\",\r\n                    \"destination\": \"3-Address\"\r\n      " +
                    "          }\r\n            ],\r\n            \"fields\": [\r\n                {\r\n      " +
                    "              \"id\": \"1a843edf-1c20-4f0f-96c5-fbcae98b381a\",\r\n                   " +
                    " \"type\": \"gender\",\r\n                    \"label\": \"Gender:\",\r\n               " +
                    "     \"isRequired\": true,\r\n                    \"min\": \"\",\r\n                    " +
                    "\"max\": \"\",\r\n                    \"invalidPrompt\": \"\"\r\n                }\r\n  " +
                    "          ]\r\n        },\r\n        {\r\n            \"componentId\": \"1ba7a445-7a2b-4d" +
                    "7b-b043-296bffef402a\",\r\n            \"name\": \"3-Address\",\r\n            \"text\": " +
                    "\"%%[Client first name TE]%% what is your address?&nbsp;\",\r\n            \"learn\": \"\"" +
                    ",\r\n            \"help\": \"\",\r\n            \"tags\": [],\r\n            \"buttons\": " +
                    "[\r\n                {\r\n                    \"id\": \"cafc4de0-91c5-4f67-a59f-bbffc25342" +
                    "36\",\r\n                    \"label\": \"Continue\",\r\n                    \"destinatio" +
                    "n\": \"4-Phone number\"\r\n                }\r\n            ],\r\n            \"fields\": " +
                    "[\r\n                {\r\n                    \"id\": \"78da510a-180c-4757-b75d-fd87f470ae" +
                    "76\",\r\n                    \"type\": \"text\",\r\n                    \"label\": \"Stree" +
                    "t\",\r\n                    \"isRequired\": false,\r\n                    \"min\":" +
                    " \"\",\r\n                    \"max\": \"\",\r\n                    \"invalidPrompt\":" +
                    " \"\"\r\n                },\r\n                {\r\n                    \"id\": \"6a81a" +
                    "78e-7692-4256-88d6-651dc4e9ce0f\",\r\n                    \"type\": \"text\",\r\n      " +
                    "              \"label\": \"City\",\r\n                    \"isRequired\": false,\r\n     " +
                    "               \"min\": \"\",\r\n                    \"max\": \"\",\r\n                  " +
                    "  \"invalidPrompt\": \"\"\r\n                },\r\n                {\r\n                 " +
                    "   \"id\": \"3a6180c3-d42c-400d-a2a7-d9b556a454af\",\r\n                    \"type\": \"t" +
                    "extpick\",\r\n                    \"label\": \"State\",\r\n                    \"isRequire" +
                    "d\": false,\r\n                    \"min\": \"\",\r\n                    \"max\": \"\",\r\n" +
                    "                    \"invalidPrompt\": \"\"\r\n                },\r\n                {\r\n " +
                    "                   \"id\": \"9471103f-7868-47fd-b1ab-581cc3934493\",\r\n                  " +
                    "  \"type\": \"numberzip\",\r\n                    \"label\": \"Zip code\",\r\n            " +
                    "        \"isRequired\": false,\r\n                    \"min\": \"\",\r\n                  " +
                    "  \"max\": \"\",\r\n                    \"invalidPrompt\": \"\"\r\n                }\r\n " +
                    "           ]\r\n        },\r\n        {\r\n            \"componentId\": \"3f2e2cdd-006e-4" +
                    "f4c-b0bf-32cea08f8e90\",\r\n            \"name\": \"4-Phone number\",\r\n            \"tex" +
                    "t\": \"What is your phone number?\",\r\n            \"learn\": \"\",\r\n            \"hel" +
                    "p\": \"\",\r\n            \"tags\": [],\r\n            \"buttons\": [\r\n               " +
                    " {\r\n                    \"id\": \"aa3777d0-e7dd-48dd-afb3-6d61fc172768\",\r\n           " +
                    "         \"label\": \"Continue\",\r\n                    \"destination\": \"1-Marital Statu" +
                    "s\"\r\n                }\r\n            ],\r\n            \"fields\": [\r\n              " +
                    "  {\r\n                    \"id\": \"d63e24a3-4a4d-4fec-a1aa-9c72b2fcab5e\",\r\n         " +
                    "           \"type\": \"numberphone\",\r\n                    \"label\": \"Phone numbe" +
                    "r\",\r\n                    \"isRequired\": false,\r\n                    \"min\": \"\",\r\n " +
                    "                   \"max\": \"\",\r\n                    \"invalidPrompt\": \"\"\r\n    " +
                    "            }\r\n            ]\r\n        },\r\n        {\r\n            \"componentI" +
                    "d\": \"73464e7e-5c8a-4d96-9a0e-6f1f28592eb9\",\r\n            \"name\": \"1-Marital Sta" +
                    "tus\",\r\n            \"text\": \"What is your marital status?\",\r\n            \"lear" +
                    "n\": \"\",\r\n            \"help\": \"\",\r\n            \"tags\": [],\r\n            \"b" +
                    "uttons\": [\r\n                {\r\n                    \"id\": \"5846cabb-1826-47bc-b2c" +
                    "d-6168f78fc4f2\",\r\n                    \"label\": \"Continue\",\r\n                    \"" +
                    "destination\": \"2-Have children?\"\r\n                }\r\n            ],\r\n           " +
                    " \"fields\": [\r\n                {\r\n                    \"id\": \"0bc5ccb7-e1d2-40b6-b" +
                    "20c-4fbd6cb85de9\",\r\n                    \"type\": \"radio\",\r\n                    \"la" +
                    "bel\": \"Married\",\r\n                    \"isRequired\": false,\r\n                    \"m" +
                    "in\": \"\",\r\n                    \"max\": \"\",\r\n                    \"invalidProm" +
                    "pt\": \"\"\r\n                },\r\n                {\r\n                    \"id\": \"93" +
                    "d7a9f7-a33e-4587-a04e-e57638b77e73\",\r\n                    \"type\": \"radio\",\r\n  " +
                    "                  \"label\": \"Single\",\r\n                    \"isRequired\": fals" +
                    "e,\r\n                    \"min\": \"\",\r\n                    \"max\": \"\",\r\n    " +
                    "                \"invalidPrompt\": \"\"\r\n                },\r\n                {\r\n  " +
                    "                  \"id\": \"1d292f71-312e-4308-b22b-cad39cc7e9be\",\r\n                 " +
                    "   \"type\": \"radio\",\r\n                    \"label\": \"Divorced\",\r\n             " +
                    "       \"isRequired\": false,\r\n                    \"min\": \"\",\r\n                   " +
                    " \"max\": \"\",\r\n                    \"invalidPrompt\": \"\"\r\n                },\r\n  " +
                    "              {\r\n                    \"id\": \"289d456e-698b-431d-b0ec-4e82d9d110" +
                    "a4\",\r\n                    \"type\": \"radio\",\r\n                    \"label\": \"Wi" +
                    "dowed\",\r\n                    \"isRequired\": false,\r\n                    \"mi" +
                    "n\": \"\",\r\n                    \"max\": \"\",\r\n                    \"invalidPr" +
                    "ompt\": \"\"\r\n                }\r\n            ]\r\n        },\r\n        {\r\n   " +
                    "         \"componentId\": \"d81340c4-b93a-46c1-8176-fcf8b49a9f21\",\r\n            \"na" +
                    "me\": \"2-Have children?\",\r\n            \"text\": \"Do you have children?\",\r\n    " +
                    "        \"learn\": \"\",\r\n            \"help\": \"\",\r\n            \"tags\": [],\r\n  " +
                    "          \"buttons\": [\r\n                {\r\n                    \"id\": \"16402097-d" +
                    "668-486d-99ed-f1b26c9510a7\",\r\n                    \"label\": \"Yes\",\r\n            " +
                    "        \"destination\": \"3-How many children?\"\r\n                },\r\n              " +
                    "  {\r\n                    \"id\": \"f7534f78-5b33-4012-a4a6-16cf49c89ee6\",\r\n          " +
                    "          \"label\": \"No\",\r\n                    \"destination\": \"1-Purpose of for" +
                    "m\"\r\n                }\r\n            ],\r\n            \"fields\": []\r\n        },\r\n " +
                    "       {\r\n            \"componentId\": \"1d2e7c41-f7d1-46da-bd3e-7bfc10a1678b\",\r\n    " +
                    "        \"name\": \"3-How many children?\",\r\n            \"text\": \"How many children " +
                    "do you have?\u00A0\",\r\n            \"learn\": \"\",\r\n            \"help\": \"\",\r\n  " +
                    "          \"tags\": [],\r\n            \"buttons\": [\r\n                {\r\n           " +
                    "         \"id\": \"f631fad5-bd39-4982-8881-e81d25cbe664\",\r\n                    \"lab" +
                    "el\": \"Continue\",\r\n                    \"destination\": \"4-Child's name and birthdat" +
                    "e\"\r\n                }\r\n            ],\r\n            \"fields\": [\r\n               " +
                    " {\r\n                    \"id\": \"c0e22b4a-4937-4f53-b880-b202700fb2e6\",\r\n          " +
                    "          \"type\": \"numberpick\",\r\n                    \"label\": \"Number of childre" +
                    "n\",\r\n                    \"isRequired\": false,\r\n                    \"min\": \"1\"," +
                    "\r\n                    \"max\": \"4\",\r\n                    \"invalidPrompt\": \"\"\r\n" +
                    "                }\r\n            ]\r\n        },\r\n        {\r\n            \"componentI" +
                    "d\": \"2247da03-da30-4cbb-b24f-2132f4f04485\",\r\n            \"name\": \"1-The End\",\r\n" +
                    "            \"text\": \"Congratulations! You have finished your A2J Guided Interview. Click" +
                    " Get My Document to print your completed form.\u00A0\",\r\n            \"learn\": \"\",\r\n " +
                    "           \"help\": \"\",\r\n            \"tags\": [],\r\n            \"buttons\": [\r\n  " +
                    "              {\r\n                    \"id\": \"0956d97e-3fcd-4383-bb25-45c2a138ceb" +
                    "4\",\r\n                    \"label\": \"Get My Document\",\r\n                    \"d" +
                    "estination\": \"SUCCESS\"\r\n                }\r\n            ],\r\n            \"field" +
                    "s\": []\r\n        },\r\n        {\r\n            \"componentId\": \"ff6fb136-1d62-4330-" +
                    "a82e-242e592d9d59\",\r\n            \"name\": \"1-Purpose of form\",\r\n            \"t" +
                    "ext\": \"Why are you filling out this form?\",\r\n            \"learn\": \"\",\r\n   " +
                    "         \"help\": \"\",\r\n            \"tags\": [],\r\n            \"buttons\": [\r\n " +
                    "               {\r\n                    \"id\": \"98613482-53ad-458f-988c-5b473358c7b" +
                    "f\",\r\n                    \"label\": \"Continue\",\r\n                    \"destinatio" +
                    "n\": \"1-The End\"\r\n                }\r\n            ],\r\n            \"fiel" +
                    "ds\": [\r\n                {\r\n                    \"id\": \"cfd6fd1d-2974-43ef-8e3c-6f" +
                    "b0e9a0d8cf\",\r\n                    \"type\": \"textlong\",\r\n                    \"la" +
                    "bel\": \"\",\r\n                    \"isRequired\": false,\r\n                    \"m" +
                    "in\": \"\",\r\n                    \"max\": \"\",\r\n                    \"invalidProm" +
                    "pt\": \"\"\r\n                }\r\n            ]\r\n        },\r\n        {\r\n    " +
                    "        \"componentId\": \"32e9dd47-5b06-4db2-a297-054436add52f\",\r\n            \"na" +
                    "me\": \"4-Child's name and birthdate\",\r\n            \"text\": \"What is your %%ORDI" +
                    "NAL(ChildCount)%% child's name and date of birth?&nbsp;\",\r\n            \"lea" +
                    "rn\": \"\",\r\n            \"help\": \"\",\r\n            \"tags\": [],\r\n            \"b" +
                    "uttons\": [\r\n                {\r\n                    \"id\": \"7d6b2ee0-6b30-4011-" +
                    "b99e-0d64c16217c8\",\r\n                    \"label\": \"Continue\",\r\n               " +
                    "     \"destination\": \"\"\r\n                }\r\n            ],\r\n            \"fi" +
                    "elds\": [\r\n                {\r\n                    \"id\": \"bf13a106-37b3-4e8f-bd" +
                    "9f-4c31a8a8d2c2\",\r\n                    \"type\": \"text\",\r\n                    \"la" +
                    "bel\": \"Child's first name:\",\r\n                    \"isRequired\": false,\r\n    " +
                    "                \"min\": \"\",\r\n                    \"max\": \"\",\r\n              " +
                    "      \"invalidPrompt\": \"\"\r\n                },\r\n                {\r\n          " +
                    "          \"id\": \"50dbe540-24c8-4f5e-9765-770d675f6240\",\r\n                    \"typ" +
                    "e\": \"datemdy\",\r\n                    \"label\": \"Date of birth:\",\r\n            " +
                    "        \"isRequired\": false,\r\n                    \"min\": \"\",\r\n              " +
                    "      \"max\": \"\",\r\n                    \"invalidPrompt\": \"\"\r\n             " +
                    "   }\r\n            ]\r\n        }\r\n    ]\r\n}";
            }
        }

        public static string CuratedExperienceAnswersSchema
        {
            get
            {
                return
                  " {\r\n  'id': '288af4da-06bb-4655-aa91-41314e248d6b',\r\n  'curatedExperienceId': '9a6a6131-657d-467d-b09b-c570b7dad242',\r\n  'answers': [\r\n    {\r\n      'answerButtonId': 'f3a1e369-edba-4152-bbc7-a52e8e77b56e',\r\n      'answerFields': []\r\n    },\r\n    {\r\n      'answerButtonId': '2b92e07b-a555-48e8-ad7b-90b99ebc5c96',\r\n      'answerFields': [\r\n        {\r\n          'fieldId': '22cbf2ac-a8d3-48c5-b230-297111e0e85c',\r\n          'value': 'gh'\r\n        },\r\n        {\r\n          'fieldId': '6c8312eb-131d-4cfb-a542-0e3f6d07a1d3',\r\n          'value': 'fgh'\r\n        },\r\n        {\r\n          'fieldId': 'd2a935b4-bb07-494f-9c59-f3115b19d002',\r\n          'value': 'gh'\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      'answerButtonId': 'cafc4de0-91c5-4f67-a59f-bbffc2534236',\r\n      'answerFields': [\r\n        {\r\n          'fieldId': '78da510a-180c-4757-b75d-fd87f470ae76',\r\n          'value': 'fh'\r\n        },\r\n        {\r\n          'fieldId': '6a81a78e-7692-4256-88d6-651dc4e9ce0f',\r\n          'value': 'fh'\r\n        },\r\n        {\r\n          'fieldId': '9471103f-7868-47fd-b1ab-581cc3934493',\r\n          'value': '54'\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      'answerButtonId': 'aa3777d0-e7dd-48dd-afb3-6d61fc172768',\r\n      'answerFields': [\r\n        {\r\n          'fieldId': 'd63e24a3-4a4d-4fec-a1aa-9c72b2fcab5e',\r\n          'value': '46'\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      'answerButtonId': '5846cabb-1826-47bc-b2cd-6168f78fc4f2',\r\n      'answerFields': [\r\n        {\r\n          'fieldId': '0bc5ccb7-e1d2-40b6-b20c-4fbd6cb85de9',\r\n          'value': null\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      'answerButtonId': '98613482-53ad-458f-988c-5b473358c7bf',\r\n      'answerFields': [\r\n        {\r\n          'fieldId': 'cfd6fd1d-2974-43ef-8e3c-6fb0e9a0d8cf',\r\n          'value': '546h'\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";
            }
        }

        public static IEnumerable<object[]> CuratedExperienceData()
        {
            yield return new object[] { CuratedExperience,CuratedExperience, Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242")
            };
        }
        public static CuratedExperience CuratedExperience =>
          new CuratedExperience
          {
              Title = "Test",
              CuratedExperienceId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
              A2jPersonalizedPlanId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
              Components = { new CuratedExperienceComponent { ComponentId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"), Buttons = { new Button { Id = Guid.Parse("2b92e07b-a555-48e8-ad7b-90b99ebc5c96"), Label = "Continue", Destination = "2-Gender", Name = "3-Address", Value = "btnValue" } }, Code = { }, Fields = { }, Help = "", Learn = "", Name = "", Tags = { }, Text = "" } }
          };
        public static CuratedExperience CuratedExperience2 =>
          new CuratedExperience
          {
              Title = "Test",
              CuratedExperienceId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
              A2jPersonalizedPlanId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
              Components = { new CuratedExperienceComponent { ComponentId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"), Buttons = { new Button { Id = Guid.Parse("2b92e07b-a555-48e8-ad7b-90b99ebc5c96"), Label = "Continue", Destination = "2-Gender", Name = "2-Gender", Value = "btnValue" } }, Code = { }, Fields = { }, Help = "", Learn = "", Name = "2-Gender", Tags = { }, Text = "" } }
          };
        public static CuratedExperience CuratedExperience3 =>
         new CuratedExperience
         {
             Title = "Test",
             CuratedExperienceId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
             A2jPersonalizedPlanId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
             Components = { new CuratedExperienceComponent { ComponentId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"), Buttons = { new Button { Id = Guid.Parse("2b92e07b-a555-48e8-ad7b-90b99ebc5c96"), Label = "Continue", Destination = "2-Gender", Name = "2-Gender", Value = "btnValue" } }, Code = { CodeAfter= "GOTO,PAGE1", CodeBefore= "TESTcODEbEFORE,page2" }, Fields = { }, Help = "", Learn = "", Name = "2-Gender", Tags = { }, Text = "" } }
         };
        public static CuratedExperience CuratedExperience4 =>
         new CuratedExperience
         {
             Title = "Test",
             CuratedExperienceId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
             A2jPersonalizedPlanId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
             Components = { new CuratedExperienceComponent { ComponentId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"), Buttons = { new Button { Id = Guid.Parse("2b92e07b-a555-48e8-ad7b-90b99ebc5c96"), Label = "Continue", Destination = "2-Gender", Name = "2-Gender", Value = "btnValue" } }, Code = { CodeAfter = "GOTO,PAGE1", CodeBefore = "TESTcODEbEFORE,page2" }, Fields = { }, Help = "", Learn = "", Name = "2-Gender", Tags = { }, Text = "" } }
         };
        public static CuratedExperience CuratedExperienceWithNoComponent =>
         new CuratedExperience
         {
             Title = "Test",
             CuratedExperienceId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
             A2jPersonalizedPlanId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
             Components = { new CuratedExperienceComponent { ComponentId = Guid.Parse("00000000-0000-0000-0000-000000000000"), Buttons = { new Button { Id = Guid.Parse("2b92e07b-a555-48e8-ad7b-90b99ebc5c96"), Label = "Continue", Destination = "2-Gender", Name = "3-Address", Value = "btnValue" } }, Code = { }, Fields = { }, Help = "", Learn = "", Name = "", Tags = { }, Text = "" } }
         };
        public static IEnumerable<object[]> CuratedExperienceComponentViewModelData()
        {
            yield return new object[] { CuratedExperience, CuratedExperienceComponentViewModel, Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"), CuratedExperienceAnswersSchema };
            yield return new object[] { CuratedExperienceWithNoComponent, null, Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"), CuratedExperienceAnswersSchema };
        }
        public static IEnumerable<object[]> CuratedExperienceComponentViewModelDataWithDefaultComponentId()
        {
            yield return new object[] { CuratedExperience, CuratedExperienceComponentViewModel, Guid.Parse("00000000-0000-0000-0000-000000000000"), CuratedExperienceAnswersSchema };
        }
        public static CuratedExperienceComponentViewModel CuratedExperienceComponentViewModel =>
            new CuratedExperienceComponentViewModel
            {
                ComponentId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
                Buttons = { new Button { Id = Guid.Parse("2b92e07b-a555-48e8-ad7b-90b99ebc5c96"), Label = "Continue", Destination = "2-Gender", Name = "3-Address", Value = "btnValue" } },
                Code = { },
                Fields = { },
                Help = "",
                Learn = "",
                Name = "",
                Tags = { },
                Text = "",
                AnswersDocId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                CuratedExperienceId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
                QuestionsRemaining = 0
            };
        public static CuratedExperienceComponentViewModel CuratedExperienceComponentViewModel2 =>
            new CuratedExperienceComponentViewModel
            {
                ComponentId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
                Buttons = { new Button { Id = Guid.Parse("2b92e07b-a555-48e8-ad7b-90b99ebc5c96"), Label = "Continue", Destination = "2-Gender", Name = "2-Gender", Value = "btnValue" } },
                Code = { },
                Fields = { },
                Help = "",
                Learn = "",
                Name = "2-Gender",
                Tags = { },
                Text = "",
                AnswersDocId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                CuratedExperienceId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
                QuestionsRemaining = 1
            };
        public static IEnumerable<object[]> NextComponentViewModelData()
        {
            yield return new object[] { CuratedExperience4, CuratedExperienceComponentViewModel2, Guid.Parse("2b281b6f-9668-4771-b16b-c2f10a317aac"), CuratedExperienceAnswersViewModel2, CuratedExperienceAnswers4 };
            //yield return new object[] { CuratedExperience3, CuratedExperienceComponentViewModel2, Guid.Parse("2b281b6f-9668-4771-b16b-c2f10a317aac"), CuratedExperienceAnswersViewModel2, CuratedExperienceAnswers };
            yield return new object[] { CuratedExperience2, CuratedExperienceComponentViewModel2, Guid.Parse("2b281b6f-9668-4771-b16b-c2f10a317aac"), CuratedExperienceAnswersViewModel2, CuratedExperienceAnswers };
            yield return new object[] { CuratedExperience, null, Guid.Parse("2b281b6f-9668-4771-b16b-c2f10a317aac"), CuratedExperienceAnswersViewModel, CuratedExperienceAnswers };
        }
        public static CuratedExperienceAnswersViewModel CuratedExperienceAnswersViewModel =>
           new CuratedExperienceAnswersViewModel
           {
               AnswersDocId = Guid.Parse("2b281b6f-9668-4771-b16b-c2f10a317aac"),
               ButtonId = Guid.Parse("2b92e07b-a555-48e8-ad7b-90b99ebc5c96"),
               CuratedExperienceId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
               Fields = { }
           };
        public static CuratedExperienceAnswersViewModel CuratedExperienceAnswersViewModel2 =>
           new CuratedExperienceAnswersViewModel
           {
               AnswersDocId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
               ButtonId = Guid.Parse("2b92e07b-a555-48e8-ad7b-90b99ebc5c96"),
               CuratedExperienceId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
               Fields = { }
           };
        public static CuratedExperienceAnswers CuratedExperienceAnswers =>
               new CuratedExperienceAnswers
               {
                   AnswersDocId = Guid.Parse("288af4da-06bb-4655-aa91-41314e248d6b"),
                   ButtonComponents = new List<ButtonComponent>() { new ButtonComponent { ButtonId = Guid.Parse("288af4da-06bb-4655-aa91-41314e248d6b"), CodeAfter = "GOTO,PAGE1", CodeBefore = "TESTcODEbEFORE,page2", Name = "test", Value = "test" } },
                   CuratedExperienceId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
                   FieldComponents = new List<FieldComponent>() { new FieldComponent { CodeAfter = "GOTO,PAGE1", CodeBefore = "TESTcODEbEFORE,page2", Fields = new List<AnswerField>() { new AnswerField { FieldId = Guid.Parse("288af4da-06bb-4655-aa91-41314e248d6b"), Name = "test", Value = "test", Text = "test" } } } }
               };
        public static CuratedExperienceAnswers CuratedExperienceAnswers4 =>
              new CuratedExperienceAnswers
              {
                  AnswersDocId = Guid.Parse("288af4da-06bb-4655-aa91-41314e248d6b"),
                  ButtonComponents = new List<ButtonComponent>() { new ButtonComponent { ButtonId = Guid.Parse("288af4da-06bb-4655-aa91-41314e248d6b"), CodeAfter = "GOTO,PAGE1", CodeBefore = "TESTcODEbEFORE,page2", Name = "test", Value = "test" } },
                  CuratedExperienceId = Guid.Parse("9a6a6131-657d-467d-b09b-c570b7dad242"),
                  FieldComponents = new List<FieldComponent>() { new FieldComponent { CodeAfter = "GOTO,PAGE1", CodeBefore = "TESTcODEbEFORE,page2", Fields = new List<AnswerField>() { new AnswerField { FieldId = Guid.Parse("288af4da-06bb-4655-aa91-41314e248d6b"), Name = "test", Value = "test", Text = "test" } } } }
              };
    }
}