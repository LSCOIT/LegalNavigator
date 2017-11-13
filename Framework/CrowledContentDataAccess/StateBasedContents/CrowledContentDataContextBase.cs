using ContentDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentDataAccess.StateBasedContents
{
    public class CrowledContentDataContextBase: DbContext
    {
        public CrowledContentDataContextBase(string connectionString):base(connectionString)
        {

        }
        public CrowledContentDataContextBase()
            : base("name=CrowledContentsDb")
        {
        }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<SubTopic> SubTopics { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentContent> DocumentContents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }

    }
}
