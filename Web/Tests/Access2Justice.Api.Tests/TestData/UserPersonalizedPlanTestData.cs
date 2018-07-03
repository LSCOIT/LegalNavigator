using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Api.Tests.TestData
{
    class UserPersonalizedPlanTestData
    {
        public static JArray userProfilePersonalizedPlanData =
             JArray.Parse(@"[{'id':'d984fc20-ae28-4749-942a-16e2b10f0a20','oId':'outlookoremailOId','type':'plans','planTags':[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','stepTags':[{'id':'6b230be1-302b-7090-6cb3-fc6aa084274c',
                                    'order':'1','markCompleted':'false'},{'id':'d46aecee-8c79-df1b-4081-1ea02b5022df','order':'2','markCompleted':'true'}]},{'id':'932abb0a-c6bb-46da-a3d8-5f52c2c914a0','stepTags':[
                                    {'id':'2705d544-6af7-bd69-4f19-a1b53e346da2','order':'2','markCompleted':'false'},{'id':'3d64b676-cc4b-397d-a5bb-f4a0ea6d3040','order':'1','markCompleted':'false'}]}]}]");
        public static JArray expectedUserProfilePersonalizedPlanData =
                    JArray.Parse(@"[{'id':'d984fc20-ae28-4749-942a-16e2b10f0a20','oId':'outlookoremailOId','type':'plans','planTags':[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','stepTags':[{'id':'6b230be1-302b-7090-6cb3-fc6aa084274c',
                                    'order':'1','markCompleted':'false'},{'id':'d46aecee-8c79-df1b-4081-1ea02b5022df','order':'2','markCompleted':'true'}]},{'id':'932abb0a-c6bb-46da-a3d8-5f52c2c914a0','stepTags':[
                                    {'id':'2705d544-6af7-bd69-4f19-a1b53e346da2','order':'2','markCompleted':'false'},{'id':'3d64b676-cc4b-397d-a5bb-f4a0ea6d3040','order':'1','markCompleted':'false'}]}]}]");
        public static JArray expectedUserProfilePersonalizedPlanUpdateData =
                   JArray.Parse(@"[{'id':'d984fc20-ae28-4749-942a-16e2b10f0a20','oId':'outlookoremailOId','type':'plans','planTags':[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','stepTags':[{'id':'6b230be1-302b-7090-6cb3-fc6aa084274c',
                                    'order':'1','markCompleted':'false'},{'id':'d46aecee-8c79-df1b-4081-1ea02b5022df','order':'2','markCompleted':'false'}]},{'id':'932abb0a-c6bb-46da-a3d8-5f52c2c914a0','stepTags':[
                                    {'id':'2705d544-6af7-bd69-4f19-a1b53e346da2','order':'2','markCompleted':'false'},{'id':'3d64b676-cc4b-397d-a5bb-f4a0ea6d3040','order':'1','markCompleted':'false'}]}]}]");
    }
}