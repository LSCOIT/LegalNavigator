using ContentDataAccess;
using CrawledContentDataAccess.StateBasedContents;
using System;
using System.Collections.Generic;
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
            : base("name=StateBasedContentsDb")
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
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Process> Processes { get; set; }
        public virtual DbSet<IntentToNSMICode> IntentToNSMICodes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
