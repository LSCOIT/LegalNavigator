using ContentDataAccess.DataContextFactory;
using ContentDataAccess.PlatformCoreSettingContents;
using ContentDataAccess.StateBasedContents;
using CrawledContentDataAccess.CuratedExperienceContents;

namespace ContentDataAccess.DataContextFactory
{
    public class CrowledContentDataContextFactory : ICrowledContentDataContextFactory
    {
        public CrowledContentDataContextBase GetCrowledContentDataContext(string connectionString)
        {
            return new CrowledContentDataContextBase(connectionString);
        }

        public CuratedExperienceDataContext GetCuratedExperienceDataContext()
        {
            return new CuratedExperienceDataContext();
        }

        public PlatformCoreDataContext GetPlatformCoreDataContext()
        {
            throw new System.NotImplementedException();
        }
    }
}
