using ContentDataAccess;
using CrawledContentDataAccess.StateBasedContents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentDataAccess.StateBasedContents
{
    public class StateBasedContentDataContext : DbContext
    {
        public StateBasedContentDataContext(string connectionString):base(connectionString)
        {

        }
        public StateBasedContentDataContext()
            : base("name=StateBasedContentsDb") //Default connection string
        {
        }

        //Crowled Contents
        //--Law help contents
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<SubTopic> SubTopics { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentContent> DocumentContents { get; set; }
        //--Court Contents
        public virtual DbSet<Intent> Intents { get; set; }
        public virtual DbSet<Utterance> Utterances { get; set; }


        //--Curated Contents
        public virtual DbSet<LawCategory> LawCategories { get; set; }
        public virtual DbSet<LawCategoryParentChild> LawCategoryParentChildren { get; set; }
        public virtual DbSet<LawCategorySibling> LawCategorySiblings { get; set; }        
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Process> Processes { get; set; }
        public virtual DbSet<IntentToNSMICode> IntentToNSMICodes { get; set; }
        public virtual DbSet<Scenario> Scenarios { get; set; }
        public virtual DbSet<LawCategoryToScenario> LawCategoryToScenarios { get; set; }
        public virtual DbSet<ScenarioToLawCategory> ScenarioToLawCategories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<LawCategory>()
            .Property(c => c.LCID)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

    }
}
