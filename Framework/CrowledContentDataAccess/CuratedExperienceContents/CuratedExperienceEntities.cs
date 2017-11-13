using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawledContentDataAccess.CuratedExperienceContents
{
  
    public  class LawCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LCID { get; set; }
        public int NSMICode { get; set; }
        public string Description { get; set; }
    }
    public class Resource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ResourceId { get; set; }
        public string ResourceType { get; set; }
        public string ResourceJson { get; set; }

        public virtual LawCategory LawCategory { get; set; }
    }

    public class Process
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ProcessId { get; set; }
        public string step { get; set; }
        public string ParentStep { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public virtual LawCategory LawCategory { get; set; }


    }
}
