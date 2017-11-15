using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDataAccess.StateBasedContents
{
    public class DocumentContent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentContentId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public virtual Document Document { get; set; }
    }

    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public bool HasMoreContents { get; set; }

        public List<DocumentContent> DocumentContents { get; set; }

        public virtual SubTopic SubTopic { get; set; }
    }

    public class SubTopic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubTopicId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }

        public List<Document> Docs { get; set; }
        // public ICollection<Document> Docs { get; set; }

        public virtual Topic Topic { get; set; }
    }
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int topicId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }

        //public ICollection<SubTopic> SubTopics { get; set; }
        public List<SubTopic> SubTopics { get; set; }
    }

    public class RelevantTopic
    {
        public string Url { get; set; }
        public string Name { get; set; }
    }
}
