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
                   'ParentTopicId':'','keywords':'eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao',
                    'zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':
                   'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'',
                   'icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'
                   ','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/',
                    '_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257}]");
        public static JArray resourcesData =
                    JArray.Parse(@"[{'id':'77d301e7-6df2-612e-4704-c04edf271806','name':'Tenant Action Plan 
                    for Eviction','description':'This action plan is for tenants who are facing eviction and have experienced the following:',
                    'resourceType':'Action','externalUrl':'','url':'','topicTags':[{'id':'f102bfae-362d-4659-aaef-956c391f79de'},
                    {'id':'2c0cc7b8-62b1-4efb-8568-b1f767f879bc'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}],'location':[{'state':'Hawaii',
                    'city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Alaska'}],'icon':'./assets/images/resources/resource.png',
                    'createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSAJdNlwIBAAAAAAAAAA==',
                    '_self':'dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIBAAAAAAAAAA==/','_etag':'\'040007b5-0000-0000-0000-5b0792260000\'',
                    '_attachments':'attachments/','_ts':1527222822},{'id':'19a02209-ca38-4b74-bd67-6ea941d41518','name':'Legal Help Organization',
                    'description':'This action plan is for tenants who are facing eviction and have experienced the following:','resourceType':'Organization'
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
        public static JArray formData =
                    JArray.Parse(@"[{'overview': 'Form1','fullDescription': 'Below is the form you will need if you are looking to settle your child custody dispute in court. We have included helpful tips to guide you along the way.',
                    'id':'77d301e7-6df2-612e-4704-c04edf271806','name': 'Form1','description': 'Subhead lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem',
                    'resourceType': 'Forms','externalUrl': 'www.youtube.com','url': 'access2justice.com','referenceTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],
                    'location': [{'state': 'Hawaii','county':'','city': 'Haiku-Pauwela','zipCode':''}],'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp': '','modifiedBy': 'API','modifiedTimeStamp': ''}]");
        public static JArray actionPlanData =
                    JArray.Parse(@"[{'conditions': [{'condition': {'title': 'Take to your partner to see if you can come to an agreement', 'description': 'Why you should do this dolor sit amet.'}}],
                    'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Action Plan','description': 'This action plan is for tenants who are facing eviction and have experienced the following:',
                    'resourceType': 'Action Plans','externalUrl': 'www.youtube.com','url': 'access2justice.com','referenceTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray referencesInputData =
                    JArray.Parse(@"[{'conditions': [{'condition': {'title': 'Take to your partner to see if you can come to an agreement', 'description': 'Why you should do this dolor sit amet.'}}],
                    'parentTopicId': [{'id': '349fa67b-164f-4a65-bb5d-a5b3dd2640a6'}],'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Action Plan','description': 'This action plan is for tenants who are facing eviction and have experienced the following:',
                    'resourceType': 'Action Plans','externalUrl': 'www.youtube.com','url': 'access2justice.com','referenceTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray articleData =
                    JArray.Parse(@"[{'overview': 'Overview','headline1': 'HL1','headline2': 'HL2','headline3': 'HL3','id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Article1','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Articles','externalUrl': 'www.youtube.com','url': 'access2justice.com','referenceTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray videoData =
                    JArray.Parse(@"[{'overview': 'Overview','isRecommended': 'Yes','videoUrl': 'Url','id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Video','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Videos','externalUrl': 'www.youtube.com','url': 'access2justice.com','referenceTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray organizationData =
                    JArray.Parse(@"[{'overview': 'Overview','subService': 'Law Professional','street': '123 Street','city': 'Honululu','state': 'Hawaii','zipCode': '70085','telephone': '67586758768','eligibilityInformation': 'eligibility','reviewedByCommunityMember': 'Community member',
                    'reviewerFullName': 'John Smith','reviewerTitle': 'Lawyer','reviewerImage': 'ReviewerImage.jpeg','id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Organization','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Organizations','externalUrl': 'www.youtube.com','url': 'access2justice.com','referenceTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray essentialReadingData =
                    JArray.Parse(@"[{'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Essential Reading in Hawaii','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Essential Readings','externalUrl': 'www.youtube.com','url': 'access2justice.com','referenceTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray topicData =
                    JArray.Parse(@"[{'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Family','parentTopicId': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'keywords': 'HOUSING',
                    'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'jsonContent':'jsonContent','icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray referenceTagData =
                     JArray.Parse(@"[{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}]");
        public static JArray parentTopicIdData =
                     JArray.Parse(@"[{'id': '349fa67b-164f-4a65-bb5d-a5b3dd2640a6'}]");
        public static JArray locationData =
                     JArray.Parse(@"[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''},{'state':'Alaska','county':'','city':'','zipCode':''}]");
        public static JArray conditionData =
                     JArray.Parse(@"[{'condition': {'title': 'Take to your partner to see if you can come to an agreement','description': 'Why you should do this dolor sit amet'}}]");
        public static JArray emptyResourceData = JArray.Parse(@"[{'referenceTags':[{'id': ''}],'location': [{'state': '','county':'','': '','zipCode':''}],'conditions': [{'condition': {'title': '','description': ''}}],'parentTopicId':[{'id':''}] }]");
        //Mcoked Data
       public static JArray emptyLocationData =
                     JArray.Parse(@"[{'state':'','county':'','city':'','zipCode':''}]");
       public static JArray emptyConditionObject =
                     JArray.Parse(@"[{'condition':[]}]");
       public static JArray EmptyReferences =
                     JArray.Parse(@"[[{'id':''}],[{'state':'','county':'','city':'','zipCode':''}],[{'condition':[{'title':'','description':''}]}],[{'id':''}]]");
       public static string expectedTopicId = "addf41e9-1a27-4aeb-bcbb-7959f95094ba";
       public static string expectedResourceId = "77d301e7-6df2-612e-4704-c04edf271806";
        public static Location expectedLocationValue= new Location() { State="Hawaii",County="",City= "Honolulu", ZipCode= "96741" };
        public static string expectedpagedResource = "{\"ContinuationToken\":\"[]\",\"Results\":[],\"TopicIds\":[]}";
       public static string expectedResourceCount = "{\"ResourceName\":\"Organizations\",\"ResourceCount\":4}";
       public static string expectedEmptyResourceCount = "{\"ResourceName\":\"All\",\"ResourceCount\":0}";
        public static JArray expectedformData =
                     JArray.Parse(@"[{'overview': 'Form1','fullDescription': 'Below is the form you will need if you are looking to settle your child custody dispute in court. We have included helpful tips to guide you along the way.',
                    'id':'77d301e7-6df2-612e-4704-c04edf271806','name': 'Form1','description': 'Subhead lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem',
                    'resourceType': 'Forms','externalUrl': 'www.youtube.com','url': 'access2justice.com','referenceTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],
                    'location': [{'state': 'Hawaii','county':'','city': 'Haiku-Pauwela','zipCode':''}],'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp': '','modifiedBy': 'API','modifiedTimeStamp': ''}]");
        public static JArray expectedActionPlanData =
                    JArray.Parse(@"[{'conditions': [{'condition': [{'title': 'Take to your partner to see if you can come to an agreement', 'description': 'Why you should do this dolor sit amet.'}]}],
                    'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Action Plan','description': 'This action plan is for tenants who are facing eviction and have experienced the following:',
                    'resourceType': 'Action Plans','externalUrl': 'www.youtube.com','url': 'access2justice.com','referenceTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray expectedArticleData =
                    JArray.Parse(@"[{'overview': 'Overview','headline1': 'HL1','headline2': 'HL2','headline3': 'HL3','id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Article1','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Articles','externalUrl': 'www.youtube.com','url': 'access2justice.com','referenceTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray expectedVideoData =
                    JArray.Parse(@"[{'overview': 'Overview','isRecommended': 'Yes','videoUrl': 'Url','id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Video','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Videos','externalUrl': 'www.youtube.com','url': 'access2justice.com','referenceTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray expectedOrganizationData =
                    JArray.Parse(@"[{'overview': 'Overview','subService': 'Law Professional','street': '123 Street','city': 'Honululu','state': 'Hawaii','zipCode': '70085','telephone': '67586758768','eligibilityInformation': 'eligibility','reviewedByCommunityMember': 'Community member',
                    'reviewerFullName': 'John Smith','reviewerTitle': 'Lawyer','reviewerImage': 'ReviewerImage.jpeg','id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Organization','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Organizations','externalUrl': 'www.youtube.com','url': 'access2justice.com','referenceTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray expectedEssentialReadingData =
                    JArray.Parse(@"[{'id': '807f2e0d-c431-4f1c-b8c8-1223e6750bec','name': 'Essential Reading in Hawaii','description': 'Subhead lorem ipsum solor sit amet bibodem',
                    'resourceType': 'Essential Readings','externalUrl': 'www.youtube.com','url': 'access2justice.com','referenceTags': [{'id': 'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'location': [{'state': 'Hawaii','county': '','city': 'Haiku-Pauwela','zipCode': ''},{'state': 'Alaska','county': '','city': '','zipCode': ''}],
                    'icon': './assets/images/resources/resource.png','createdBy': 'API','createdTimeStamp':'','modifiedBy': 'API','modifiedTimeStamp':''}]");
        public static JArray expectedTopicData =
                    JArray.Parse(@"[{'id':'807f2e0d-c431-4f1c-b8c8-1223e6750bec','name':'Family','parentTopicId':[{'id':'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'keywords':'HOUSING','jsonContent':'jsonContent','location':[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''},{'state':'Alaska','county':'','city':'','zipCode':''}],'icon':'./assets/images/resources/resource.png','createdBy':'API','createdTimeStamp':'','modifiedBy':'API','modifiedTimeStamp':''}]");
        public static JArray expectedTopicsData =
                    JArray.Parse(@"[{'id':'807f2e0d-c431-4f1c-b8c8-1223e6750bec','name':'Family','parentTopicId':[{'id':'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],'keywords':'HOUSING','location':[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''},{'state':'Alaska','county':'','city':'','zipCode':''}],'jsonContent':'jsonContent','icon':'./assets/images/resources/resource.png','createdBy':'API','createdTimeStamp':'','modifiedBy':'API','modifiedTimeStamp':''}]");
        public static string expectedReferenceTagData = "aaa085ef-96fb-4fd0-bcd0-0472ede66512";
        public static string expectedParentTopicIdData = "349fa67b-164f-4a65-bb5d-a5b3dd2640a6";
        public static JArray expectedLocationData =
                     JArray.Parse(@"[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''},{'state':'Alaska','county':'','city':'','zipCode':''}]");
        public static JArray expectedReferenceLocationData =
                     JArray.Parse(@"[{'state': 'Hawaii','county':'','city': 'Haiku-Pauwela','zipCode':''}]");
        public static JArray expectedConditionData =
                     JArray.Parse(@"[{'condition':[{'title':'Take to your partner to see if you can come to an agreement', 'description':'Why you should do this dolor sit amet'}]}]");
        public static JArray expectedResourceReferences =
                     JArray.Parse(@"[[{'id':'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''},{'state':'Alaska','county':'','city':'','zipCode':''}]]");
        public static JArray expectedActionPlanReferences =
                     JArray.Parse(@"[[{'id':'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''},{'state':'Alaska','county':'','city':'','zipCode':''}],[{'condition':[{'title':'Take to your partner to see if you can come to an agreement','description':'Why you should do this dolor sit amet.'}]}]]");
        public static JArray expectedReferencesData =
                     JArray.Parse(@"[[{'id':'aaa085ef-96fb-4fd0-bcd0-0472ede66512'}],[{'state':'Hawaii','county':'','city':'Haiku-Pauwela','zipCode':''},{'state':'Alaska','county':'','city':'','zipCode':''}],[{'condition':[{'title':'Take to your partner to see if you can come to an agreement','description':'Why you should do this dolor sit amet.'}]}],[{'id':'349fa67b-164f-4a65-bb5d-a5b3dd2640a6'}]]");
    }
}
