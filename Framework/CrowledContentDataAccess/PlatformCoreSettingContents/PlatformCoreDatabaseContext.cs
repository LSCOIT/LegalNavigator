using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ContentDataAccess.PlatformCoreSettingContents
{
    class PlatformCoreDatabaseContext: DbContext
    {
        public PlatformCoreDatabaseContext()
            : base("name=PlatformCoreDb")
        {
        }
        public virtual DbSet<User> Topics { get; set; }
        public virtual DbSet<Role> SubTopics { get; set; }
        public virtual DbSet<Location> Documents { get; set; }
        public virtual DbSet<CrawlerSetting> DocumentContents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }

    }
}
