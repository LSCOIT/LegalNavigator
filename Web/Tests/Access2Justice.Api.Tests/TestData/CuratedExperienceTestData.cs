using System;

namespace Access2Justice.Api.Tests.TestData
{
    public static class CuratedExperienceTestData
    {
        public static string A2JAuthorSampleSchema
        {
            get
            {
                return "{\r\n    \"authorId\": 11,\r\n    \"tool\": \"A2J\",\r\n    \"toolversion\": \"2012-04-19\",\r\n    \"avatar\":" +
                    " \"avatar2\",\r\n    \"avatarSkinTone\": \"\",\r\n    \"avatarHairColor\": \"\",\r\n    \"guideGender\": \"Female\",\r\n  " +
                    "  \"completionTime\": \"\",\r\n    \"copyrights\": \"\",\r\n    \"createdate\": \"\",\r\n    \"credits\": \"\",\r\n  " +
                    "  \"description\": \"TESTING  This A2J Guided Interview was created for the purposes of creating screenshots for a " +
                    "A2J Author and HotDocs sample exercise file.\u00A0 \",\r\n    \"emailContact\": \"\",\r\n    \"jurisdiction\": \"J" +
                    "urisdiction of my interview\",\r\n    \"language\": \"en\",\r\n    \"modifydate\": \"\",\r\n    \"sendfeedback\": fa" +
                    "lse,\r\n    \"subjectarea\": \"Jurisdiction of my interview\",\r\n    \"title\": \"Sample A2J Guided Interview (for " +
                    "Sample Exercise)\",\r\n    \"viewer\": \"A2J\",\r\n    \"endImage\": \"\",\r\n    \"logoImage\": \"\",\r\n    \"aut" +
                    "hors\": [{\r\n        \"name\": \"Jessica Frank\",\r\n        \"title\": \"A2J Author Program Coordinator\",\r\n   " +
                    "     \"organization\": \"IIT Chicago-Kent College of Law\",\r\n        \"email\": \"jbolack@kentlaw.iit.edu\"\r\n  " +
                    "  },\r\n    {\r\n        \"name\": \"Tester Name\",\r\n        \"title\": \"Testing A2J Author\",\r\n        \"organiz" +
                    "ation\": \"Avanade\",\r\n        \"email\": \"tester@xserver.com\"\r\n    }],\r\n    \"firstPage\": \"1-Introduction\"" +
                    ",\r\n    \"exitPage\": \"\",\r\n    \"steps\": [{\r\n        \"number\": \"0\",\r\n        \"text\": \"INTRODUCTION\"\r\n  " +
                    "  }, {\r\n        \"number\": \"1\",\r\n        \"text\": \"YOUR INFORMATION\"\r\n    }, {\r\n        \"number\": \"2\",\r\n " +
                    "       \"text\": \"FAMILY INFORMATION\"\r\n    }, {\r\n        \"number\": \"3\",\r\n        \"text\": \"PURPOSE OF F" +
                    "ORM\"\r\n    }, {\r\n        \"number\": \"4\",\r\n        \"text\": \"THE END\"\r\n    }],\r\n    \"vars\": {\r\n " +
                    "       \"user gender\": {\r\n            \"name\": \"User Gender\",\r\n            \"type\": \"Text\",\r\n         " +
                    "   \"repeating\": false,\r\n            \"comment\": \"User's gender will be used to display appopriate avatar.\"\r\n  " +
                    "      },\r\n        \"user avatar\": {\r\n            \"name\": \"User Avatar\",\r\n            \"type\": \"Text\",\r\n " +
                    "           \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n        \"client first name te\": {\r\n" +
                    "            \"name\": \"Client first name TE\",\r\n            \"type\": \"Text\",\r\n            \"repeating\": false,\r\n" +
                    "            \"comment\": \"\"\r\n        },\r\n        \"client middle name te\": {\r\n            \"name\": \"Client" +
                    " middle name TE\",\r\n            \"type\": \"Text\",\r\n            \"repeating\": false,\r\n            \"comment\":" +
                    " \"\"\r\n        },\r\n        \"client last name te\": {\r\n            \"name\": \"Client last name TE\",\r\n     " +
                    "       \"type\": \"Text\",\r\n            \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n   " +
                    "     \"a2j version\": {\r\n            \"name\": \"A2J Version\",\r\n            \"type\": \"Text\",\r\n          " +
                    "  \"repeating\": false,\r\n            \"comment\": \"A2J Author Version\"\r\n        },\r\n        \"a2j interview id\": " +
                    "{\r\n            \"name\": \"A2J Interview ID\",\r\n            \"type\": \"Text\",\r\n            \"repeating\": false,\r\n " +
                    "           \"comment\": \"Guide ID\"\r\n        },\r\n        \"a2j bookmark\": {\r\n            \"name\": \"A2J B" +
                    "ookmark\",\r\n            \"type\": \"Text\",\r\n            \"repeating\": false,\r\n            \"comment\": \"C" +
                    "urrent Page\"\r\n        },\r\n        \"a2j history\": {\r\n            \"name\": \"A2J History\",\r\n            " +
                    "\"type\": \"Text\",\r\n            \"repeating\": false,\r\n            \"comment\": \"Progress History List (XML)\"\r\n " +
                    "       },\r\n        \"a2j navigation tf\": {\r\n            \"name\": \"A2J Navigation TF\",\r\n            " +
                    "\"type\": \"TF\",\r\n            \"repeating\": false,\r\n            \"comment\": \"Allow navigation?\"\r\n     " +
                    "   },\r\n        \"a2j interview incomplete tf\": {\r\n            \"name\": \"A2J Interview Incomplete TF\",\r\n  " +
                    "          \"type\": \"TF\",\r\n            \"repeating\": false,\r\n            \"comment\": \"Reached Successful " +
                    "Exit?\"\r\n        },\r\n        \"a2j step 0\": {\r\n            \"name\": \"A2J Step 0\",\r\n            " +
                    "\"type\": \"Text\",\r\n            \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n   " +
                    "     \"a2j step 1\": {\r\n            \"name\": \"A2J Step 1\",\r\n            \"type\": \"Text\",\r\n         " +
                    "   \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n        \"a2j step 2\": {\r\n        " +
                    "    \"name\": \"A2J Step 2\",\r\n            \"type\": \"Text\",\r\n            \"repeating\": false,\r\n      " +
                    "      \"comment\": \"\"\r\n        },\r\n        \"a2j step 3\": {\r\n            \"name\": \"A2J Step 3\",\r\n " +
                    "           \"type\": \"Text\",\r\n            \"repeating\": false,\r\n            \"comment\": \"\"\r\n       " +
                    " },\r\n        \"a2j step 4\": {\r\n            \"name\": \"A2J Step 4\",\r\n            \"type\": \"Text\",\r\n " +
                    "           \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n        \"a2j step 5\": {\r\n " +
                    "           \"name\": \"A2J Step 5\",\r\n            \"type\": \"Text\",\r\n            \"repeating\": false,\r\n    " +
                    "        \"comment\": \"\"\r\n        },\r\n        \"a2j step 6\": {\r\n            \"name\": \"A2J Step 6\",\r\n   " +
                    "         \"type\": \"Text\",\r\n            \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n  " +
                    "      \"a2j step 7\": {\r\n            \"name\": \"A2J Step 7\",\r\n            \"type\": \"Text\",\r\n           " +
                    " \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n        \"a2j step 8\": {\r\n    " +
                    "        \"name\": \"A2J Step 8\",\r\n            \"type\": \"Text\",\r\n            \"repeating\": false,\r\n   " +
                    "         \"comment\": \"\"\r\n        },\r\n        \"a2j step 9\": {\r\n            \"name\": \"A2J Step 9\",\r\n    " +
                    "        \"type\": \"Text\",\r\n            \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n    " +
                    "    \"a2j step 10\": {\r\n            \"name\": \"A2J Step 10\",\r\n            \"type\": \"Text\",\r\n          " +
                    "  \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n        \"a2j step 11\": {\r\n         " +
                    "   \"name\": \"A2J Step 11\",\r\n            \"type\": \"Text\",\r\n            \"repeating\": false,\r\n     " +
                    "       \"comment\": \"\"\r\n        },\r\n        \"address city te\": {\r\n            \"name\": \"Address city TE\",\r\n   " +
                    "         \"type\": \"Text\",\r\n            \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n   " +
                    "     \"address street te\": {\r\n            \"name\": \"Address street TE\",\r\n            \"type\": \"Text\",\r\n    " +
                    "        \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n        \"address zipcode te\": {\r\n   " +
                    "         \"name\": \"Address zipcode TE\",\r\n            \"type\": \"Text\",\r\n            \"repeating\": false,\r\n  " +
                    "          \"comment\": \"\"\r\n        },\r\n        \"child name first te\": {\r\n            \"name\": \"Child name first TE\",\r\n " +
                    "           \"type\": \"Text\",\r\n            \"repeating\": true,\r\n            \"comment\": \"\"\r\n        },\r\n " +
                    "       \"client name full te\": {\r\n            \"name\": \"Client name full TE\",\r\n            \"type\": \"Text\",\r\n      " +
                    "      \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n        \"phone number te\": {\r\n   " +
                    "         \"name\": \"Phone number TE\",\r\n            \"type\": \"Text\",\r\n            \"repeating\": false,\r\n   " +
                    "         \"comment\": \"\"\r\n        },\r\n        \"purpose of form te\": {\r\n            \"name\": \"Purpose of" +
                    " form TE\",\r\n            \"type\": \"Text\",\r\n            \"repeating\": false,\r\n            \"comment\": \"\"\r\n   " +
                    "     },\r\n        \"number of children nu\": {\r\n            \"name\": \"Number of children NU\",\r\n      " +
                    "      \"type\": \"Number\",\r\n            \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n     " +
                    "   \"child dob da\": {\r\n            \"name\": \"Child DOB DA\",\r\n            \"type\": \"Date\",\r\n         " +
                    "   \"repeating\": true,\r\n            \"comment\": \"\"\r\n        },\r\n        \"today date da\": {\r\n       " +
                    "     \"name\": \"Today date DA\",\r\n            \"type\": \"Date\",\r\n            \"repeating\": false,\r\n   " +
                    "         \"comment\": \"\"\r\n        },\r\n        \"have children tf\": {\r\n            \"name\": \"Have chil" +
                    "dren TF\",\r\n            \"type\": \"TF\",\r\n            \"repeating\": false,\r\n            \"comme" +
                    "nt\": \"\"\r\n        },\r\n        \"address state mc\": {\r\n            \"name\": \"Address state MC\",\r\n " +
                    "           \"type\": \"MC\",\r\n            \"repeating\": false,\r\n            \"comment\": \"\"\r\n  " +
                    "      },\r\n        \"marital status mc\": {\r\n            \"name\": \"Marital status MC\",\r\n         " +
                    "   \"type\": \"MC\",\r\n            \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n  " +
                    "      \"child information filter\": {\r\n            \"name\": \"Child Information Filter\",\r\n      " +
                    "      \"type\": \"TF\",\r\n            \"repeating\": false,\r\n            \"comment\": \"\"\r\n       " +
                    " },\r\n        \"childcount\": {\r\n            \"name\": \"ChildCount\",\r\n            \"type\": \"Number\",\r\n " +
                    "           \"repeating\": false,\r\n            \"comment\": \"\"\r\n        },\r\n        \"a2j step 12\": {\r\n  " +
                    "          \"name\": \"A2J Step 12\",\r\n            \"type\": \"Text\",\r\n            \"repeating\": false,\r\n  " +
                    "          \"comment\": \"\"\r\n        }\r\n    },\r\n    \"pages\": {\r\n        \"1-Introduction\": {\r\n      " +
                    "      \"name\": \"1-Introduction\",\r\n            \"type\": \"A2J\",\r\n            \"step\": 0,\r\n       " +
                    "     \"repeatVar\": \"\",\r\n            \"nested\": false,\r\n            \"outerLoopVar\": \"\",\r\n        " +
                    "    \"text\": \" This is the introduction. This A2J Guided Interview was created as part of a sample exercise. It" +
                    " is not intended for use by the general public.\u00A0 \",\r\n            \"textCitation\": \"\",\r\n           " +
                    " \"textAudioURL\": \"\",\r\n            \"learn\": \"\",\r\n            \"help\": \"\",\r\n            \"helpC" +
                    "itation\": \"\",\r\n            \"helpAudioURL\": \"\",\r\n            \"helpReader\": \"\",\r\n            \"helpI" +
                    "mageURL\": \"\",\r\n            \"helpVideoURL\": \"\",\r\n            \"buttons\": [{\r\n                \"label\": \"C" +
                    "ontinue\",\r\n                \"next\": \"1-Name\",\r\n                \"url\": \"\",\r\n                \"re" +
                    "peatVar\": \"\",\r\n                \"repeatVarSet\": \"\",\r\n                \"name\": \"\",\r\n              " +
                    "  \"value\": \"\"\r\n            }],\r\n            \"fields\": [],\r\n            \"codeBefore\": \"\",\r\n     " +
                    "       \"codeAfter\": \"\",\r\n            \"codeCitation\": \"\",\r\n            \"notes\": \"\"\r\n        },\r\n   " +
                    "     \"1-Name\": {\r\n            \"name\": \"1-Name\",\r\n            \"type\": \"A2J\",\r\n        " +
                    "    \"step\": 1,\r\n            \"repeatVar\": \"\",\r\n            \"nested\": false,\r\n       " +
                    "     \"outerLoopVar\": \"\",\r\n            \"text\": \" Enter your name. \",\r\n            \"textCitation\": \"\",\r\n     " +
                    "       \"textAudioURL\": \"\",\r\n            \"learn\": \"\",\r\n            \"help\": \"\",\r\n           " +
                    " \"helpCitation\": \"\",\r\n            \"helpAudioURL\": \"\",\r\n            \"helpReader\": \"\",\r\n        " +
                    "    \"helpImageURL\": \"\",\r\n            \"helpVideoURL\": \"\",\r\n            \"buttons\": [{\r\n              " +
                    "  \"label\": \"Continue\",\r\n                \"next\": \"2-Gender\",\r\n                \"url\": \"\",\r\n        " +
                    "        \"repeatVar\": \"\",\r\n                \"repeatVarSet\": \"\",\r\n                \"name\": \"\",\r\n   " +
                    "             \"value\": \"\"\r\n            }],\r\n            \"fields\": [{\r\n                \"type\": \"text\",\r\n   " +
                    "             \"label\": \"First:\",\r\n                \"name\": \"Client first name TE\",\r\n              " +
                    "  \"value\": \"\",\r\n                \"order\": \"ASC\",\r\n                \"required\": true,\r\n              " +
                    "  \"min\": \"\",\r\n                \"max\": \"\",\r\n                \"calculator\": false,\r\n            " +
                    "    \"maxChars\": \"\",\r\n                \"listSrc\": \"\",\r\n                \"listData\": \"\",\r\n     " +
                    "           \"sample\": \"\",\r\n                \"invalidPrompt\": \"You must type a response in the highlighted space " +
                    "before you can continue.\"\r\n            }, {\r\n                \"type\": \"text\",\r\n                \"la" +
                    "bel\": \"Middle:\",\r\n                \"name\": \"Client middle name TE\",\r\n                \"value\": \"\",\r\n " +
                    "               \"order\": \"ASC\",\r\n                \"required\": false,\r\n                \"min\": \"\",\r\n    " +
                    "            \"max\": \"\",\r\n                \"calculator\": false,\r\n                \"maxChars\": \"\",\r\n      " +
                    "          \"listSrc\": \"\",\r\n                \"listData\": \"\",\r\n                \"sample\": \"\",\r\n         " +
                    "       \"invalidPrompt\": \"You must type a response in the highlighted space before you can continue.\"\r\n          " +
                    "  }, {\r\n                \"type\": \"text\",\r\n                \"label\": \"Last:\",\r\n               " +
                    " \"name\": \"Client last name TE\",\r\n                \"value\": \"\",\r\n                \"order\": \"ASC\",\r\n    " +
                    "            \"required\": true,\r\n                \"min\": \"\",\r\n                \"max\": \"\",\r\n             " +
                    "   \"calculator\": false,\r\n                \"maxChars\": \"\",\r\n                \"listSrc\": \"\",\r\n            " +
                    "    \"listData\": \"\",\r\n                \"sample\": \"\",\r\n                \"invalidPrompt\": \"You must type a " +
                    "response in the highlighted space before you can continue.\"\r\n            }],\r\n            \"codeBefore\": \"\",\r\n " +
                    "           \"codeCitation\": \"\",\r\n            \"notes\": \"\"\r\n        },\r\n        \"2-Gender\": {\r\n      " +
                    "      \"name\": \"2-Gender\",\r\n            \"type\": \"A2J\",\r\n            \"step\": 1,\r\n            \"repeatVa" +
                    "r\": \"\",\r\n            \"nested\": false,\r\n            \"outerLoopVar\": \"\",\r\n            \"text\": \"Choose y" +
                    "our gender.\",\r\n            \"textCitation\": \"\",\r\n            \"textAudioURL\": \"\",\r\n            \"lear" +
                    "n\": \"Why are you asking this question?\",\r\n            \"help\": \"This question is used to populate the avatar t" +
                    "hat will represent you through the rest of this A2J Guided Interview.\u00A0\",\r\n            \"helpCitation\": \"\",\r\n   " +
                    "         \"helpAudioURL\": \"\",\r\n            \"helpReader\": \"\",\r\n            \"helpImageURL\": \"\",\r\n        " +
                    "    \"helpVideoURL\": \"\",\r\n            \"buttons\": [{\r\n                \"label\": \"Continue\",\r\n            " +
                    "    \"next\": \"3-Address\",\r\n                \"url\": \"\",\r\n                \"repeatVar\": \"\",\r\n           " +
                    "     \"repeatVarSet\": \"\",\r\n                \"name\": \"\",\r\n                \"value\": \"\"\r\n            " +
                    "}],\r\n            \"fields\": [{\r\n                \"type\": \"gender\",\r\n                \"label\": \"Gender:\",\r\n    " +
                    "            \"name\": \"User Gender\",\r\n                \"value\": \"\",\r\n                \"order\": \"\",\r\n        " +
                    "        \"required\": true,\r\n                \"min\": \"\",\r\n                \"max\": \"\",\r\n               " +
                    " \"calculator\": false,\r\n                \"maxChars\": \"\",\r\n                \"listSrc\": \"\",\r\n          " +
                    "      \"listData\": \"\",\r\n                \"sample\": \"\",\r\n                \"invalidPrompt\": \"\"\r\n     " +
                    "       }],\r\n            \"codeBefore\": \"\",\r\n            \"codeAfter\": \"\",\r\n            \"codeCitation\": \"\",\r\n " +
                    "           \"notes\": \"\"\r\n        },\r\n        \"3-Address\": {\r\n            \"name\": \"3-Address\",\r\n        " +
                    "    \"type\": \"A2J\",\r\n            \"step\": 1,\r\n            \"repeatVar\": \"\",\r\n            \"nested\": false,\r\n    " +
                    "        \"outerLoopVar\": \"\",\r\n            \"text\": \"%%[Client first name TE]%% what is your address?&nbsp;\",\r\n  " +
                    "          \"textCitation\": \"\",\r\n            \"textAudioURL\": \"\",\r\n            \"learn\": \"\",\r\n         " +
                    "   \"help\": \"\",\r\n            \"helpCitation\": \"\",\r\n            \"helpAudioURL\": \"\",\r\n           " +
                    " \"helpReader\": \"\",\r\n            \"helpImageURL\": \"\",\r\n            \"helpVideoURL\": \"\",\r\n        " +
                    "    \"buttons\": [{\r\n                \"label\": \"Continue\",\r\n                \"next\": \"4-Phone number\",\r\n     " +
                    "           \"url\": \"\",\r\n                \"repeatVar\": \"\",\r\n                \"repeatVarSet\": \"\",\r\n  " +
                    "              \"name\": \"\",\r\n                \"value\": \"\"\r\n            }],\r\n            \"fields\": [{\r\n " +
                    "               \"type\": \"text\",\r\n                \"label\": \"Street\",\r\n                \"name\": \"Address street TE\",\r\n  " +
                    "              \"value\": \"\",\r\n                \"order\": \"\",\r\n                \"required\": false,\r\n          " +
                    "      \"min\": \"\",\r\n                \"max\": \"\",\r\n                \"calculator\": false,\r\n               " +
                    " \"maxChars\": \"\",\r\n                \"listSrc\": \"\",\r\n                \"listData\": \"\",\r\n                " +
                    "\"sample\": \"\",\r\n                \"invalidPrompt\": \"\"\r\n            }, {\r\n                \"type\": \"text\",\r\n " +
                    "               \"label\": \"City\",\r\n                \"name\": \"Address city TE\",\r\n                \"value\": \"\",\r\n    " +
                    "            \"order\": \"\",\r\n                \"required\": false,\r\n                \"min\": \"\",\r\n               " +
                    " \"max\": \"\",\r\n                \"calculator\": false,\r\n                \"maxChars\": \"\",\r\n            " +
                    "    \"listSrc\": \"\",\r\n                \"listData\": \"\",\r\n                \"sample\": \"\",\r\n          " +
                    "      \"invalidPrompt\": \"\"\r\n            }, {\r\n                \"type\": \"textpick\",\r\n               " +
                    " \"label\": \"State\",\r\n                \"name\": \"Address state MC\",\r\n                \"value\": \"\",\r\n      " +
                    "          \"order\": \"\",\r\n                \"required\": false,\r\n                \"min\": \"\",\r\n               " +
                    " \"max\": \"\",\r\n                \"calculator\": false,\r\n                \"maxChars\": \"\",\r\n              " +
                    "  \"listSrc\": \"US_states.xml\",\r\n                \"listData\": \"\",\r\n                \"sample\": \"\",\r\n      " +
                    "          \"invalidPrompt\": \"\"\r\n            }, {\r\n                \"type\": \"numberzip\",\r\n              " +
                    "  \"label\": \"Zip code\",\r\n                \"name\": \"Address zipcode TE\",\r\n                \"value\": \"\",\r\n   " +
                    "             \"order\": \"\",\r\n                \"required\": false,\r\n                \"min\": \"\",\r\n              " +
                    "  \"max\": \"\",\r\n                \"calculator\": false,\r\n                \"maxChars\": \"\",\r\n            " +
                    "    \"listSrc\": \"\",\r\n                \"listData\": \"\",\r\n                \"sample\": \"\",\r\n             " +
                    "   \"invalidPrompt\": \"\"\r\n            }],\r\n            \"codeBefore\": \"\",\r\n            \"codeAfter\": \"\",\r\n  " +
                    "          \"codeCitation\": \"\",\r\n            \"notes\": \"\"\r\n        },\r\n        \"4-Phone number\": {\r\n         " +
                    "   \"name\": \"4-Phone number\",\r\n            \"type\": \"A2J\",\r\n            \"step\": 1,\r\n         " +
                    "   \"repeatVar\": \"\",\r\n            \"nested\": false,\r\n            \"outerLoopVar\": \"\",\r\n            " +
                    "\"text\": \"What is your phone number?\",\r\n            \"textCitation\": \"\",\r\n            \"textAudioURL\": \"\",\r\n    " +
                    "        \"learn\": \"\",\r\n            \"help\": \"\",\r\n            \"helpCitation\": \"\",\r\n          " +
                    "  \"helpAudioURL\": \"\",\r\n            \"helpReader\": \"\",\r\n            \"helpImageURL\": \"\",\r\n         " +
                    "   \"helpVideoURL\": \"\",\r\n            \"buttons\": [{\r\n                \"label\": \"Continue\",\r\n             " +
                    "   \"next\": \"1-Marital Status\",\r\n                \"url\": \"\",\r\n                \"repeatVar\": \"\",\r\n       " +
                    "         \"repeatVarSet\": \"\",\r\n                \"name\": \"\",\r\n                \"value\": \"\"\r\n           " +
                    " }],\r\n            \"fields\": [{\r\n                \"type\": \"numberphone\",\r\n                \"label\": \"P" +
                    "hone number\",\r\n                \"name\": \"Phone number TE\",\r\n                \"value\": \"\",\r\n              " +
                    "  \"order\": \"\",\r\n                \"required\": false,\r\n                \"min\": \"\",\r\n               " +
                    " \"max\": \"\",\r\n                \"calculator\": false,\r\n                \"maxChars\": \"\",\r\n                " +
                    "\"listSrc\": \"\",\r\n                \"listData\": \"\",\r\n                \"sample\": \"\",\r\n               " +
                    " \"invalidPrompt\": \"\"\r\n            }],\r\n            \"codeBefore\": \"\",\r\n            \"codeAfter\": \"SET [" +
                    "Today date DA] TO TODAY\",\r\n            \"codeCitation\": \"\",\r\n            \"notes\": \"\"\r\n        },\r\n      " +
                    "  \"1-Marital Status\": {\r\n            \"name\": \"1-Marital Status\",\r\n            \"type\": \"A2J\",\r\n          " +
                    "  \"step\": 2,\r\n            \"repeatVar\": \"\",\r\n            \"nested\": false,\r\n            \"outerLoo" +
                    "pVar\": \"\",\r\n            \"text\": \"What is your marital status?\",\r\n            \"textCitation\": \"\",\r\n   " +
                    "         \"textAudioURL\": \"\",\r\n            \"learn\": \"\",\r\n            \"help\": \"\",\r\n            \"helpCita" +
                    "tion\": \"\",\r\n            \"helpAudioURL\": \"\",\r\n            \"helpReader\": \"\",\r\n            \"helpImage" +
                    "URL\": \"\",\r\n            \"helpVideoURL\": \"\",\r\n            \"buttons\": [{\r\n                \"label\": \"Co" +
                    "ntinue\",\r\n                \"next\": \"2-Have children?\",\r\n                \"url\": \"\",\r\n                \"re" +
                    "peatVar\": \"\",\r\n                \"repeatVarSet\": \"\",\r\n                \"name\": \"\",\r\n                \"va" +
                    "lue\": \"\"\r\n            }],\r\n            \"fields\": [{\r\n                \"type\": \"radio\",\r\n                \"la" +
                    "bel\": \"Married\",\r\n                \"name\": \"Marital status MC\",\r\n                \"value\": \"Marrie" +
                    "d\",\r\n                \"order\": \"\",\r\n                \"required\": false,\r\n                \"min\": \"\",\r\n  " +
                    "              \"max\": \"\",\r\n                \"calculator\": false,\r\n                \"maxChars\": \"\",\r\n  " +
                    "              \"listSrc\": \"\",\r\n                \"listData\": \"\",\r\n                \"sample\": \"\",\r\n     " +
                    "           \"invalidPrompt\": \"\"\r\n            }, {\r\n                \"type\": \"radio\",\r\n              " +
                    "  \"label\": \"Single\",\r\n                \"name\": \"Marital status MC\",\r\n                \"value\": \"Single\",\r\n     " +
                    "           \"order\": \"\",\r\n                \"required\": false,\r\n                \"min\": \"\",\r\n            " +
                    "    \"max\": \"\",\r\n                \"calculator\": false,\r\n                \"maxChars\": \"\",\r\n               " +
                    " \"listSrc\": \"\",\r\n                \"listData\": \"\",\r\n                \"sample\": \"\",\r\n              " +
                    "  \"invalidPrompt\": \"\"\r\n            }, {\r\n                \"type\": \"radio\",\r\n               " +
                    " \"label\": \"Divorced\",\r\n                \"name\": \"Marital status MC\",\r\n                \"value\": \"Divorced\",\r\n  " +
                    "              \"order\": \"\",\r\n                \"required\": false,\r\n                \"min\": \"\",\r\n             " +
                    "   \"max\": \"\",\r\n                \"calculator\": false,\r\n                \"maxChars\": \"\",\r\n           " +
                    "     \"listSrc\": \"\",\r\n                \"listData\": \"\",\r\n                \"sample\": \"\",\r\n          " +
                    "      \"invalidPrompt\": \"\"\r\n            }, {\r\n                \"type\": \"radio\",\r\n                \"label\": \"Wido" +
                    "wed\",\r\n                \"name\": \"Marital status MC\",\r\n                \"value\": \"Widowed\",\r\n             " +
                    "   \"order\": \"\",\r\n                \"required\": false,\r\n                \"min\": \"\",\r\n                \"max" +
                    "\": \"\",\r\n                \"calculator\": false,\r\n                \"maxChars\": \"\",\r\n                \"listSrc\": \"\",\r\n  " +
                    "              \"listData\": \"\",\r\n                \"sample\": \"\",\r\n                \"invalidPrompt\": \"\"\r\n  " +
                    "          }],\r\n            \"codeBefore\": \"\",\r\n            \"codeAfter\": \"\",\r\n            " +
                    "\"codeCitation\": \"\",\r\n            \"notes\": \"\"\r\n        },\r\n        \"2-Have children?\": {\r\n          " +
                    "  \"name\": \"2-Have children?\",\r\n            \"type\": \"A2J\",\r\n            \"step\": 2,\r\n           " +
                    " \"repeatVar\": \"\",\r\n            \"nested\": false,\r\n            \"outerLoopVar\": \"\",\r\n           " +
                    " \"text\": \"Do you have children?\",\r\n            \"textCitation\": \"\",\r\n            \"textAudioURL\": \"\",\r\n " +
                    "           \"learn\": \"\",\r\n            \"help\": \"\",\r\n            \"helpCitation\": \"\",\r\n          " +
                    "  \"helpAudioURL\": \"\",\r\n            \"helpReader\": \"\",\r\n            \"helpImageURL\": \"\",\r\n           " +
                    " \"helpVideoURL\": \"\",\r\n            \"buttons\": [{\r\n                \"label\": \"Yes\",\r\n             " +
                    "   \"next\": \"3-How many children?\",\r\n                \"url\": \"\",\r\n                \"repeatVar\": \"\",\r\n  " +
                    "              \"repeatVarSet\": \"\",\r\n                \"name\": \"Have children TF\",\r\n              " +
                    "  \"value\": \"true\"\r\n            }, {\r\n                \"label\": \"No\",\r\n                " +
                    "\"next\": \"1-Purpose of form\",\r\n                \"url\": \"\",\r\n                \"repeatVar\": \"\",\r\n  " +
                    "              \"repeatVarSet\": \"\",\r\n                \"name\": \"Have children TF\",\r\n             " +
                    "   \"value\": \"false\"\r\n            }],\r\n            \"fields\": [],\r\n            \"codeBefore\": \"\",\r\n   " +
                    "         \"codeAfter\": \"\",\r\n            \"codeCitation\": \"\",\r\n            \"notes\": \"\"\r\n        },\r\n     " +
                    "   \"3-How many children?\": {\r\n            \"name\": \"3-How many children?\",\r\n            \"type\": \"A2J\",\r\n        " +
                    "    \"step\": 2,\r\n            \"repeatVar\": \"\",\r\n            \"nested\": false,\r\n            " +
                    "\"outerLoopVar\": \"\",\r\n            \"text\": \"How many children do you have?\u00A0\",\r\n          " +
                    "  \"textCitation\": \"\",\r\n            \"textAudioURL\": \"\",\r\n            \"learn\": \"\",\r\n          " +
                    "  \"help\": \"\",\r\n            \"helpCitation\": \"\",\r\n            \"helpAudioURL\": \"\",\r\n        " +
                    "    \"helpReader\": \"\",\r\n            \"helpImageURL\": \"\",\r\n            \"helpVideoURL\": \"\",\r\n     " +
                    "       \"buttons\": [{\r\n                \"label\": \"Continue\",\r\n                \"next\": \"4-Child's name and bi" +
                    "rthdate\",\r\n                \"url\": \"\",\r\n                \"repeatVar\": \"ChildCount\",\r\n                \"r" +
                    "epeatVarSet\": \"=1\",\r\n                \"name\": \"\",\r\n                \"value\": \"\"\r\n            }],\r\n  " +
                    "          \"fields\": [{\r\n                \"type\": \"numberpick\",\r\n                \"label\": \"Number of c" +
                    "hildren\",\r\n                \"name\": \"Number of children NU\",\r\n                \"value\": \"\",\r\n         " +
                    "       \"order\": \"\",\r\n                \"required\": false,\r\n                \"min\": \"1\",\r\n             " +
                    "   \"max\": \"4\",\r\n                \"calculator\": false,\r\n                \"maxChars\": \"\",\r\n                \"li" +
                    "stSrc\": \"\",\r\n                \"listData\": \"\",\r\n                \"sample\": \"\",\r\n                \"inva" +
                    "lidPrompt\": \"\"\r\n            }],\r\n            \"codeBefore\": \"\",\r\n            \"codeAfter\": \"\",\r\n   " +
                    "         \"codeCitation\": \"\",\r\n            \"notes\": \"\"\r\n        },\r\n        \"1-The End\": {\r\n            \"n" +
                    "ame\": \"1-The End\",\r\n            \"type\": \"A2J\",\r\n            \"step\": 4,\r\n            \"repe" +
                    "atVar\": \"\",\r\n            \"nested\": false,\r\n            \"outerLoopVar\": \"\",\r\n            \"text\": \"Cong" +
                    "ratulations! You have finished your A2J Guided Interview. Click Get My Document to print your completed f" +
                    "orm.\u00A0\",\r\n            \"textCitation\": \"\",\r\n            \"textAudioURL\": \"\",\r\n            \"learn\": \"\",\r\n  " +
                    "          \"help\": \"\",\r\n            \"helpCitation\": \"\",\r\n            \"helpAudioURL\": \"\",\r\n          " +
                    "  \"helpReader\": \"\",\r\n            \"helpImageURL\": \"\",\r\n            \"helpVideoURL\": \"\",\r\n          " +
                    "  \"buttons\": [{\r\n                \"label\": \"Get My Document\",\r\n                \"next\": \"SUCCESS\",\r\n     " +
                    "           \"url\": \"\",\r\n                \"repeatVar\": \"\",\r\n                \"repeatVarSet\": \"\",\r\n         " +
                    "       \"name\": \"\",\r\n                \"value\": \"\"\r\n            }],\r\n            \"fields\": [],\r\n          " +
                    "  \"codeBefore\": \"\",\r\n            \"codeAfter\": \"\",\r\n            \"codeCitation\": \"\",\r\n         " +
                    "   \"notes\": \"\"\r\n        },\r\n        \"1-Purpose of form\": {\r\n            \"name\": \"1-Purpose of form\",\r\n   " +
                    "         \"type\": \"A2J\",\r\n            \"step\": 3,\r\n            \"repeatVar\": \"\",\r\n           " +
                    " \"nested\": false,\r\n            \"outerLoopVar\": \"\",\r\n            \"text\": \"Why are you filling out this form?\",\r\n   " +
                    "         \"textCitation\": \"\",\r\n            \"textAudioURL\": \"\",\r\n            \"learn\": \"\",\r\n          " +
                    "  \"help\": \"\",\r\n            \"helpCitation\": \"\",\r\n            \"helpAudioURL\": \"\",\r\n          " +
                    "  \"helpReader\": \"\",\r\n            \"helpImageURL\": \"\",\r\n            \"helpVideoURL\": \"\",\r\n     " +
                    "       \"buttons\": [{\r\n                \"label\": \"Continue\",\r\n                \"next\": \"1-The End\",\r\n    " +
                    "            \"url\": \"\",\r\n                \"repeatVar\": \"\",\r\n                \"repeatVarSet\": \"\",\r\n   " +
                    "             \"name\": \"\",\r\n                \"value\": \"\"\r\n            }],\r\n            \"fields\": [{\r\n     " +
                    "           \"type\": \"textlong\",\r\n                \"label\": \"\",\r\n                \"name\": \"Purpose of form TE\",\r\n   " +
                    "             \"value\": \"\",\r\n                \"order\": \"\",\r\n                \"required\": false,\r\n           " +
                    "     \"min\": \"\",\r\n                \"max\": \"\",\r\n                \"calculator\": false,\r\n             " +
                    "   \"maxChars\": \"\",\r\n                \"listSrc\": \"\",\r\n                \"listData\": \"\",\r\n          " +
                    "      \"sample\": \"\",\r\n                \"invalidPrompt\": \"\"\r\n            }],\r\n            \"codeBefore\": \"\",\r\n " +
                    "           \"codeAfter\": \"\",\r\n            \"codeCitation\": \"\",\r\n            \"notes\": \"\"\r\n     " +
                    "   },\r\n        \"4-Child's name and birthdate\": {\r\n            \"name\": \"4-Child's name and birthdate\",\r\n " +
                    "           \"type\": \"A2J\",\r\n            \"step\": 2,\r\n            \"repeatVar\": \"ChildCount\",\r\n         " +
                    "   \"nested\": false,\r\n            \"outerLoopVar\": \"\",\r\n            \"text\": \"What is your %%ORDINAL(Chil" +
                    "dCount)%% child's name and date of birth?&nbsp;\",\r\n            \"textCitation\": \"\",\r\n            \"textAudi" +
                    "oURL\": \"\",\r\n            \"learn\": \"\",\r\n            \"help\": \"\",\r\n            \"helpCitation\": \"\",\r\n   " +
                    "         \"helpAudioURL\": \"\",\r\n            \"helpReader\": \"\",\r\n            \"helpImageURL\": \"\",\r\n        " +
                    "    \"helpVideoURL\": \"\",\r\n            \"buttons\": [{\r\n                \"label\": \"Continue\",\r\n            " +
                    "    \"next\": \"\",\r\n                \"url\": \"\",\r\n                \"repeatVar\": \"\",\r\n                \"repea" +
                    "tVarSet\": \"\",\r\n                \"name\": \"\",\r\n                \"value\": \"\"\r\n            }],\r\n            " +
                    "\"fields\": [{\r\n                \"type\": \"text\",\r\n                \"label\": \"Child's first name:\",\r\n         " +
                    "       \"name\": \"Child name first TE\",\r\n                \"value\": \"\",\r\n                \"order\": \"\",\r\n   " +
                    "             \"required\": false,\r\n                \"min\": \"\",\r\n                \"max\": \"\",\r\n             " +
                    "   \"calculator\": false,\r\n                \"maxChars\": \"\",\r\n                \"listSrc\": \"\",\r\n              " +
                    "  \"listData\": \"\",\r\n                \"sample\": \"\",\r\n                \"invalidPrompt\": \"\"\r\n            }, {\r\n   " +
                    "             \"type\": \"datemdy\",\r\n                \"label\": \"Date of birth:\",\r\n                \"name\": \"Child DOB" +
                    " DA\",\r\n                \"value\": \"\",\r\n                \"order\": \"\",\r\n                \"required\": false,\r\n   " +
                    "             \"min\": \"\",\r\n                \"max\": \"\",\r\n                \"calculator\": false,\r\n              " +
                    "  \"maxChars\": \"\",\r\n                \"listSrc\": \"\",\r\n                \"listData\": \"\",\r\n               " +
                    " \"sample\": \"\",\r\n                \"invalidPrompt\": \"\"\r\n            }],\r\n            \"codeBefore\": \"\",\r\n   " +
                    "         \"codeCitation\": \"\",\r\n            \"notes\": \"\"\r\n        }\r\n    }\r\n}";
            }
        }
    }
}
