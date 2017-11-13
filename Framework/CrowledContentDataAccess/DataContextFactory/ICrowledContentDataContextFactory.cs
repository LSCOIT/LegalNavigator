using ContentDataAccess.PlatformCoreSettingContents;
using ContentDataAccess.StateBasedContents;
using CrawledContentDataAccess.CuratedExperienceContents;

namespace ContentDataAccess.DataContextFactory
{
    public  interface ICrowledContentDataContextFactory
    {
        CrowledContentDataContextBase GetCrowledContentDataContext(string connectionString);
        CuratedExperienceDataContext GetCuratedExperienceDataContext();
        PlatformCoreDataContext GetPlatformCoreDataContext();
    }
}