﻿using Access2Justice.Api.ViewModels;
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
                SharedResource = new List<SharedResource>()
                    {
                        new SharedResource
                        {
                            IsShared = true,
                            ExpirationDate = DateTime.UtcNow.AddYears(1),
                            PermaLink = "32803EFBBCA0D461EDA14F1BYTAC5CA8C455AB6707F89065026725F8DB7D3E8C",
                            Url = new Uri("/topics/5c035d27-2fdb-9776-6236-70983a918431", UriKind.RelativeOrAbsolute)
                        }
                    }
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

        public static IEnumerable<object[]> ShareInputData()
        {
            yield return new object[] { new ShareInput { ResourceId = Guid.Empty, Url = null, UserId = null }, null };
            yield return new object[] { new ShareInput { ResourceId = Guid.NewGuid(), Url = null, UserId = "709709e7t0r7t96" }, null };
            yield return new object[] { new ShareInput { ResourceId = Guid.Parse("5c035d27-2fdb-9776-6236-70983a918431"), Url = new Uri("/topics/5c035d27-2fdb-9776-6236-70983a918431", UriKind.RelativeOrAbsolute), UserId = "709709e7t0r7t96" }, new ShareViewModel() { PermaLink = "32803EF" } };
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
                JArray.Parse("[]"),
                null
            };
            yield return new object[] {
                "32803EF",
                JArray.Parse("[]"),
                new ShareProfileViewModel { UserName = null, UserId=null, ResourceLink= null }
            };
            yield return new object[] {
                "32803EF",
                null,
                null
            };
            yield return new object[] {
                "32803EF",
                JArray.Parse("[{\"firstName\":\"TestFirstName\",\"lastName\":\"TestLastName\",\"oId\":\"709709e7t0r7t96\",\"url\":\"/profile\"}]"),
                new ShareProfileViewModel { UserName = "TestFirstName TestLastName", UserId="709709e7t0r7t96", ResourceLink= "/profile" }
            };
            yield return new object[] {
                "32803EF",
                JArray.Parse("[{\"firstName\":\"TestFirstName\",\"lastName\":\"TestLastName\",\"oId\":\"709709e7t0r7t96\",\"url\":\"/topics/5c035d27-2fdb-9776-6236-70983a918431\"}]"),
                new ShareProfileViewModel { UserName = null, UserId=null, ResourceLink= "/topics/5c035d27-2fdb-9776-6236-70983a918431" }
            };
            yield return new object[] {
                "32803EF",
                JArray.Parse("[{\"firstName\":\"TestFirstName\",\"lastName\":\"TestLastName\",\"oId\":\"709709e7t0r7t96\",\"url\":\"https://www.microsoft.com\"}]"),
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
        }

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
                                                  "\"createdBy\": \"TestUser\"," +
                                                  "\"createdTimeStamp\": \"08/06/2018 09:02:50\"," +
                                                  "\"modifiedBy\": null," +
                                                  "\"modifiedTimeStamp\": null," +
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
    }
}