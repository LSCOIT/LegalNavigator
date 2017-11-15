using ContentDataAccess.PlatformCoreSettingContents;
using ContentDataAccess.StateBasedContents;

namespace ContentDataAccess.DataContextFactory
{
    public  interface ICrowledContentDataContextFactory
    {
        StateBasedContentDataContext GetStateBasedContentDataContext(string connectionString);        
        PlatformCoreDataContext GetPlatformCoreDataContext();
    }
}