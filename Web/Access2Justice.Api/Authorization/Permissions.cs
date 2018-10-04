namespace Access2Justice.Api.Authorization
{
    public class Permissions
    {
        public enum PermissionName
        {
            upsertstatichomepage = 1,
            upsertstaticprivacypage,
            upsertstatichelpandfaqpage,
            upsertstaticnavigation,
            upsertstaticaboutpage,
            createtopicsupload,
            createresourcesupload,
            upsertresourcedocument,
            upserttopicdocument,
            upserttopic,
            upsertresource,
            import,
            updateplan,
            generatepermalink,
            checkpermalink,
            removepermalink,
            getpermallinkresource,
            getuserprofile,
            getuserprofiledata,
            updateuserprofile,
            upsertuserpersonalizedplan            
        }
    }
}
