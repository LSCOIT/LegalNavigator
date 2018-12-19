using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Api.Tests.TestData
{
    class TopicResourceTestData
    {
        public static JArray topicsData =
                  JArray.Parse(@"[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','name':'Family',
                   'ParentTopicId':'','keywords':'Eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao',
                    'zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':
                   'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'',
                   'icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'
                   ','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/',
                    '_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257}]");
        public static JArray resourcesData =
                    JArray.Parse(@"[{'id':'77d301e7-6df2-612e-4704-c04edf271806','name':'Tenant Action Plan 
                    for Eviction','description':'This action plan is for tenants who are facing Eviction and have experienced the following:',
                    'resourceType':'Action','externalUrl':'','url':'','topicTags':[{'id':'f102bfae-362d-4659-aaef-956c391f79de'},
                    {'id':'2c0cc7b8-62b1-4efb-8568-b1f767f879bc'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}],'location':[{'state':'Hawaii',
                    'city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Alaska'}],'icon':'./assets/images/resources/resource.png',
                    'createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSAJdNlwIBAAAAAAAAAA==',
                    '_self':'dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIBAAAAAAAAAA==/','_etag':'\'040007b5-0000-0000-0000-5b0792260000\'',
                    '_attachments':'attachments/','_ts':1527222822},{'id':'19a02209-ca38-4b74-bd67-6ea941d41518','name':'Legal Help Organization',
                    'description':'This action plan is for tenants who are facing Eviction and have experienced the following:','resourceType':'Organization'
                    ,'externalUrl':'','url':'','topicTags':[{'id':'f102bfae-362d-4659-aaef-956c391f79de'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}],
                    'location':[{'state':'Hawaii','city':'Kalawao','zipCode':'96742'}],'icon':'./assets/images/resources/resource.png','createdBy':'',
                    'createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSAJdNlwIBAAAAAAAAAA==','_self':
                    'dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIBAAAAAAAAAA==/','_etag':'\'040007b5-0000-0000-0000-5b0792260000\'',
                    '_attachments':'attachments/','_ts':1527222822}]");
        public static JArray breadcrumbData =
                    JArray.Parse(@"[{'id': '4589592f-3312-eca7-64ed-f3561bbb7398',
                    'parentId': '5c035d27-2fdb-9776-6236-70983a918431', 'name': 'family1.2.1'},
                    {'id': '5c035d27-2fdb-9776-6236-70983a918431','parentId': 'f102bfae-362d-4659-aaef-956c391f79de',
                    'name': 'family1.1.1'},{'id': 'f102bfae-362d-4659-aaef-956c391f79de',
                    'parentId': 'addf41e9-1a27-4aeb-bcbb-7959f95094ba','name': 'family subtopic name 1.1'
                    },{'id': 'addf41e9-1a27-4aeb-bcbb-7959f95094ba','name': 'family'}]");
        public static JArray resourceCountData = JArray.Parse(@"[{'resourceType':'Organizations'},{'resourceType':'Organizations'},{'resourceType':'Organizations'},
                    {'resourceType':'Organizations'},{'resourceType':'All'},{'resourceType':'All'}]");
        public static ResourceFilter resourceFilter = new ResourceFilter { TopicIds = new List<string> { "addf41e9-1a27-4aeb-bcbb-7959f95094ba" }, PageNumber = 0, ResourceType = "ALL", Location = new Location() };
        public static ResourceFilter resourceFilterTrue = new ResourceFilter { TopicIds = new List<string> { "addf41e9-1a27-4aeb-bcbb-7959f95094ba" }, PageNumber = 0, ResourceType = "ALL", Location = new Location(), IsResourceCountRequired = true };

        public static JArray formData =
                    JArray.Parse(@"[{'overview': 'Form1','fullDescription': 'Below is the form you will need if you are looking to settle your child custody dispute in court. We have included helpful tips to guide you along the way.',
                    'id':'77d301e7-6df2-612e-4704-c04edf271806','name': 'Form1','resourceCategory':'form','description': 'Subhead lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem',
                    'resourceType': 'Forms','url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'organizationalUnit': 'Alaska',
                    'location': [{'state': 'Hawaii','county':'','city': 'Haiku-Pauwela','zipCode':''}],'createdBy': 'API','createdTimeStamp': '','modifiedBy': 'API','modifiedTimeStamp': ''}]");
        public static JArray resourcesEmpty = JArray.Parse(@"[]");
        public static JArray actionPlanData =
                    JArray.Parse(@"[{'conditions': [{'condition': {'title': 'Take to your partner to see if you can come to an agreement', 'description': 'Why you should do this dolor sit amet.'}}],
                    'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Action Plan','resourceCategory':'action plan','description': 'This action plan is for tenants who are facing Eviction and have experienced the following:',
                    'resourceType': 'Action Plans', 'url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'organizationalUnit': 'Alaska','location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray referencesInputData =
                    JArray.Parse(@"[{'conditions': [{'condition': {'title': 'Take to your partner to see if you can come to an agreement', 'description': 'Why you should do this dolor sit amet.'}}],
                    'parentTopicId': [{'id': '349fa67b-164f-4a65-bb5d-a5b3dd2640a6'}], 'quickLinks':[{'text':'Divorce - Hawaii State Judiciary','url':'www.courts.state.hi.us/self-help/divorce'}],'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Action Plan','description': 'This action plan is for tenants who are facing Eviction and have experienced the following:',
                    'resourceType': 'Action Plans','url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}], reviewer: [{'reviewerFullName':'Full Name','reviewerTitle':'Title','reviewText':'Text','reviewerImage':'Image'}], 'contents': [{'headline':'headline','content':'content'}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray articleData =
                    JArray.Parse(@"[{'overview': 'Overview','contents':[{'headline':'headline','content':'content'}],'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Article1','resourceCategory':'article','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Articles','url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'organizationalUnit': 'Alaska','location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray videoData =
                    JArray.Parse(@"[{'overview': 'Overview','isRecommended': 'Yes','videoUrl': 'Url','id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Video','resourceCategory':'video','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Videos','url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'organizationalUnit': 'Alaska','location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray organizationData =
                    JArray.Parse(@"[{'address': '2900 E Parks Hwy Wasilla, AK 99654','telephone': '907-279-2457','overview': 'This site is a service that is made possible through the generosity of the Legal Services Corporation and is supported by a number of partner organizations throughout Alaska.  The web site was created by Pro Bono Net and is maintained by staff at Alaska Legal Services Corporation.','eligibilityInformation': 'Helping low-income individuals solve legal problems','reviewer':[{'reviewerFullName': 'Full Name','reviewerTitle': 'Title','reviewText':'Text','reviewerImage':'Image'}],'specialties':'spl', 'qualifications':'qual','businessHours':'',
                    'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Organization','resourceCategory':'org','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Organizations','url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'organizationalUnit': 'Alaska','location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray additionalReadingData =
                    JArray.Parse(@"[{'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Additional Reading in Hawaii','resourceCategory':'Additional reading','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Additional Readings','url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'organizationalUnit': 'Alaska','location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray relatedLinkData =
                    JArray.Parse(@"[{'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'External Link in Hawaii','resourceCategory':'Link','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Related Links','url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'organizationalUnit': 'Alaska','location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray topicData =
                    JArray.Parse(@"[{'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Family','overview':'overview details','parentTopicId': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'resourceType':'Topics','keywords': 'HOUSING','organizationalUnit': 'Alaska',
                    'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'jsonContent':'jsonContent','icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray topicUpsertData =
                    JArray.Parse(@"[[{'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Family','overview':'overview details','quickLinks': [{'text':'Divorce - Hawaii State Judiciary','url':'www.courts.state.hi.us/self-help/divorce'}],'parentTopicId': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'resourceType':'Topics','keywords': 'HOUSING','organizationalUnit': 'Alaska',
                    'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'jsonContent':'jsonContent','icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]]");
        public static JArray referenceTagData =
                     JArray.Parse(@"[{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}]");
        public static JArray parentTopicIdData =
                     JArray.Parse(@"[{'id': '349fa67b-164f-4a65-bb5d-a5b3dd2640a6'}]");
        public static JArray locationData =
                     JArray.Parse(@"[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''},{'state':'Alaska','county':'','city':'','zipCode':''}]");
        public static JArray quickLinksData =
                     JArray.Parse(@"[{'text':'Divorce - Hawaii State Judiciary','url':'www.courts.state.hi.us/self-help/divorce'}]");
        public static JArray conditionData =
                     JArray.Parse(@"[{'condition': {'title': 'Take to your partner to see if you can come to an agreement','description': 'Why you should do this dolor sit amet'}}]");
        public static JArray emptyResourceData = JArray.Parse(@"[{'topicTags':[{'id': ''}],'location': [{'state': '','county':'','': '','zipCode':''}],'conditions': [{'condition': {'title': '','description': ''}}],'parentTopicId':[{'id':''}], 'quickLinks':[{'text':'','url':''}], reviewer: [{'reviewerFullName':'','reviewerTitle':'','reviewText':'','reviewerImage':''}], 'contents': [{'headline':'','content':''}] }]");
        public static JArray reviewerData =
                     JArray.Parse(@"[{'reviewerFullName':'Full Name','reviewerTitle':'Title','reviewText':'Text','reviewerImage':'Image'}]");
        public static JArray contentData =
                     JArray.Parse(@"[{'headline':'headline','content':'content'}]");
        //Mcoked Data
        public static JArray emptyLocationData =
                     JArray.Parse(@"[{'state':'','county':'','city':'','zipCode':''}]");
        public static JArray emptyReviewerData =
             JArray.Parse(@"[{'reviewerFullName':'','reviewerTitle':'','reviewText':'','reviewerImage':''}]");
        public static JArray emptyContentData =
             JArray.Parse(@"[{'headline':'','content':''}]");
        public static JArray emptyConditionObject =
                     JArray.Parse(@"[{'condition':[]}]");
        public static JArray emptyQuickLinksData =
                      JArray.Parse(@"[{'text':'','url':''}]");
        public static JArray EmptyReferences =
                     JArray.Parse(@"[[{'id':''}],[{'state':'','county':'','city':'','zipCode':''}],[{'condition':[{'title':'','description':''}]}],[{'id':''}],[{'reviewerFullName':'','reviewerTitle':'','reviewText':'','reviewerImage':''}],[{'headline':'','content':''}]]");
        public static string expectedTopicId = "addf41e9-1a27-4aeb-bcbb-7959f95094ba";
        public static string expectedResourceId = "77d301e7-6df2-612e-4704-c04edf271806";
        public static Location expectedLocationValue = new Location() { State = "Hawaii", County = "", City = "Honolulu", ZipCode = "96741" };
        public static string expectedpagedResource = "{\"ContinuationToken\":\"[]\",\"Results\":[],\"TopicIds\":[]}";
        public static string expectedResourceCount = "{\"ResourceName\":\"Organizations\",\"ResourceCount\":4}";
        public static string expectedEmptyResourceCount = "{\"ResourceName\":\"All\",\"ResourceCount\":0}";
        public static JArray expectedformData =
                     JArray.Parse(@"[{'overview': 'Form1','fullDescription': 'Below is the form you will need if you are looking to settle your child custody dispute in court. We have included helpful tips to guide you along the way.',
                    'id':'77d301e7-6df2-612e-4704-c04edf271806','name': 'Form1',resourceCategory:'form','description': 'Subhead lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem',
                    'resourceType': 'Forms','url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'organizationalUnit': 'Alaska',
                    'location': [{'state': 'Hawaii','county':'','city': 'Haiku-Pauwela','zipCode':''}],'createdBy': 'API','createdTimeStamp': '','modifiedBy': 'API','modifiedTimeStamp': ''}]");
        public static JArray expectedUserProfileSavedResourcesUpdateData =
           JArray.Parse(@"[{'id': 'd984fc20-ae28-4749-942a-16e2b10f0a20','oId': 'outlookoremailOId','type': 'resources','resourceTags': [{'id': 'addf41e9-1a27-4aeb-bcbb-7959f95094ba'},{'id': 'd46aecee-8c79-df1b-4081-1ea02b5022df'}],'id':'f8d41156-4c7e-44c8-a543-b6c40f661900'}]");
        public static JArray expectedActionPlanData =
                    JArray.Parse(@"[{'conditions': [{'condition': [{'title': 'Take to your partner to see if you can come to an agreement', 'description': 'Why you should do this dolor sit amet.'}]}],
                    'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Action Plan',resourceCategory:'action plan','description': 'This action plan is for tenants who are facing Eviction and have experienced the following:',
                    'resourceType': 'Action Plans','url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'organizationalUnit': 'Alaska','location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray expectedArticleData =
                    JArray.Parse(@"[{'overview': 'Overview','contents':[{'headline':'headline','content':'content'}],'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Article1',resourceCategory:'article','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Articles','url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'organizationalUnit': 'Alaska','location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray expectedVideoData =
                    JArray.Parse(@"[{  'overview': 'Overview',  'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec',  'name': 'Video',  'resourceCategory': 'video',  'description': 'Subhead lorem ipsum solor sit amet bibodem',  'resourceType': 'Videos',    'url': 'access2justice.com',  'topicTags': [    {      'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'    }  ],'organizationalUnit': 'Alaska',  'location': [    {      'state': 'Hawaii',      'county': '',      'city': 'Haiku-Pauwela',      'zipCode': ''    },    {      'state': 'Alaska',      'county': '',      'city': '',      'zipCode': ''    }  ],  'createdBy': 'API',  'createdTimeStamp': '',  'modifiedBy': 'API',  'modifiedTimeStamp': ''}]");
        public static JArray expectedOrganizationData =
                    JArray.Parse(@"[{'address': '2900 E Parks Hwy Wasilla, AK 99654','telephone': '907-279-2457','overview': 'This site is a service that is made possible through the generosity of the Legal Services Corporation and is supported by a number of partner organizations throughout Alaska.  The web site was created by Pro Bono Net and is maintained by staff at Alaska Legal Services Corporation.','eligibilityInformation': 'Helping low-income individuals solve legal problems','reviewer':[{'reviewerFullName': 'Full Name','reviewerTitle': 'Title','reviewText':'Text','reviewerImage':'Image'}],'specialties':'spl', 'qualifications':'qual','businessHours':'',
                    'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Organization',resourceCategory:'org','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Organizations','url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'organizationalUnit': 'Alaska','location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray expectedAdditionalReadingData =
                    JArray.Parse(@"[{'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Additional Reading in Hawaii',type:'Additional reading','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Additional Readings','url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'organizationalUnit': 'Alaska','location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray expectedRelatedLinkData =
                    JArray.Parse(@"[{'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'External Link in Hawaii','resourceCategory':'Link','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Related Links','url': 'access2justice.com','topicTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'organizationalUnit': 'Alaska','location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray expectedTopicData =
                    JArray.Parse(@"[{  'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec',  'name': 'Family',  'overview': 'overview details',  'parentTopicId': [    {      'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'    }  ],  'resourceType': 'Topics',  'keywords': 'HOUSING', 'organizationalUnit': 'Alaska', 'location': [    {      'state': 'Hawaii',      'county': '',      'city': 'Haiku-Pauwela',      'zipCode': ''    },    {      'state': 'Alaska',      'county': '',      'city': '',      'zipCode': ''    }  ],  'icon': './assets/images/resources/resource.png',  'createdBy': 'API',  'createdTimeStamp': '',  'modifiedBy': 'API',  'modifiedTimeStamp': ''}]");
        public static JArray expectedTopicsData =
                    JArray.Parse(@"[{  'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec',  'name': 'Family',  'overview': 'overview details',  'parentTopicId': [    {      'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'    }  ],  'resourceType': 'Topics',  'keywords': 'HOUSING', 'organizationalUnit': 'Alaska', 'location': [    {      'state': 'Hawaii',      'county': '',      'city': 'Haiku-Pauwela',      'zipCode': ''    },    {      'state': 'Alaska',      'county': '',      'city': '',      'zipCode': ''    }  ],  'jsonContent': 'jsonContent',  'icon': './assets/images/resources/resource.png',  'createdBy': 'API',  'createdTimeStamp': '',  'modifiedBy': 'API',  'modifiedTimeStamp': ''}]");
        public static string expectedTopicTagData = "aaa085ef-96fb-4fd0-bcd0-0472ede66512";
        public static string expectedParentTopicIdData = "349fa67b-164f-4a65-bb5d-a5b3dd2640a6";
        public static JArray expectedQuickLinksData =
                     JArray.Parse(@"[{'text':'Divorce - Hawaii State Judiciary','url':'www.courts.state.hi.us/self-help/divorce'}]");
        public static JArray expectedLocationData =
                     JArray.Parse(@"[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''},{'state':'Alaska','county':'','city':'','zipCode':''}]");
        public static JArray expectedReferenceLocationData =
                     JArray.Parse(@"[{'state': 'Hawaii','county':'','city': 'Haiku-Pauwela','zipCode':''}]");
        public static JArray expectedConditionData =
                     JArray.Parse(@"[{'condition':[{'title':'Take to your partner to see if you can come to an agreement', 'description':'Why you should do this dolor sit amet'}]}]");
        public static JArray expectedQuickLinkData =
             JArray.Parse(@"[{'text':'Divorce - Hawaii State Judiciary','url':'www.courts.state.hi.us/self-help/divorce'}]");
        public static JArray expectedResourceReferences =
                     JArray.Parse(@"[[{'id':'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''},{'state':'Alaska','county':'','city':'','zipCode':''}], [{'headline':'headline','content':'content'}]]");
        public static JArray expectedActionPlanReferences =
                     JArray.Parse(@"[[{'id':'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''},{'state':'Alaska','county':'','city':'','zipCode':''}],[{'condition':[{'title':'Take to your partner to see if you can come to an agreement','description':'Why you should do this dolor sit amet.'}]}]]");
        public static JArray expectedReferencesData =
                     JArray.Parse(@"[[{'id':'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''},{'state':'Alaska','county':'','city':'','zipCode':''}],[{'condition':[{'title':'Take to your partner to see if you can come to an agreement','description':'Why you should do this dolor sit amet.'}]}],[{'id':'349fa67b-164f-4a65-bb5d-a5b3dd2640a6'}],[{'reviewerFullName':'Full Name','reviewerTitle':'Title','reviewText':'Text','reviewerImage':'Image'}], [{'headline':'headline','content':'content'}]]");
        public static JArray expectedReviewerData =
             JArray.Parse(@"[{'reviewerFullName':'Full Name','reviewerTitle':'Title','reviewText':'Text','reviewerImage':'Image'}]");
        public static JArray expectedContentData =
             JArray.Parse(@"[{'headline':'headline','content':'content'}]");

        public static Location expectedLocation = new Location
        {
            State = "Hawaii",
            County = "",
            City = "Haiku-Pauwela",
            ZipCode = ""
        };
        public static TopicInput TopicInput = new TopicInput
        {
            Id = "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
            Location = expectedLocation,
            IsShared = false
        };
        public static TopicInput TopicInputIsSharedTrue = new TopicInput
        {
            Id = "aaa085ef-96fb-4fd0-bcd0-0472ede66512",
            Location = expectedLocation,
            IsShared = true
        };
        public static Topic TopicIds = new Topic
        {
            Id = "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
            Name = "Family",
            OrganizationalUnit = "Alaska"
        };
        public static IEnumerable<object[]> TopicInputEnumerable()
        {
            yield return new object[] { new TopicInput { Id = Guid.Parse("addf41e9-1a27-4aeb-bcbb-7959f95094ba").ToString(), Location = expectedLocation, IsShared = false }, TopicInput };
            yield return new object[] { new TopicInput { Id = Guid.Parse("aaa085ef-96fb-4fd0-bcd0-0472ede66512").ToString(), Location = expectedLocation, IsShared = true }, TopicInputIsSharedTrue };
        }
        //public static IEnumerable<object[]> FormData()
        //{
        //    yield return new object[] { formData, expectedformData, propertyNames, values, TopicInput };
        //}
        public static IEnumerable<object[]> Topics()
        {
            yield return new object[] { new Topic { Id = Guid.Parse("addf41e9-1a27-4aeb-bcbb-7959f95094ba").ToString(), Name = "test", OrganizationalUnit = "Alaska" }, TopicIds };
        }
        public static string ResourceObjects = "{\"name\":\"Form1\",\"type\":\"form\",\"description\":\"Subhead lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem\",\"resourceType\":\"Forms\",\"externalUrl\":\"www.youtube.com\",\"url\":\"access2justice.com\",\"topicTags\":[{\"id\":\"aaa085ef-96fb-4fd0-bcd0-0472ede66512\"},{\"id\":\"2c0cc7b8-62b1-4efb-8568-b1f767f879bc\"}],\"organizationalUnit\":\"Alaska\",\"location\":[{\"state\":\"Hawaii\",\"city\":\"Haiku-Pauwela\"},{\"state\":\"Alaska\"}],\"overview\":\"Form1\",\"fullDescription\":\"Below is the form you will need if you are looking to settle your child custody dispute in court. We have included helpful tips to guide you along the way.\",\"createdBy\":\"API\",\"createdTimeStamp\":\"\",\"modifiedBy\":\"API\",\"modifiedTimeStamp\":\"\"},{\"name\":\"Form2\",\"type\":\"form\",\"description\":\"Subhead lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem\",\"resourceType\":\"Forms\",\"externalUrl\":\"\",\"url\":\"\",\"topicTags\":[{\"id\":\"aaa085ef-96fb-4fd0-bcd0-0472ede66512\"},{\"id\":\"2c0cc7b8-62b1-4efb-8568-b1f767f879bc\"}],\"organizationalUnit\":\"Alaska\",\"location\":[{\"state\":\"Hawaii\",\"city\":\"Haiku-Pauwela\"},{\"state\":\"Hawaii\"}],\"overview\":\"Form2\",\"fullDescription\":\"Below is the form you will need if you are looking to settle your child custody dispute in court. We have included helpful tips to guide you along the way.\",\"createdBy\":\"\",\"createdTimeStamp\":\"\",\"modifiedBy\":\"\",\"modifiedTimeStamp\":\"\"}";
        public static IEnumerable<object[]> LocationData()
        {
            yield return new object[] { locationDataHawaii };
            yield return new object[] { locationDataAlaska };
        }
        public static IEnumerable<object[]> TopicTagsData()
        {
            yield return new object[] { referencesTopicTags };
        }
        public static IEnumerable<object[]> ReferencesData()
        {
            yield return new object[] { referencesConditions, referencesConditionsExpected };
            yield return new object[] { referencesConditionsEmpty, referencesEmptyExpected };
            yield return new object[] { referencesTopicTags, referencesTopicTagsExpected };
            yield return new object[] { referencesTopicTagsEmpty, referencesEmptyExpected };
            yield return new object[] { referencesLocations, referencesLocationsExpected };
            yield return new object[] { referencesParentTopicIds, referencesParentTopicIdsExpected };
            yield return new object[] { referencesParentTopicIdsEmpty, referencesEmptyExpected };
            yield return new object[] { referencesReviewer, referencesReviewerExpected };
            yield return new object[] { referencesReviewerEmpty, referencesEmptyExpected };
            yield return new object[] { referenceContents, referenceContentsExpected };
            yield return new object[] { referenceContentsEmpty, referencesEmptyExpected };
        }
        public static JArray locationDataHawaii =
                    JArray.Parse(@"[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''}]");
        public static JArray locationDataAlaska =
                    JArray.Parse(@"[{'state':'Alaska','county':'','city':'','zipCode':''}]");
        public static JArray referencesConditions = JArray.Parse(@"[{'conditions': [{'condition':{ 'title':'condition','description':'Take to your partner to see if you can come to an agreement' }}]}]");
        public static JArray referencesConditionsExpected = JArray.Parse(@"[{ 'title':'condition','description':'Take to your partner to see if you can come to an agreement' }]");
        //public static JArray referencesConditions2 = JArray.Parse(@"[{'conditions': [{ 'title':'condition','description':'Take to your partner to see if you can come to an agreement' }]}]");
        public static JArray referencesConditionsEmpty = JArray.Parse(@"[{'conditions': []}]");
        public static JArray referencesEmptyExpected = JArray.Parse(@"[]");
        public static JArray referencesTopicTags = JArray.Parse(@"[{'topicTags': [{'id':'f102bfae-362d-4659-aaef-956c391f79de'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}]}]");
        public static JArray referencesTopicTagsExpected = JArray.Parse(@"[{'id':'f102bfae-362d-4659-aaef-956c391f79de'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}]");
        public static JArray referencesTopicTagsEmpty = JArray.Parse(@"[{'topicTags': []}]");
        public static JArray referencesLocations = JArray.Parse(@"[{'location': [{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''}]}]");
        public static JArray referencesLocationsExpected = JArray.Parse(@"[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''}]");
        public static JArray referencesParentTopicIds = JArray.Parse(@"[{'parentTopicId': [{'id':'f102bfae-362d-4659-aaef-956c391f79de'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}]}]");
        public static JArray referencesParentTopicIdsExpected = JArray.Parse(@"[{'id':'f102bfae-362d-4659-aaef-956c391f79de'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}]");
        public static JArray referencesParentTopicIdsEmpty = JArray.Parse(@"[{'parentTopicId': []}]");
        public static JArray referencesReviewer = JArray.Parse(@"[{'reviewer': [{'reviewerFullName':'reviewerFullName','reviewerTitle':'reviewerTitle','reviewText':'reviewText','reviewerImage':'reviewerImage'}]}]");
        public static JArray referencesReviewerExpected = JArray.Parse(@"[{'reviewerFullName':'reviewerFullName','reviewerTitle':'reviewerTitle','reviewText':'reviewText','reviewerImage':'reviewerImage'}]");
        public static JArray referencesReviewerEmpty = JArray.Parse(@"[{'reviewer': []}]");
        public static JArray referenceContents = JArray.Parse(@"[{'contents': [{'headline':'headline','content':'content'}]}]");
        public static JArray referenceContentsExpected = JArray.Parse(@"[{'headline':'headline','content':'content'}]");
        public static JArray referenceContentsEmpty = JArray.Parse(@"[{'contents': []}]");
        public static JArray topicTagsData = JArray.Parse(@"[{'id':'f102bfae-362d-4659-aaef-956c391f79de'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}]");

        public static IEnumerable<object[]> ResourceFilter()
        {
            yield return new object[] { new ResourceFilter { TopicIds = new List<string> { "addf41e9-1a27-4aeb-bcbb-7959f95094ba" }, PageNumber = 0, ResourceType = "ALL", Location = new Location(), ResourceIds = new List<string> { "addf41e9-1a27-4aeb-bcbb-7959f95094ba" } }, expectedTopicsEmpty };
        }
        public static string expectedTopicsEmpty = "{\"topics\\:[]\",\"resources\\:\"[]\"}";
        public static IEnumerable<object[]> ResourceData()
        {
            yield return new object[] { formData, expectedformData, "Forms" };
            yield return new object[] { formData, resourcesEmpty, "Forms" };
            yield return new object[] { actionPlanData, expectedActionPlanData, "Action Plans" };
            yield return new object[] { actionPlanData, resourcesEmpty, "Action Plans" };
            yield return new object[] { articleData, expectedArticleData, "Articles" };
            yield return new object[] { articleData, resourcesEmpty, "Articles" };
            yield return new object[] { videoData, expectedVideoData, "Videos" };
            yield return new object[] { videoData, resourcesEmpty, "Videos" };
            yield return new object[] { organizationData, expectedOrganizationData, "Organizations" };
            yield return new object[] { organizationData, resourcesEmpty, "Organizations" };
            yield return new object[] { essentialReadingData, expectedEssentialReadingData, "Essential Readings" };
            yield return new object[] { essentialReadingData, resourcesEmpty, "Essential Readings" };
            yield return new object[] { relatedLinkData, expectedRelatedLinkData, "Related Links" };
            yield return new object[] { relatedLinkData, resourcesEmpty, "Related Links" };
        }

    }
}
