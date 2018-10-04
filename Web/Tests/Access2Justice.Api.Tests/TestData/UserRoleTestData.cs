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
            yield return new object[] { "400d79ef-3895-4eef-b7c7-c59fcfb42afe",
				JArray.Parse(@"[{'id': 'e3583076-bed6-497b-847e-c732adf56925','roleInformationId':'400d79ef-3895-4eef-b7c7-c59fcfb42afe'}]"),
				new UserRole {RoleInformationId=Guid.Parse("400d79ef-3895-4eef-b7c7-c59fcfb42afe"), RoleName="PortalAdmin", Permissions = { "upserttopics","upsertresources","upsertstaticcontent"} } };
            yield return new object[] { "0f4755fc-d9a0-4357-a4fa-ccb86654f673", new UserRole { RoleInformationId = Guid.Parse("0f4755fc-d9a0-4357-a4fa-ccb86654f673"), RoleName = "StateAdmin", Permissions = { "upserttopic", "upsertresource", "generatepermalink" } } };
            yield return new object[] { "0f4755fc-d9a0-4357-a4fa-ccb86654f673", null };
        }
    }
}
