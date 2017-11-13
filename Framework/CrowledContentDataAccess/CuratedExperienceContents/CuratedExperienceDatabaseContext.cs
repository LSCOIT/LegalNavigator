using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawledContentDataAccess.CuratedExperienceContents
{
    class CuratedExperienceDatabaseContext: DbContext
    {
        public CuratedExperienceDatabaseContext()
            : base("name=CuratedExperienceDb")
        {
        }
        public virtual DbSet<LawCategory> LawCategories { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Process> Processes { get; set; }

    }

}
