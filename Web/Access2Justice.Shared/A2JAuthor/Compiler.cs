﻿using Access2Justice.Shared.Interfaces.A2JAuthor;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public class Compiler : ICompile
    {
        public JObject Compile(JObject personalizedPlan, Dictionary<string, string> evaluatedUserAnswers)
        {
            var stepsInScope = new JObject();

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

            return stepsInScope;
        }
    }
}
