using CrawledContentDataAccess.StateBasedContents;
using System.Data.Entity;

namespace CrowledContentDataAccess.DataContextFactory
{
    public  interface ICrowledContentDataContextFactory
    {
        CrowledContentDataContextBase GetCrowledContentDataContext(string connectionString);
       // CrowledContentDataContextForAL GetCrowledContentDataContextForAL(string connectionString);
    }
}