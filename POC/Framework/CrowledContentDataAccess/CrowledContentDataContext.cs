using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CrowledContentDataAccess
{
    public class CrowledContentDataContext: DbContext
    {
        public CrowledContentDataContext()
            : base("name=CrowledContentsDb")
        {
        }
        // public virtual DbSet<CsvFile> CsvFileContents { get; set; }
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
