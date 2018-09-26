using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Authorization
{
   // public class UserRoles
   // {
   //     public enum RoleEnum
   //     {
   //         GlobalAdmin=1,
   //         StateAdmin,
   //         Developer,
   //         Authenticated,
   //         Anonymous
   //     }

   //     public enum PolicyEnum
   //     {
   //         GlobalAdminPolicy =1,
   //         StateAdminPolicy,
   //         DeveloperPolicy,
   //         AuthenticatedPolicy,
   //         AnonymousPolicy,
			//AdminRolesPolicy,
			//AuthenticatedUserPolicy
   //     }
   // }

    public class Permissions
    {
        public enum PermissionType
        {
            Restricted = 1,
            Anonymous
        }

        public enum PermissionName
        {
            Search = 1,
            GeneratePermaLink,
            CheckPermaLink,
            RemovePermaLink,
            GetPermalLinkResource,
            GetStaticResources,
            upsertstatichomepage,
            upsertstaticprivacypage,
            upsertstatichelpandfaqpage,
            upsertstaticnavigation,
            upsertstaticaboutpage,
            gettopics,
            getsubtopics,
            getresource,
            getresourcedetails,
            getdocument,
            resources,
            getbreadcrumbs,
            gettopicdetails,
            getorganizationdetails,
            getschematopic,
            getschemaactionplan,
            getschemaarticle,
            getschemavideo,
            getschemaorganization,
            getschemaform,
            getschemaessentialreading,
            createresources,
            upsertresourcedocument,
            createupload,
            upserttopicdocument,
            personalizedresources,
            getuserprofile,
            getuserprofiledata,
            updateuserprofile,
            upsertuserpersonalizedplan,
            upsertuserprofile,
            websearch,
            Import,
            Start,
            Component,
            SaveAndGetNext,
            PersonalizedPlan,
            updateplan,
            getplandetails,
            getplan
        }

    }
}
