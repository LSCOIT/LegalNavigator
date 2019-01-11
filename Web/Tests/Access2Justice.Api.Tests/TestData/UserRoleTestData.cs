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
                    RoleInformationId = new List<Guid>{new Guid("4bf9df8f-dfee-4b08-be4d-35cc053fa298")} },
                JArray.Parse(@"[{
                                    'id': '4bf9df8f-dfee-4b08-be4d-35cc053fa298',
                                    'roleName': 'StateAdmin',
                                    'type': 'Role',
                                    'organizationalUnit': 'Alaska',
                                    'permissions': [ 'upsertstatichomepage', 'upsertstaticprivacypage', 'upsertstatichelpandfaqpage', 'upsertstaticnavigation']
                                    }]"),
                new List<string>(new string[] { "upsertstatichomepage", "upsertstaticprivacypage", "upsertstatichelpandfaqpage","upsertstaticnavigation" }) };

            yield return new object[] {
                new UserProfile {
                    Id = Guid.Parse("e3583076-bed6-497b-847e-c732adf56925").ToString(),
                    OId ="userOId",
                    RoleInformationId = new List<Guid>{new Guid("4bf9df8f-dfee-4b08-be4d-35cc053fa298"),
                                                       new Guid("b4f63784-fe99-4774-ad28-ce7911986bea")} },
                JArray.Parse(@"[{
                                    'id': '4bf9df8f-dfee-4b08-be4d-35cc053fa298',
                                    'roleName': 'StateAdmin',
                                    'type': 'Role',
                                    'organizationalUnit': 'Alaska',
                                    'permissions': [ 'upsertstatichomepage', 'upsertstaticprivacypage', 'upsertstatichelpandfaqpage', 'upsertstaticnavigation']
                                    },
                                  {
                                    'id': 'b4f63784-fe99-4774-ad28-ce7911986bea',
                                    'roleName': 'StateAdmin',
                                    'type': 'Role',
                                    'organizationalUnit': 'Hawaii | Florida',
                                    'permissions': [ 'upsertstatichomepage', 'upsertstaticprivacypage', 'upsertstatichelpandfaqpage', 'upsertstaticnavigation']
                                  }]"),
                new List<string>(new string[] { "upsertstatichomepage", "upsertstaticprivacypage", "upsertstatichelpandfaqpage","upsertstaticnavigation" }) };

            yield return new object[] {
                new UserProfile {
                    Id = Guid.Parse("e3583076-bed6-497b-847e-c732adf56925").ToString(),
                    OId ="userOId",
                    RoleInformationId = new List<Guid>{new Guid("4bf9df8f-dfee-4b08-be4d-35cc053fa298"),
                                                       new Guid("b4f63784-fe99-4774-ad28-ce7911986bea")} },
                JArray.Parse(@"[{
                                    'id': '4bf9df8f-dfee-4b08-be4d-35cc053fa298',
                                    'roleName': 'PortalAdmin',
                                    'type': 'Role',
                                    'permissions': [ 'upsertstatichomepage', 'upsertstaticprivacypage', 'upsertstatichelpandfaqpage', 'upsertstaticnavigation','upserttopic','upsertresource']
                                    },
                                  {
                                    'id': 'b4f63784-fe99-4774-ad28-ce7911986bea',
                                    'roleName': 'StateAdmin',
                                    'type': 'Role',
                                    'organizationalUnit': 'Hawaii | Florida',
                                    'permissions': [ 'upsertstatichomepage', 'upsertstaticprivacypage', 'upsertstatichelpandfaqpage', 'upsertstaticnavigation']
                                  }]"),
                new List<string>(new string[] { "upsertstatichomepage", "upsertstaticprivacypage", "upsertstatichelpandfaqpage","upsertstaticnavigation","upserttopic","upsertresource" }) };

            yield return new object[] {
                new UserProfile {
                    Id = Guid.Parse("e3583076-bed6-497b-847e-c732adf56925").ToString(),
                    OId ="userOId",
                    RoleInformationId = new List<Guid>() },
                JArray.Parse(@"[]"),
                new List<string>() };

        }

        public static IEnumerable<object[]> ValidateOUForRole()
        {
            yield return new object[] { new List<string>{"4bf9df8f-dfee-4b08-be4d-35cc053fa298"} ,"",
                new List<Role>{ new Role { RoleInformationId = new Guid("4bf9df8f-dfee-4b08-be4d-35cc053fa298"),RoleName = "PortalAdmin",
                Type = "Role", Permissions = new List<string>{ "upsertstatichomepage", "upsertstaticprivacypage", "upsertstatichelpandfaqpage" } } },
                true };

            yield return new object[] { new List<string>{"4bf9df8f-dfee-4b08-be4d-35cc053fa298",
                "b4f63784-fe99-4774-ad28-ce7911986bea"} ,"Alaska | Hawaii",
                new List<Role>{ new Role { RoleInformationId = new Guid("4bf9df8f-dfee-4b08-be4d-35cc053fa298"),RoleName = "StateAdmin",OrganizationalUnit="Alaska",
                Type = "Role", Permissions = new List<string>{ "upsertstatichomepage", "upsertstaticprivacypage", "upsertstatichelpandfaqpage" } },
                new Role { RoleInformationId = new Guid("b4f63784-fe99-4774-ad28-ce7911986bea"),RoleName = "StateAdmin",OrganizationalUnit="Hawaii",
                Type = "Role", Permissions = new List<string>{ "upsertstatichomepage", "upsertstaticnavigation", "upsertstatichelpandfaqpage" } } },
                true };
            yield return new object[] { new List<string>{"4bf9df8f-dfee-4b08-be4d-35cc053fa298",
                                                       "b4f63784-fe99-4774-ad28-ce7911986bea"} ,"Alaska | Hawaii",
                                                       new List<Role>{ new Role { RoleInformationId = new Guid("4bf9df8f-dfee-4b08-be4d-35cc053fa298"),RoleName = "StateAdmin",OrganizationalUnit="Alaska",
                Type = "Role", Permissions = new List<string>{ "upsertstatichomepage", "upsertstaticprivacypage", "upsertstatichelpandfaqpage" } },
                new Role { RoleInformationId = new Guid("b4f63784-fe99-4774-ad28-ce7911986bea"),RoleName = "StateAdmin",OrganizationalUnit="Hawaii",
                Type = "Role", Permissions = new List<string>{ "upsertstatichomepage", "upsertstaticnavigation", "upsertstatichelpandfaqpage" } } },
                true };

            yield return new object[] { new List<string>{"4bf9df8f-dfee-4b08-be4d-35cc053fa298"} ,"Hawaii",
                new List<Role>{new Role { RoleInformationId = new Guid("b4f63784-fe99-4774-ad28-ce7911986bea"),RoleName = "StateAdmin",OrganizationalUnit="Alaska",
                Type = "Role", Permissions = new List<string>{ "upsertstatichomepage", "upsertstaticnavigation", "upsertstatichelpandfaqpage" } } },
                false };

            yield return new object[] { new List<string>{"4bf9df8f-dfee-4b08-be4d-35cc053fa298"} ,"Alaska",
                new List<Role>{new Role { RoleInformationId = new Guid("b4f63784-fe99-4774-ad28-ce7911986bea"),RoleName = "StateAdmin",OrganizationalUnit="",
                Type = "Role", Permissions = new List<string>{ "upsertstatichomepage", "upsertstaticnavigation", "upsertstatichelpandfaqpage" } } },
                false };

            yield return new object[] { new List<string>{"4bf9df8f-dfee-4b08-be4d-35cc053fa298"} ,"Alaska | Hawaii",
                new List<Role>(),
                false };
        }
        public static IEnumerable<object[]> ValidateOrganizationalUnitData()
        {
            yield return new object[] { "",
                new List<Role>{ new Role { RoleInformationId = new Guid("4bf9df8f-dfee-4b08-be4d-35cc053fa298"),RoleName = "PortalAdmin",
                Type = "Role", Permissions = new List<string>{ "upsertstatichomepage", "upsertstaticprivacypage", "upsertstatichelpandfaqpage" } } },
                false};
            yield return new object[] { "userOId",
                new List<Role>{ new Role { RoleInformationId = new Guid("4bf9df8f-dfee-4b08-be4d-35cc053fa298"),RoleName = "PortalAdmin",
                Type = "Role", Permissions = new List<string>{ "upsertstatichomepage", "upsertstaticprivacypage", "upsertstatichelpandfaqpage" } } },
                false};
        }
    }
}
