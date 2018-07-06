using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Api.Tests.TestData
{
    class UserPersonalizedPlanTestData
    {
        public static JArray userProfile =
                  JArray.Parse(@"[{'id': '4589592f-3312-eca7-64ed-f3561bbb7398',
                    'oId': '709709e7t0r7t96', 'firstName': 'family1.2.1', 'lastName': '5c035d27-2fdb-9776-6236-70983a918431','email': 'f102bfae-362d-4659-aaef-956c391f79de'}]");
        public static JArray userProfilePersonalizedPlanData =
             JArray.Parse(@"[{'planId':'d984fc20-ae28-4749-942a-16e2b10f0a20','oId':'outlookoremailOId','type':'plans','planTags':[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','stepTags':[{'id':'6b230be1-302b-7090-6cb3-fc6aa084274c',
                                    'order':'1','markCompleted':'false'},{'id':'d46aecee-8c79-df1b-4081-1ea02b5022df','order':'2','markCompleted':'true'}]},{'id':'932abb0a-c6bb-46da-a3d8-5f52c2c914a0','stepTags':[
                                    {'id':'2705d544-6af7-bd69-4f19-a1b53e346da2','order':'2','markCompleted':'false'},{'id':'3d64b676-cc4b-397d-a5bb-f4a0ea6d3040','order':'1','markCompleted':'false'}]}]}]");
        public static JArray expectedUserProfilePersonalizedPlanData =
                    JArray.Parse(@"[{'planId':'d984fc20-ae28-4749-942a-16e2b10f0a20','oId':'outlookoremailOId','type':'plans','planTags':[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','stepTags':[{'id':'6b230be1-302b-7090-6cb3-fc6aa084274c',
                                    'order':'1','markCompleted':'false'},{'id':'d46aecee-8c79-df1b-4081-1ea02b5022df','order':'2','markCompleted':'true'}]},{'id':'932abb0a-c6bb-46da-a3d8-5f52c2c914a0','stepTags':[
                                    {'id':'2705d544-6af7-bd69-4f19-a1b53e346da2','order':'2','markCompleted':'false'},{'id':'3d64b676-cc4b-397d-a5bb-f4a0ea6d3040','order':'1','markCompleted':'false'}]}]}]");
        public static JArray expectedUserProfilePersonalizedPlanUpdateData =
                   JArray.Parse(@"[{'planId':'d984fc20-ae28-4749-942a-16e2b10f0a20','oId':'outlookoremailOId','type':'plans','planTags':[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','stepTags':[{'id':'6b230be1-302b-7090-6cb3-fc6aa084274c',
                                    'order':'1','markCompleted':'false'},{'id':'d46aecee-8c79-df1b-4081-1ea02b5022df','order':'2','markCompleted':'false'}]},{'id':'932abb0a-c6bb-46da-a3d8-5f52c2c914a0','stepTags':[
                                    {'id':'2705d544-6af7-bd69-4f19-a1b53e346da2','order':'2','markCompleted':'false'},{'id':'3d64b676-cc4b-397d-a5bb-f4a0ea6d3040','order':'1','markCompleted':'false'}]}],'id':'f8d41156-4c7e-44c8-a543-b6c40f661900'}]");
        public static JArray userProfileSavedResourcesData =
                    JArray.Parse(@"[{'resourcesId': 'd984fc20-ae28-4749-942a-16e2b10f0a20','oId': 'outlookoremailOId','type': 'resources','resourceTags': [{'id': 'addf41e9-1a27-4aeb-bcbb-7959f95094ba'},{'id': 'd46aecee-8c79-df1b-4081-1ea02b5022df'}]}]");
        public static JArray expectedUserProfileSavedResourcesData =
            JArray.Parse(@"[{'resourcesId': 'd984fc20-ae28-4749-942a-16e2b10f0a20','oId': 'outlookoremailOId','type': 'resources','resourceTags': [{'id': 'addf41e9-1a27-4aeb-bcbb-7959f95094ba'},{'id': 'd46aecee-8c79-df1b-4081-1ea02b5022df'}]}]");
        public static JArray expectedUserProfileSavedResourcesUpdateData =
            JArray.Parse(@"[{'resourcesId': 'd984fc20-ae28-4749-942a-16e2b10f0a20','oId': 'outlookoremailOId','type': 'resources','resourceTags': [{'id': 'addf41e9-1a27-4aeb-bcbb-7959f95094ba'},{'id': 'd46aecee-8c79-df1b-4081-1ea02b5022df'}],'id':'f8d41156-4c7e-44c8-a543-b6c40f661900'}]");
    }
}