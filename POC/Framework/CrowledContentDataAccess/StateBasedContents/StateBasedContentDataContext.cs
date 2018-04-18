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
            this.Database.CommandTimeout = 300;
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

        public virtual DbSet<UploadBatchReference> UploadBatchReferences { get; set; }


        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<QuestionsAndAnswers> QuestionsAndAnswers { get; set; }

        //Localization related tables
        public virtual DbSet<UsageTranslation> UsageTranslations { get; set; }
        public virtual DbSet<LanguageSupport> LanguageSupports { get; set; }
        //public virtual DbSet<LawCategory_fr_FR> LawCategories_fr_FR { get; set; }
        //public virtual DbSet<LawCategory_es_MX> LawCategories_es_MX { get; set; }
        //public virtual DbSet<Scenario_fr_FR> Scenarios_fr_FR { get; set; }
        //public virtual DbSet<Scenario_es_MX> Scenarios_es_MX { get; set; }
        //public virtual DbSet<Resource_fr_FR> Resources_fr_FR { get; set; }
        //public virtual DbSet<Resource_es_MX> Resources_es_MX { get; set; }
        //public virtual DbSet<Process_fr_FR> Processes_fr_FR { get; set; }
        //public virtual DbSet<Process_es_MX> Processes_es_MX { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<LawCategory>()
            .Property(c => c.LCID)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

    }
}
