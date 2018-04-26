using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Api.Models.CuratedExperience;
using Access2Justice.Api.ViewModels;
using Microsoft.VisualBasic;

namespace Access2Justice.Api.BusinessLogic
{
    public class CuratedExperienceChoiceSetMapper
    {
        public static CuratedExperienceChoiceSet GetQuestions(CuratedExperience curatedExperience, string id)
        {
            var jsonTreeItem = curatedExperience.SurvayTree.Where(x => x.SurvayItemId == id).First();

            var choiceSet = new CuratedExperienceChoiceSet();
            choiceSet.Id = jsonTreeItem.SurvayItemId;
            choiceSet.Title = jsonTreeItem.Description;
            choiceSet.QuestionType = jsonTreeItem.QuestionType;

            foreach (var child in jsonTreeItem.Questoins.ToList())
            {
                choiceSet.Choice.Add(new Choice
                {
                    Id = child.Id,
                    ChoiceText = child.Description
                });
            }

            return choiceSet;
        }
    }
}
