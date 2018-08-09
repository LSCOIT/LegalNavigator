using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Api.Tests.TestData
{
    class ShareTestData
    {
        public static string id = "0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C";
        public static string oId = "709709e7t0r7t96";
        public static string resourceId = "5c035d27-2fdb-9776-6236-70983a918431";
        public static string permalinkInputData = "296497E";
        public static JArray ExpectedResourceData = JArray.Parse("[\"/topics/5c035d27-2fdb-9776-6236-70983a918431\"]");

        public static UserProfile UserProfileWithSharedResourceData =>

            new UserProfile
            {
                Id = Guid.NewGuid(),
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                IsActive = "true",
                OId = "709709e7t0r7t96",
                EMail = "test@test.com",
                SharedResource = new List<SharedResource>()
                    {
                        new SharedResource
                        {
                            IsShared = true,
                            ExpirationDate = DateTime.UtcNow,
                            PermaLink = "32803EFBBCA0D461EDA14F1BF56C5CA8C455AB6707F76A65026725F8DB7D3E8C",
                            Url = new Uri("/topics/5c035d27-2fdb-9776-6236-70983a918431", UriKind.Relative)
                        }
                    }
            };

        public static UserProfile UserProfileWithoutSharedResourceData =>
           new UserProfile
           {
               Id = Guid.Parse("0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C"),
               FirstName = "TestFirstName",
               LastName = "TestLastName",
               IsActive = "true",
               OId = "709709e7t0r7t96",
               EMail = "test@test.com"
           };

        public static IEnumerable<object[]> ShareInputData()
        {
            yield return new object[] { new ShareInput { ResourceId = Guid.Empty, Url = null, UserId = null }, null };
            yield return new object[] { new ShareInput { ResourceId = Guid.NewGuid(), Url = null, UserId = "709709e7t0r7t96" }, null };
            yield return new object[] { new ShareInput { ResourceId = Guid.Parse("5c035d27-2fdb-9776-6236-70983a918431"), Url = new Uri("/topics/5c035d27-2fdb-9776-6236-70983a918431", UriKind.Relative), UserId = "709709e7t0r7t96" }, new ShareViewModel() { PermaLink = "296497E" } };
        }

        public static ShareInput ShareInputSingleData =>
            new ShareInput
            {
                ResourceId = Guid.Parse("5c035d27-2fdb-9776-6236-70983a918431"),
                Url = new Uri("/topics/5c035d27-2fdb-9776-6236-70983a918431", UriKind.Relative),
                UserId = "709709e7t0r7t96"
            };



        public static Document document = new Document();

        public static string userProfile = "{\"id\": \"0693B88C-3866-4CCA-97C8-B8E3F3D1FF3C\"," +
                                                "\"oId\": \"709709e7t0r7t96\"," +
                                                "\"firstName\": \"TestFirstName\"," +
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
                                                  "\"createdBy\": \"TestUser\","+
                                                  "\"createdTimeStamp\": \"08/06/2018 09:02:50\","+
                                                  "\"modifiedBy\": null,"+
                                                  "\"modifiedTimeStamp\": null,"+
                                                  "\"sharedResource\": [{" +
                                                      "\"isShared\": true," +
                                                      "\"expirationDate\": \"2019-08-07T11:44:58.4928479Z\"," +
                                                      "\"permaLink\": \"296497EC2E240A852AA2D90250861115BBD2684806B3F1CE5109B6C0E58DCD5B\"," +
                                                      "\"url\": \"/topics/5c035d27-2fdb-9776-6236-70983a918431\"" +
                                                    "}" +
                                                  "]," +
                                                  "\"_rid\": \"ap9UAKM1MRMbAAAAAAAAAA==\"," +
                                                  "\"_self\": \"dbs/ap9UAA==/colls/ap9UAKM1MRM=/docs/ap9UAKM1MRMbAAAAAAAAAA==/\"," +
                                                  "\"_etag\": \"0000dc11-0000-0000-0000-5b6b2d3a0000\","+
                                                  "\"_attachments\": \"attachments/\","+
                                                  "\"_ts\": 1533750586 " +
                                                "}";
    }
}