using ContentDataAccess.DataContextFactory;
using ContentDataAccess.StateBasedContents;

namespace ContentDataAccess.DataContextFactory
{
    public class CrowledContentDataContextFactory : ICrowledContentDataContextFactory
    {
        public CrowledContentDataContextBase GetCrowledContentDataContext(string connectionString)
        {
            return new CrowledContentDataContextBase(connectionString);
        }
        //public CrowledContentDataContextForAL GetCrowledContentDataContextForAL(string connectionString)
        //{
        //    return new CrowledContentDataContextForAL(connectionString);
        //}
    }
}
