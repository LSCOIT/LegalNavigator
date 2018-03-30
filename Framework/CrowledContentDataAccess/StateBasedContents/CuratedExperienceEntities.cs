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
        public string StateDeviation { get; set; }
        //public LCType LCType { get;set; }
        //public LawCategory[] RelatedIntents { get; set; }
       // [ForeignKey("UploadBatchReference")]
        public int? Batch_Id { get; set; }

        public List<Process> Processes { get; set; }
        public List<Resource> Resources { get; set; }

        //public virtual UploadBatchReference UploadBatchReference { get; set; }
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
        public int LawCategoryParentChildID { get; set; }
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
        public StepType stepType { get; set; }

        public virtual LawCategory LawCategory { get; set; }
        //  public virtual Scenario Scenario { get; set; }

    }

    public class UploadBatchReference{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Batch_Id { get; set; }
        public string Name { get; set; }
        public DateTime TimeStamp { get; set; }
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

    public class Video
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VideoId { get; set; }    
        public string Title { get; set; }
        public string Url { get; set; }
        public string ResourceJson { get; set; }//Body
        public string ActionType { get; set; }

        //Let the code drive this column
        public int LCID { get; set; }
    }

    public class QuestionsAndAnswers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QAId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }       
        public string NsmiCode { get; set; }

        public string Intent { get; set; }

        public int? Batch_Id { get; set; }
    }

   /* public class LawCategory_fr_FR //ToDo: To be popaulated Manually
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EnglishLcId { get; set; }
        public string Description { get; set; }
        public string StateDeviation { get; set; }
    }
    public class LawCategory_es_MX //ToDo: To be popaulated Manually
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EnglishLcId { get; set; }
        public string Description { get; set; }
        public string StateDeviation { get; set; }
    }

    public class Scenario_fr_FR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }       
        public int EnglishScenarioId { get; set; }       
        public string Description { get; set; }
        public string Outcome { get; set; }

    }
    public class Scenario_es_MX
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EnglishScenarioId { get; set; }
        public string Description { get; set; }
        public string Outcome { get; set; }

    }

    public class Process_fr_FR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EnglishProcessId { get; set; }
        public string Title { get; set; }
        public string ActionJson { get; set; }
        public string Description { get; set; }
    }

    public class Process_es_MX
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EnglishProcessId { get; set; }
        public string Title { get; set; }
        public string ActionJson { get; set; }
        public string Description { get; set; }
    }

    public class Resource_fr_FR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EnglishResourceId { get; set; }       
        public string ResourceJson { get; set; }//Body
        public string Title { get; set; }       
    }

    public class Resource_es_MX
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EnglishResourceId { get; set; }
        public string ResourceJson { get; set; }//Body
        public string Title { get; set; }
    }*/

    public class UsageTranslation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //limit
        public int Limit { get; set; }

        //usedtillNow
        public int UsedTillNow { get; set; }

        //lastUpdated
        public int LastUpdated { get; set; }

        //TimeStamp
        public DateTime LastRunTime { get; set; }
    }
    public class LanguageSupport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //name
        public string LanguageName { get; set; }

        //usedtillNow
        public string LanguageCode { get; set; }
    }
}
