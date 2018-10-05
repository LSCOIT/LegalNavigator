using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Api.Tests.TestData
{
    class UserRoleTestData
    {
        public static IEnumerable<object[]> GetPermissionData()
        {
            yield return new object[] {
                new UserProfile {
                    Id = Guid.Parse("e3583076-bed6-497b-847e-c732adf56925").ToString(),
                    OId ="userOId",
                    RoleInformationId = Guid.Parse("400d79ef-3895-4eef-b7c7-c59fcfb42afe")},
                JArray.Parse("[{\"id\": \"400d79ef-3895-4eef-b7c7-c59fcfb42afe\", \"type\": \"Role\"," +
                "\"roleName\": \"Authenticated\"," +
                "\"permissions\":[\"generatepermalink\",\"removepermalink\",\"getpermallinkresource\"]}]"),
                new List<string>(new string[] { "generatepermalink", "removepermalink", "getpermallinkresource" }) };
            yield return new object[] {
                new UserProfile {
                    Id = Guid.Parse("e3583076-bed6-497b-847e-c732adf56925").ToString(),
                    OId ="userOId",
                    RoleInformationId = Guid.Parse("400d79ef-3895-4eef-b7c7-c59fcfb42afe")},
                JArray.Parse("[{\"id\": \"400d79ef-3895-4eef-b7c7-c59fcfb42afe\", \"type\": \"Role\"," +
                "\"roleName\": \"Authenticated\"," +
                "\"permissions\":[]}]"),
                new List<string>() };
            yield return new object[] {
                new UserProfile {
                    Id = Guid.Parse("e3583076-bed6-497b-847e-c732adf56925").ToString(),
                    OId ="userOId",
                    RoleInformationId = Guid.Empty },
                JArray.Parse("[]"),
                new List<string>() };
        }

        public static IEnumerable<object[]> ValidateOrganizationalUnitData()
        {
            yield return new object[] { "Alaska" };
        }

        public static IEnumerable<object[]> ValidateOUForRole()
        {
            yield return new object[] { "400d79ef-3895-4eef-b7c7-c59fcfb42afe","Alaska | Hawaii",
                JArray.Parse("[{\"id\": \"400d79ef-3895-4eef-b7c7-c59fcfb42afe\", \"type\": \"Role\"," +
                "\"organizationalUnit\": \"Alaska\",\"roleName\": \"PortalAdmin\"}]"), true };
            yield return new object[] { "400d79ef-3895-4eef-b7c7-c59fcfb42afe","Alaska | Hawaii",
                JArray.Parse("[{\"id\": \"400d79ef-3895-4eef-b7c7-c59fcfb42afe\", \"type\": \"Role\"," +
                "\"organizationalUnit\": \"Hawaii\"}]"), true };
            yield return new object[] { "400d79ef-3895-4eef-b7c7-c59fcfb42afe","Alaska",
                JArray.Parse("[{\"id\": \"400d79ef-3895-4eef-b7c7-c59fcfb42afe\", \"type\": \"Role\"," +
                "\"organizationalUnit\": \"Hawaii\"}]"), false };
            yield return new object[] { "400d79ef-3895-4eef-b7c7-c59fcfb42afe","Alaska",
                JArray.Parse("[{\"id\": \"400d79ef-3895-4eef-b7c7-c59fcfb42afe\", \"type\": \"Role\"," +
                "\"organizationalUnit\": \"\"}]"), false };
            yield return new object[] { "400d79ef-3895-4eef-b7c7-c59fcfb42afe",string.Empty,
                JArray.Parse("[]"), false };
        }

    }
}
