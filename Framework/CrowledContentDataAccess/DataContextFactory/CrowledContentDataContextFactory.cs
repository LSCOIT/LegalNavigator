using CrowledContentDataAccess.DataContextFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CrowledContentDataAccess;
using CrawledContentDataAccess.StateBasedContents;

namespace CrawledContentDataAccess.DataContextFactory
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
