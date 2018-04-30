using Access2Justice.Api.Models.CuratedExperience;
using Access2Justice.Api.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    internal class CuratedExperienceChoiceSetMapper
    {
        internal static CuratedExperienceSurvey GetQuestions(CuratedExperience curatedExperience, string surveyItemId)
        {
            var jsonTreeItem = curatedExperience.SurveyTree.Where(x => x.SurveyItemId == surveyItemId).First();

            var choiceSet = new CuratedExperienceSurvey();
            choiceSet.Id = jsonTreeItem.SurveyItemId;
            choiceSet.Title = jsonTreeItem.Description;
            choiceSet.QuestionType = jsonTreeItem.QuestionType;

            foreach (var child in jsonTreeItem.Questoins.ToList())
            {
                choiceSet.Choices.Add(new Choice
                {
                    Id = child.Id,
                    ChoiceText = child.Description
                });
            }

            return choiceSet;
        }

        internal static CuratedExperienceSurvey MapAnswersToQuestions(CuratedExperienceSurvey question, Dictionary<Guid, string> answers)
        {
            foreach (var answer in answers)
            {
                if(question.Id == answer.Key.ToString())
                {
                    question.UserAnswer = answer.Value;
                }
            }

            return question;
        }
    }
}
