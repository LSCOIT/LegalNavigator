using Access2Justice.Shared.Models;
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
             JArray.Parse(@"[{'PersonalizedActionPlanId':'d984fc20-ae28-4749-942a-16e2b10f0a20','oId':'outlookoremailOId','type':'plans','planTags':[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','stepTags':[{'id':'6b230be1-302b-7090-6cb3-fc6aa084274c',
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
                    JArray.Parse(@"[{'id': 'd984fc20-ae28-4749-942a-16e2b10f0a20','oId': 'outlookoremailOId','type': 'resources','resourceTags': [{'id': 'addf41e9-1a27-4aeb-bcbb-7959f95094ba'},{'id': 'd46aecee-8c79-df1b-4081-1ea02b5022df'}]}]");
        public static JArray expectedUserProfileSavedResourcesData =
            JArray.Parse(@"[{'id': 'd984fc20-ae28-4749-942a-16e2b10f0a20','oId': 'outlookoremailOId','type': 'resources','resourceTags': [{'id': 'addf41e9-1a27-4aeb-bcbb-7959f95094ba'},{'id': 'd46aecee-8c79-df1b-4081-1ea02b5022df'}]}]");
        public static JArray expectedUserProfileSavedResourcesUpdateData =
            JArray.Parse(@"[{'id': 'd984fc20-ae28-4749-942a-16e2b10f0a20','oId': 'outlookoremailOId','type': 'resources','resourceTags': [{'id': 'addf41e9-1a27-4aeb-bcbb-7959f95094ba'},{'id': 'd46aecee-8c79-df1b-4081-1ea02b5022df'}],'id':'f8d41156-4c7e-44c8-a543-b6c40f661900'}]");
        public static JArray userPlanData =
             JArray.Parse(@"[{'id':'d984fc20-ae28-4749-942a-16e2b10f0a20','oId':'outlookoremailOId','type':'plans','planTags':[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','stepTags':[{'id':'6b230be1-302b-7090-6cb3-fc6aa084274c',
                                    'order':'1','markCompleted':'false'},{'id':'d46aecee-8c79-df1b-4081-1ea02b5022df','order':'2','markCompleted':'true'}]},{'id':'932abb0a-c6bb-46da-a3d8-5f52c2c914a0','stepTags':[
                                    {'id':'2705d544-6af7-bd69-4f19-a1b53e346da2','order':'2','markCompleted':'false'},{'id':'3d64b676-cc4b-397d-a5bb-f4a0ea6d3040','order':'1','markCompleted':'false'}]}]}]");
        public static JArray expectedUserPlanData =
                    JArray.Parse(@"[{'id':'d984fc20-ae28-4749-942a-16e2b10f0a20','oId':'outlookoremailOId','type':'plans','planTags':[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','stepTags':[{'id':'6b230be1-302b-7090-6cb3-fc6aa084274c',
                                    'order':'1','markCompleted':'false'},{'id':'d46aecee-8c79-df1b-4081-1ea02b5022df','order':'2','markCompleted':'true'}]},{'id':'932abb0a-c6bb-46da-a3d8-5f52c2c914a0','stepTags':[
                                    {'id':'2705d544-6af7-bd69-4f19-a1b53e346da2','order':'2','markCompleted':'false'},{'id':'3d64b676-cc4b-397d-a5bb-f4a0ea6d3040','order':'1','markCompleted':'false'}]}]}]");
        public static JArray expectedUserPlanUpdateData =
                   JArray.Parse(@"[{'id':'d984fc20-ae28-4749-942a-16e2b10f0a20','oId':'outlookoremailOId','type':'plans','planTags':[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','stepTags':[{'id':'6b230be1-302b-7090-6cb3-fc6aa084274c',
                                    'order':'1','markCompleted':'false'},{'id':'d46aecee-8c79-df1b-4081-1ea02b5022df','order':'2','markCompleted':'false'}]},{'id':'932abb0a-c6bb-46da-a3d8-5f52c2c914a0','stepTags':[
                                    {'id':'2705d544-6af7-bd69-4f19-a1b53e346da2','order':'2','markCompleted':'false'},{'id':'3d64b676-cc4b-397d-a5bb-f4a0ea6d3040','order':'1','markCompleted':'false'}]}]}]");

        public static JArray expectedUserRoleData = JArray.Parse(@"[{
                                    'id': '4bf9df8f-dfee-4b08-be4d-35cc053fa298',
                                    'roleName': 'Authenticated',
                                    'type': 'Role',
                                    'permissions': [ 'upsertstatichomepage', 'upsertstaticprivacypage', 'upsertstatichelpandfaqpage', 'upsertstaticnavigation','upserttopic','upsertresource']
                                    }]");

        public static IEnumerable<object[]> UserProfileResponseData()
        {
            yield return new object[] {
                new UserProfile { Id = "08f68ff8-37ed-44e6-8f29-c8616a4c2d23", OId="7576F1E59E1296195ECD8A08614E4F904093B2DCD2F4D7F39973A957D33880C08BE648DA853B8350944862D45A7B7FD297C78467DD0579A9F4DECA4068AB9E6A", IsActive= "Yes", Name="TestFirstName TestLastName"},
                JArray.Parse(@"[]"),
                JArray.Parse(@"[{'oId':'7576F1E59E1296195ECD8A08614E4F904093B2DCD2F4D7F39973A957D33880C08BE648DA853B8350944862D45A7B7FD297C78467DD0579A9F4DECA4068AB9E6A','name':'TestFirstName TestLastName','firstName':'TestFirstName','lastName':'TestLastName','isActive':'Yes','sharedResourceId':'00000000-0000-0000-0000-000000000000','personalizedActionPlanId':'00000000-0000-0000-0000-000000000000','curatedExperienceAnswersId':'00000000-0000-0000-0000-000000000000','savedResourcesId':'00000000-0000-0000-0000-000000000000' }]"),
                JArray.Parse(@"[{'oId':'7576F1E59E1296195ECD8A08614E4F904093B2DCD2F4D7F39973A957D33880C08BE648DA853B8350944862D45A7B7FD297C78467DD0579A9F4DECA4068AB9E6A','name':'TestFirstName TestLastName','eMail':null,'isActive':'Yes','sharedResourceId':'00000000-0000-0000-0000-000000000000','personalizedActionPlanId':'00000000-0000-0000-0000-000000000000','curatedExperienceAnswersId':'00000000-0000-0000-0000-000000000000','savedResourcesId':'00000000-0000-0000-0000-000000000000','roleInformationId':null,'roleInformation':[{'roleName':'Authenticated','organizationalUnit':''}]}]"),
                "08f68ff8-37ed-44e6-8f29-c8616a4c2d23"
            };

            yield return new object[] {
                new UserProfile { Id = "08f68ff8-37ed-44e6-8f29-c8616a4c2d23", OId="7576F1E59E1296195ECD8A08614E4F904093B2DCD2F4D7F39973A957D33880C08BE648DA853B8350944862D45A7B7FD297C78467DD0579A9F4DECA4068AB9E6A", IsActive= "Yes", Name="TestFirstName TestLastName", CuratedExperienceAnswersId= Guid.Parse("5c035d27-2fdb-9776-6236-70983a918431")},
                JArray.Parse(@"[]"),
                JArray.Parse(@"[{'oId':'7576F1E59E1296195ECD8A08614E4F904093B2DCD2F4D7F39973A957D33880C08BE648DA853B8350944862D45A7B7FD297C78467DD0579A9F4DECA4068AB9E6A','name':'TestFirstName TestLastName','firstName':'TestFirstName','lastName':'TestLastName','isActive':'Yes','sharedResourceId':'00000000-0000-0000-0000-000000000000','personalizedActionPlanId':'00000000-0000-0000-0000-000000000000','curatedExperienceAnswersId':'5c035d27-2fdb-9776-6236-70983a918431','savedResourcesId':'00000000-0000-0000-0000-000000000000' }]"),
                JArray.Parse(@"[{'oId':'7576F1E59E1296195ECD8A08614E4F904093B2DCD2F4D7F39973A957D33880C08BE648DA853B8350944862D45A7B7FD297C78467DD0579A9F4DECA4068AB9E6A','name':'TestFirstName TestLastName','eMail':null,'isActive':'Yes','sharedResourceId':'00000000-0000-0000-0000-000000000000','personalizedActionPlanId':'00000000-0000-0000-0000-000000000000','curatedExperienceAnswersId':'5c035d27-2fdb-9776-6236-70983a918431','savedResourcesId':'00000000-0000-0000-0000-000000000000','roleInformationId':null,'roleInformation':[{'roleName':'Authenticated','organizationalUnit':''}] }]"),
                "08f68ff8-37ed-44e6-8f29-c8616a4c2d23"
            };
        }
    }
}