using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class Compiler : ICompile
    {
        public JObject Compile(JObject personalizedPlan, Dictionary<string, string> evaluatedUserAnswers)
        {
            var stepsInScope = personalizedPlan.Properties();
            var root = ((JObject)stepsInScope.GetArrayValue("rootNode").FirstOrDefault()).Properties();
            var children = root.GetArrayValue("children").ToList();


            foreach (var child in children)
            {
                foreach (var c in child)
                {
                    //var temp2 = c.Where(x => x. = "state");
                    var temp3 = ((JObject)c).Properties().GetArrayValue("state").ToList();

                    foreach (var state in temp3)
                    {
                        var temp5 = ((JObject)state).Properties().GetValue("operator");

                        var breakpoint5 = string.Empty; // Todo:@Alaa - remove this temp code
                    }
                    //var temp4 = JsonConvert.DeserializeObject(temp3);

                    
                    var breakpoint3 = string.Empty; // Todo:@Alaa - remove this temp code
                }

                //((JObject)child).Properties().ToList();
                var breakpoint2 = string.Empty; // Todo:@Alaa - remove this temp code
            }
            


            var breakpoint = string.Empty; // Todo:@Alaa - remove this temp code
            //if (evaluatedUserAnswers.Any())
            //{
            //    stepsInScope.Active = personalizedPlan.Active;
            //    stepsInScope.CreatedAt = personalizedPlan.CreatedAt;
            //    stepsInScope.Footer = personalizedPlan.Footer;
            //    stepsInScope.GuideId = personalizedPlan.GuideId;
            //    stepsInScope.Header = personalizedPlan.Header;
            //    stepsInScope.HideFooterOnFirstPage = personalizedPlan.HideFooterOnFirstPage;
            //    stepsInScope.HideHeaderOnFirstPage = personalizedPlan.HideHeaderOnFirstPage;
            //    stepsInScope.RootNode.Id = personalizedPlan.RootNode.Id;
            //    stepsInScope.RootNode.State = personalizedPlan.RootNode.State;
            //    stepsInScope.RootNode.Tag = personalizedPlan.RootNode.Tag;
            //    stepsInScope.TemplateId = personalizedPlan.TemplateId;
            //    stepsInScope.Title = personalizedPlan.Title;
            //    stepsInScope.UpdatedAt = personalizedPlan.UpdatedAt;
            //}

            //foreach (var answer in evaluatedUserAnswers)
            //{
            //    foreach (var child in personalizedPlan.RootNode.Children)
            //    {
            //        if (answer.Key == child.State.LeftOperand && answer.Value == child.State.Operator)
            //        {
            //            stepsInScope.RootNode.Children.Add(child);
            //        }
            //    }
            //}

            return null;
        }
    }
}
