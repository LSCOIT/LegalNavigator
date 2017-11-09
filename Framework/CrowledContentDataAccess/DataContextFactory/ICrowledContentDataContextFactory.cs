using ContentDataAccess.StateBasedContents;

namespace ContentDataAccess.DataContextFactory
{
    public  interface ICrowledContentDataContextFactory
    {
        CrowledContentDataContextBase GetCrowledContentDataContext(string connectionString);
       // CrowledContentDataContextForAL GetCrowledContentDataContextForAL(string connectionString);
    }
}