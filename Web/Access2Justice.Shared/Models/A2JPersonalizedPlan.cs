using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class A2JPersonalizedPlan
    {
        public string title { get; set; }
        public string active { get; set; }
        public string header { get; set; }
        public string hideHeaderOnFirstPage { get; set; }
        public string footer { get; set; }
        public string hideFooterOnFirstPage { get; set; }
        public RootNode rootNode { get; set; }
        public string createdAt { get; set; }
        public string guideId { get; set; }
        public int templateId { get; set; }
        public long updatedAt { get; set; }
    }

    public class RootNode
    {
        public string tag { get; set; }
        public State state { get; set; }
        public List<Child> children { get; set; }
        public string id { get; set; }
    }

    public class State
    {
        public string fontFamily { get; set; }
        public string fontSize { get; set; }
        public string sectionCounter { get; set; }
        public string @operator { get; set; }
        public string leftOperand { get; set; }
        public string rightOperand { get; set; }
        public string rightOperandType { get; set; }
        public string hasConditionalLogic { get; set; }
    }

    public class Child
    {
        public string tag { get; set; }
        public string id { get; set; }
        public ChildState state { get; set; }
    }

    public class ChildState
    {
        public string userContent { get; set; }
        public string notes { get; set; }
    }

}
