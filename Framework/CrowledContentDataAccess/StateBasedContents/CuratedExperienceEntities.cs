using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawledContentDataAccess.StateBasedContents
{

    public class LawCategory //ToDo: To be popaulated Manually
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LCID { get; set; }
        public string NSMICode { get; set; }
        public string Description { get; set; }
        //public LCType LCType { get;set; }
        //public LawCategory[] RelatedIntents { get; set; }
       
    }

    public class LawCategorySibling
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LawCategorySiblingID { get; set; }      
        public virtual LawCategory LawCategory { get; set; }
        public virtual LawCategory SiblingLawCategory { get; set; }
    }

    public class LawCategoryParentChild
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LawCategoryParentChildID{ get; set; }
        public virtual LawCategory ParentLawCategory { get; set; }
        public virtual LawCategory ChildLawCategory { get; set; } 
   }


 public class Resource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ResourceId { get; set; }
        public string ResourceType { get; set; }
        public string ResourceJson { get; set; }//Body
        public string Title { get; set; }
        public string Action { get; set; }

        public virtual LawCategory LawCategory { get; set; }
    }

    public class Process
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }      
        public string ParentId { get; set; }
        public string Title { get; set; }
        public string ActionJson { get; set; }
        public string Description { get; set; }
        public StepType  stepType{get;set;}

        public virtual LawCategory LawCategory { get; set; }
      //  public virtual Scenario Scenario { get; set; }

    }
    public enum ResourceType
    {
        Related ,
        Title ,
        Phone,
        Address,
        Url

    }

    public enum StepType
    {
        Description=1 ,
        Action=2
    }

    public enum LCType
    {
        Intent=1,
        Scenario
    }
    
    //public class Quesion
    //{
    //   public string FollowUpQuestion { get; set; }
    //   public string[] PossibleAnswers { get; set; }
    //}

    public class IntentToNSMICode{//ToDo:Populate Manaully
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Provide data annotation to constrain length
        public string NSMICode { get; set; }
        //
        public string Intent { get; set; }
        }

    public class Scenario
    {//ToDo:Populate Manaully
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScenarioId { get; set; }
        //Let the code drive this column
        public int LC_ID { get; set; }        
        public string Description { get; set; }
        public string StateDeviation { get; set; }
        public string Outcome { get; set; }
        //public virtual LawCategory LawCategory { get; set; }
    }

    public class LawCategoryToScenario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //Let the code drive this column
        public int LCID { get; set; }
        public int ScenarioId { get; set; }       

    }
    public class ScenarioToLawCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ScenarioId { get; set; }
        //Let the code drive this column
        public int LCID { get; set; }
    }
}
