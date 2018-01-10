using ContentDataAccess.DataContextFactory;
using ContentDataAccess.PlatformCoreSettingContents;
using ContentDataAccess.StateBasedContents;

namespace ContentDataAccess.DataContextFactory
{
    public class CrowledContentDataContextFactory : ICrowledContentDataContextFactory
    {
        public StateBasedContentDataContext GetStateBasedContentDataContext(string connectionString)
        {
            return new StateBasedContentDataContext(connectionString);
        }

       
        public PlatformCoreDataContext GetPlatformCoreDataContext()
        {
            return new PlatformCoreDataContext();
        }
    }
}
