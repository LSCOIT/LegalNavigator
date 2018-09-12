// Todo:@Alaa remove! (seems like strong-typing this object is more the code more complicated than dealing with it as a dynmaic object, I'll use the dynamic approach.)

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Access2Justice.Shared.Models
{
    // the naming convention is this way because we don't have control over the json file and
    // wanted to make sure we don't diverge far from their names to make it easier to compare/map.
    public class A2JPersonalizedPlan
    {
        public A2JPersonalizedPlan()
        {
            RootNode = new RootNode();
        }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "active")]
        public string Active { get; set; }

        [JsonProperty(PropertyName = "header")]
        public string Header { get; set; }

        [JsonProperty(PropertyName = "hideHeaderOnFirstPage")]
        public string HideHeaderOnFirstPage { get; set; }

        [JsonProperty(PropertyName = "footer")]
        public string Footer { get; set; }

        [JsonProperty(PropertyName = "hideFooterOnFirstPage")]
        public string HideFooterOnFirstPage { get; set; }

        [JsonProperty(PropertyName = "rootNode")]
        public RootNode RootNode { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "guideId")]
        public string GuideId { get; set; }

        [JsonProperty(PropertyName = "templateId")]
        public int TemplateId { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public long UpdatedAt { get; set; }
    }

    public class RootNode
    {
        public RootNode()
        {
            Children = new List<Child>();
        }

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "state")]
        public State State { get; set; }

        [JsonProperty(PropertyName = "children")]
        public List<Child> Children { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }

    public class RootNodeRootNode
    {
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "children")]
        public List<ChildChildChild> Children { get; set; }
    }

    public class Child
    {
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "state")]
        public StateState State { get; set; }

        [JsonProperty(PropertyName = "children")]
        public List<ChildChild> Children { get; set; }
    }

    public class ChildChild
    {
        [JsonProperty(PropertyName = "guideId")]
        public string GuideId { get; set; }

        [JsonProperty(PropertyName = "templateId")]
        public string TemplateId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "active")]
        public string Active { get; set; }

        [JsonProperty(PropertyName = "header")]
        public string Header { get; set; }

        [JsonProperty(PropertyName = "hideHeaderOnFirstPage")]
        public string HideHeaderOnFirstPage { get; set; }

        [JsonProperty(PropertyName = "footer")]
        public string Footer { get; set; }

        [JsonProperty(PropertyName = "hideFooterOnFirstPage")]
        public string HideFooterOnFirstPage { get; set; }

        [JsonProperty(PropertyName = "rootNode")]
        public RootNodeRootNode RootNode { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }

    public class ChildChildChild
    {
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "state")]
        public StateStateState State { get; set; }
    }

    public class State
    {
        [JsonProperty(PropertyName = "fontFamily")]
        public string FontFamily { get; set; }

        [JsonProperty(PropertyName = "fontSize")]
        public string FontSize { get; set; }

        [JsonProperty(PropertyName = "sectionCounter")]
        public string SectionCounter { get; set; }

        [JsonProperty(PropertyName = "operator")]
        public string Operator { get; set; }

        [JsonProperty(PropertyName = "leftOperand")]
        public string LeftOperand { get; set; }

        [JsonProperty(PropertyName = "rightOperand")]
        public string RightOperand { get; set; }

        [JsonProperty(PropertyName = "rightOperandType")]
        public string RightOperandType { get; set; }

        [JsonProperty(PropertyName = "hasConditionalLogic")]
        public string HasConditionalLogic { get; set; }
    }

    public class StateState
    {
        [JsonProperty(PropertyName = "userContent")]
        public string UserContent { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }

        [JsonProperty(PropertyName = "operator")]
        public string Operator { get; set; }

        [JsonProperty(PropertyName = "elseClause")]
        public string ElseClause { get; set; }

        [JsonProperty(PropertyName = "leftOperand")]
        public string LeftOperand { get; set; }

        [JsonProperty(PropertyName = "rightOperand")]
        public string RightOperand { get; set; }

        [JsonProperty(PropertyName = "leftOperandType")]
        public string LeftOperandType { get; set; }

        [JsonProperty(PropertyName = "rightOperandType")]
        public string RightOperandType { get; set; }
    }

    public class StateStateState
    {
        [JsonProperty(PropertyName = "userContent")]
        public string UserContent { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
    }
}
