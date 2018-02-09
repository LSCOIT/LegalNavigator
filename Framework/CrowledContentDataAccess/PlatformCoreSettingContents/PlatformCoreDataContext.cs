using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ContentDataAccess.PlatformCoreSettingContents
{
    public class PlatformCoreDataContext : DbContext
    {
      
        public PlatformCoreDataContext()
            : base("name=PlatformCoreDb")
        {
        }
        
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<CrawlerSetting> CrawlerSettings { get; set; }
        public virtual DbSet<LawTaxonomy> LawTaxonomies { get; set; }
        public virtual DbSet<State> States { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<State>()
            .Property(c => c.Id)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        }

    }
}
