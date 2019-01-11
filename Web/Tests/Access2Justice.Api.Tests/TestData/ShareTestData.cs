using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Access2Justice.Api.Tests.TestData
{
    class ShareTestData
    {
        public static UserProfile UserProfileWithSharedResourceData =>

            new UserProfile
            {
                Id = Guid.Parse("0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C").ToString(),
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                IsActive = "true",
                OId = "709709e7t0r7t96",
                EMail = "test@test.com",
                SharedResourceId = Guid.Parse("0568B88C-3866-4CCA-97C8-B8E3F3D1FF3C")
            };
        public static UserProfile UserProfileWithPlanId =>

            new UserProfile
            {
                Id = Guid.Parse("0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C").ToString(),
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                IsActive = "true",
                OId = "709709e7t0r7t96",
                EMail = "test@test.com",
                SharedResourceId = Guid.Parse("0568B88C-3866-4CCA-97C8-B8E3F3D1FF3C"),
                PersonalizedActionPlanId =Guid.Parse("132d8f82-96df-4d9b-8023-4332a1924da4")
            };

        public static UserProfile UserProfileWithoutPlanId =>

           new UserProfile
           {
               Id = Guid.Parse("0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C").ToString(),
               FirstName = "TestFirstName",
               LastName = "TestLastName",
               IsActive = "true",
               OId = "709709e7t0r7t96",
               EMail = "test@test.com",
               SharedResourceId = Guid.Parse("0568B88C-3866-4CCA-97C8-B8E3F3D1FF3C")               
           };

        public static UserProfile UserProfileWithoutSharedResourceData =>
           new UserProfile
           {
               Id = Guid.Parse("0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C").ToString(),
               FirstName = "TestFirstName",
               LastName = "TestLastName",
               IsActive = "true",
               OId = "709709e7t0r7t96",
               EMail = "test@test.com"
           };

        public static IEnumerable<object[]> UserProfileWithSharedResourceDataForUpdate()
        {
            yield return new object[] { new UserProfile { Id = Guid.Parse("0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C").ToString(), FirstName = "TestFirstName", LastName = "TestLastName", IsActive = "true", OId = "709709e7t0r7t96", EMail = "test@test.com",SharedResourceId = Guid.Parse("0568B88C-3866-4CCA-97C8-B8E3F3D1FF3C") },
                new SharedResource { IsShared = true, PermaLink = "32803EFB57602FF080B32575F327410A41A3346A2DC12809EBE7B2E3F307BCF7", Url= new Uri("/topics/5c035d27-2fdb-9776-6236-70983a918431", UriKind.RelativeOrAbsolute)},
                                sharedResourcesData, "{\"id\":\"0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C\",\"_rid\":\"ap9UAKM1MRMbAAAAAAAAAA==\",\"_self\":\"dbs/ap9UAA==/colls/ap9UAKM1MRM=/docs/ap9UAKM1MRMbAAAAAAAAAA==/\",\"_ts\":1533750586,\"_etag\":\"0000dc11-0000-0000-0000-5b6b2d3a0000\",\"oId\":\"709709e7t0r7t96\",\"firstName\":\"TestFirstName\",\"lastName\":\"TestLastName\",\"eMail\":\"test@hotmail.com\",\"isActive\":\"Yes\",\"createdBy\":\"TestUser\",\"createdTimeStamp\":\"08/06/2018 09:02:50\",\"modifiedBy\":null,\"modifiedTimeStamp\":null,\"sharedResource\":[{\"isShared\":true,\"expirationDate\":\"2019-08-07T11:44:58.4928479Z\",\"permaLink\":\"32803EFBBCA0D461EDA14F1BF56C5CA8C455AB6707F76A65026725F8DB7D3E8C\",\"url\":\"/topics/5c035d27-2fdb-9776-6236-70983a918431\"}]}"
            };
            yield return new object[] { new UserProfile { Id = Guid.Parse("0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C").ToString(), FirstName = "TestFirstName", LastName = "TestLastName", IsActive = "true", OId = "709709e7t0r7t96", EMail = "test@test.com" },
                new SharedResource { IsShared = true, PermaLink = "32803EFB57602FF080B32575F327410A41A3346A2DC12809EBE7B2E3F307BCF7", Url= new Uri("/topics/5c035d27-2fdb-9776-6236-70983a918431", UriKind.RelativeOrAbsolute)},
                                sharedResourcesData, "{\"id\":\"0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C\",\"_rid\":\"ap9UAKM1MRMbAAAAAAAAAA==\",\"_self\":\"dbs/ap9UAA==/colls/ap9UAKM1MRM=/docs/ap9UAKM1MRMbAAAAAAAAAA==/\",\"_ts\":1533750586,\"_etag\":\"0000dc11-0000-0000-0000-5b6b2d3a0000\",\"oId\":\"709709e7t0r7t96\",\"firstName\":\"TestFirstName\",\"lastName\":\"TestLastName\",\"eMail\":\"test@hotmail.com\",\"isActive\":\"Yes\",\"createdBy\":\"TestUser\",\"createdTimeStamp\":\"08/06/2018 09:02:50\",\"modifiedBy\":null,\"modifiedTimeStamp\":null,\"sharedResource\":[{\"isShared\":true,\"expirationDate\":\"2019-08-07T11:44:58.4928479Z\",\"permaLink\":\"32803EFBBCA0D461EDA14F1BF56C5CA8C455AB6707F76A65026725F8DB7D3E8C\",\"url\":\"/topics/5c035d27-2fdb-9776-6236-70983a918431\"}]}"
            };
        }

        public static IEnumerable<object[]> ShareInputData()
        {
            yield return new object[] { new ShareInput { ResourceId = Guid.Empty, Url = null, UserId = null }, null };
            yield return new object[] { new ShareInput { ResourceId = Guid.NewGuid(), Url = null, UserId = "709709e7t0r7t96" }, null };
            yield return new object[] { new ShareInput { ResourceId = Guid.Parse("5c035d27-2fdb-9776-6236-70983a918431"), Url = new Uri("/topics/5c035d27-2fdb-9776-6236-70983a918431", UriKind.RelativeOrAbsolute), UserId = "709709e7t0r7t96" }, new ShareViewModel() { PermaLink = "32803EF" } };
            yield return new object[] { new ShareInput { ResourceId = Guid.Parse("5c035d27-2fdb-9776-6236-70983a918431"), Url = new Uri("/topics/4h635d27-2fdb-9776-6236-70983a918431", UriKind.RelativeOrAbsolute), UserId = "709709e7t0r7t96" }, null };
        }

        public static IEnumerable<object[]> UnShareInputData()
        {
            yield return new object[] { new ShareInput { ResourceId = Guid.Empty, UserId = null }, null };
            yield return new object[] { new ShareInput { ResourceId = Guid.NewGuid(), Url = null, UserId = "709709e7t0r7t96" }, null };
            yield return new object[] { new ShareInput { ResourceId = Guid.Parse("5c035d27-2fdb-9776-6236-70983a918431"), Url = new Uri("/topics/5c035d27-2fdb-9776-6236-70983a918431", UriKind.RelativeOrAbsolute), UserId = "709709e7t0r7t96" }, true };
            yield return new object[] { new ShareInput { ResourceId = Guid.Parse("0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C"), Url = new Uri("/topics/5c035d27-2fdb-9776-6236-70983a918431", UriKind.RelativeOrAbsolute), UserId = "709709e7t0r7t96" }, true };
        }

        public static IEnumerable<object[]> ShareProfileResponseData()
        {
            yield return new object[] {
                "",
                "",
                JArray.Parse("[]"),
                JArray.Parse("[]"),
                null
            };
            yield return new object[] {
                "32803EF",
                "",
                JArray.Parse("[]"),
                JArray.Parse("[]"),
                new ShareProfileViewModel { UserName = null, UserId=null, ResourceLink= null }
            };
            yield return new object[] {
                "32803EF",
                "",
                null,
                null,
                null
            };
            yield return new object[] {
                "32803EF",
                "76087dfd-977b-48bb-bbf4-766fad5c639b",
                JArray.Parse("[{\"id\":\"76087dfd-977b-48bb-bbf4-766fad5c639b\",\"url\":\"/profile\"}]"),
                JArray.Parse("[{\"name\":\"TestFirstName TestLastName\",\"oId\":\"709709e7t0r7t96\",\"url\":\"/profile\"}]"),
                new ShareProfileViewModel { UserName = "TestFirstName TestLastName", UserId="709709e7t0r7t96", ResourceLink= "/profile" }
            };
            yield return new object[] {
                "32803EF",
                "76087dfd-977b-48bb-bbf4-766fad5c639b",
                JArray.Parse("[{\"id\":\"76087dfd-977b-48bb-bbf4-766fad5c639b\",\"url\":\"/topics/5c035d27-2fdb-9776-6236-70983a918431\"}]"),
                JArray.Parse("[{\"name\":\"TestFirstName\",\"oId\":\"709709e7t0r7t96\",\"url\":\"/topics/5c035d27-2fdb-9776-6236-70983a918431\"}]"),
                new ShareProfileViewModel { UserName = null, UserId=null, ResourceLink= "/topics/5c035d27-2fdb-9776-6236-70983a918431" }
            };
            yield return new object[] {
                "32803EF",
                "76087dfd-977b-48bb-bbf4-766fad5c639b",
                JArray.Parse("[{\"id\":\"76087dfd-977b-48bb-bbf4-766fad5c639b\",\"url\":\"https://www.microsoft.com\"}]"),
                JArray.Parse("[{\"name\":\"TestFirstName\",\"oId\":\"709709e7t0r7t96\",\"url\":\"https://www.microsoft.com\"}]"),
                new ShareProfileViewModel { UserName = null, UserId=null, ResourceLink= "https://www.microsoft.com" }
            };
        }

        public static IEnumerable<object[]> ShareGenerateInputData()
        {
            yield return new object[] { new ShareInput { ResourceId = Guid.Parse("5c035d27-2fdb-9776-6236-70983a918431"),
                Url = new Uri("/topics/5c035d27-2fdb-9776-6236-70983a918431", UriKind.Relative),
                UserId = "709709e7t0r7t96",
                UniqueId = Guid.Parse("6D4826FD-24BB-41BA-9AD6-39AF7737C335")
            },
            0,//permalink Length param
            new ShareViewModel() { PermaLink = "32803EFBBCA0D461EDA14F1BYTAC5CA8C455AB6707F89065026725F8DB7D3E8C" } };

            yield return new object[] { new ShareInput { ResourceId = Guid.Parse("5c035d27-2fdb-9776-6236-70983a918431"),
                Url = new Uri("/topics/5c035d27-2fdb-9776-6236-70983a918431", UriKind.Relative),
                UserId = "709709e7t0r7t96",
                UniqueId = Guid.Parse("6D4826FD-24BB-41BA-9AD6-39AF7737C335")
            },
            7,//permalink Length param
            new ShareViewModel() { PermaLink = "32803EF" } };

            yield return new object[] { new ShareInput { ResourceId = Guid.Parse("5c035d27-2fdb-9776-6236-70983a918431"),
                Url = new Uri("/plan/5c035d27-2fdb-9776-6236-70983a918431", UriKind.Relative),
                UserId = "709709e7t0r7t96",
                UniqueId = Guid.Parse("6D4826FD-24BB-41BA-9AD6-39AF7737C335")
            },
            7,//permalink Length param
            new ShareViewModel() { PermaLink = "32803EF" } };

        }

        public static IEnumerable<object[]> ShareGenerateInputDataNull()
        {
            yield return new object[] { new ShareInput { ResourceId = Guid.Parse("5c035d27-2fdb-9776-6236-70983a918431"),
                Url = new Uri("/topics/5c035d27-2fdb-9776-6236-70983a918431", UriKind.Relative),
                UserId = null,
                UniqueId = Guid.Parse("6D4826FD-24BB-41BA-9AD6-39AF7737C335")
            },
            7,//permalink Length param
            new ShareViewModel() { PermaLink = null } };
        }
        public static IEnumerable<object[]> UpdatePersonalizedPlanData()
        {
            yield return new object[] { "132d8f82-96df-4d9b-8023-4332a1924da4", true, true };
            yield return new object[] { "132d8f82-96df-4d9b-8023-4332a1924da4", false, false };
        }

        public static string userProfile = "{\"id\": \"0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C\"," +
                                                "\"topics\": \"5c035d27-2fdb-9776-6236-70983a918431\"," +
                                                "\"isShared\": \"TestFirstName\"," +
                                                  "\"lastName\": \"TestLastName\"," +
                                                  "\"eMail\": \"test@hotmail.com\"," +
                                                  "\"isActive\": \"Yes\"," +
                                                "}";

        public static string userProfileWithSharedResource = "{\"id\": \"0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C\"," +
                                                "\"oId\": \"709709e7t0r7t96\"," +
                                                "\"firstName\": \"TestFirstName\"," +
                                                  "\"lastName\": \"TestLastName\"," +
                                                  "\"eMail\": \"test@hotmail.com\"," +
                                                  "\"isActive\": \"Yes\"," +
                                                  "\"createdBy\": \"TestUser\"," +
                                                  "\"createdTimeStamp\": \"08/06/2018 09:02:50\"," +
                                                  "\"modifiedBy\": null," +
                                                  "\"modifiedTimeStamp\": null," +
                                                  "\"personalizedActionPlanId\": \"132d8f82-96df-4d9b-8023-4332a1924da4\"," +
                                                  "\"sharedResource\": [{" +
                                                      "\"isShared\": true," +
                                                      "\"expirationDate\": \"2019-08-07T11:44:58.4928479Z\"," +
                                                      "\"permaLink\": \"32803EFBBCA0D461EDA14F1BF56C5CA8C455AB6707F76A65026725F8DB7D3E8C\"," +
                                                      "\"url\": \"/topics/5c035d27-2fdb-9776-6236-70983a918431\"" +
                                                    "}" +
                                                  "]," +
                                                  "\"_rid\": \"ap9UAKM1MRMbAAAAAAAAAA==\"," +
                                                  "\"_self\": \"dbs/ap9UAA==/colls/ap9UAKM1MRM=/docs/ap9UAKM1MRMbAAAAAAAAAA==/\"," +
                                                  "\"_etag\": \"0000dc11-0000-0000-0000-5b6b2d3a0000\"," +
                                                  "\"_attachments\": \"attachments/\"," +
                                                  "\"_ts\": 1533750586 " +
                                                "}";
        public static string updatedSharedResourcesData = "{\"id\":\"e3583076-bed6-497b-847e-c732adf56925\"," +
            "\"sharedResources\":[{\"isShared\":true," +
             "\"expirationDate \": \"2019-08-30T11:00:00.6731861Z\"," +
            "\"permaLink\":\"8427C11032F448084C0E87408E51615701AD539389BD4923C21FA410EECD43A3\"," +
            "\"url\":\"/topics/363909be-bb87-4dba-9743-7f3849f0b80d\"}]," +
            "\"_rid\":\"ap9UAJ3+yXQNAAAAAAAAAA==\"," +
            "\"_self\": \"dbs/ap9UAA==/colls/ap9UAKM1MRM=/docs/ap9UAKM1MRMbAAAAAAAAAA==/\"," +
                                                  "\"_etag\": \"0000dc11-0000-0000-0000-5b6b2d3a0000\"," +
                                                  "\"_attachments\": \"attachments/\"," +
                                                  "\"_ts\": 1533750586 " + "}";

        public static JArray sharedResourcesData =
                            JArray.Parse(@"[{
    'id': 'e3583076-bed6-497b-847e-c732adf56925',
    'sharedResources': [
      {
        'isShared': true,
        'expirationDate': '2019-08-30T12:25:34.0461574Z',
        'permaLink': '32803EFB57602FF080B32575F327410A41A3346A2DC12809EBE7B2E3F307BCF7',
        'url': '/topics/5c035d27-2fdb-9776-6236-70983a918431'
      }
    ],
    '_rid': 'ap9UAJ3+yXQNAAAAAAAAAA==',
    '_self': 'dbs/ap9UAA==/colls/ap9UAJ3+yXQ=/docs/ap9UAJ3+yXQNAAAAAAAAAA==/',
    '_etag': '\'9200fec6-0000-0000-0000-5b87e4020000\'',
    '_attachments': 'attachments/',
    '_ts': 1535632386
  }]");

        public static string upsertSharedResource = "{\"id\": \"0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C\"," +
                                " \"oId\": \"709709e7t0r7t96\"," +
                                " \"firstName\": \"TestFirstName\"," +
                                " \"lastName\": \"TestLastName\"," +
                                " \"eMail\": \"test@hotmail.com\"," +
                                " \"isActive\": \"Yes\"," +
                                " \"createdBy\": \"TestUser\"," +
                                " \"createdTimeStamp\": \"08/06/2018 09:02:50\"," +
                                "  \"modifiedBy\": null," +
                                "  \"modifiedTimeStamp\": null," +
                                "  \"sharedResource\": [" +
                                "    {" +
                                "      \"isShared\": true," +
                                "      \"expirationDate\": \"2019-08-07T11:44:58.4928479Z\"," +
                                "      \"permaLink\": \"32803EFBBCA0D461EDA14F1BF56C5CA8C455AB6707F76A65026725F8DB7D3E8C\"," +
                                "      \"url\": \"/topics/5c035d27-2fdb-9776-6236-70983a918431\"" +
                                "    }" +
                                "  ]," +
                                "  \"_rid\": \"ap9UAKM1MRMbAAAAAAAAAA==\"," +
            "  \"_self\": \"dbs/ap9UAA==/colls/ap9UAKM1MRM=/docs/ap9UAKM1MRMbAAAAAAAAAA==/\"," +
            "  \"_etag\": \"0000dc11-0000-0000-0000-5b6b2d3a0000\"," +
            "  \"_attachments\": \"attachments/\"," +
            "  \"_ts\": 1533750586" +
            "}";
    }
}