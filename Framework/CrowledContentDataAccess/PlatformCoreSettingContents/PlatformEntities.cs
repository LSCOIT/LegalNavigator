using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentDataAccess.PlatformCoreSettingContents
{   
    public class User
    {
        public User()
        {
            this.Roles = new HashSet<Role>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }

    public class Role
    {
        public Role()
        {
            this.Users = new HashSet<User>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        public string StateAdmin { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
   

    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }        
        public string LocationName { get; set; }
        public string DbConnection { get; set; }
        public string GeoAddress { get; set; }

        public virtual ICollection<CrawlerSetting> CrawlerSettings { get; set; }
    }

    public class CrawlerSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CrawlerSettingId { get; set; }
        public int LocationId { get; set; }
        public string SiteUrl { get; set; }
        public bool IsTrusted { get; set; }
        public string CrawlerAlgorithm { get; set; }
        //public bool IsRecursive { get; set; }

        public virtual Location Location { get; set; }

    }

    public class LawTaxonomy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int LawTaxonomyId { get; set; }
        public int NodeId { get; set; }
        public string NodeName { get; set; }
        public int ParentNodeId { get; set; }
        public string NSMICode { get; set; }
        public string Title { get; set; }
        public decimal Percentage { get; set; }
    }
}
