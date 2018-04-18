using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawledContentDataAccess.CuratedExperienceContents
{
    public class CuratedExperienceDataContext: DbContext
    {
        public CuratedExperienceDataContext()
            : base("name=CuratedExperienceDb")
        {
        }
        public virtual DbSet<LawCategory> LawCategories { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Process> Processes { get; set; }

    }

}
