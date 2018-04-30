using Access2Justice.Api.Models.CuratedExperience;
using Access2Justice.Api.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Api.BusinessLogic
{
    internal class CuratedExperienceChoiceSetMapper
    {
        internal static CuratedExperienceSurveyViewModel GetQuestions(CuratedExperienceSurvey curatedExperience, string surveyItemId)
        {
            var jsonTreeItem = curatedExperience.SurveyTree.Where(x => x.QuestionId == surveyItemId).First();

            var survey = new CuratedExperienceSurveyViewModel();
            survey.QuestionId = jsonTreeItem.QuestionId;
            survey.Title = jsonTreeItem.QuestionText;
            survey.QuestionType = jsonTreeItem.QuestionType;

            foreach (var child in jsonTreeItem.Choices.ToList())
            {
                survey.ChoicesViewModel.Add(new ChoiceViewModel
                {
                    ChoiceId = child.ChoiceId,
                    ChoiceText = child.ChoiceText
                });
            }

            return survey;
        }

        internal static CuratedExperienceSurveyViewModel MapAnswersToQuestions(CuratedExperienceSurveyViewModel question, Dictionary<Guid, string> answers)
        {
            foreach (var answer in answers)
            {
                if(question.QuestionId == answer.Key.ToString())
                {
                    question.UserAnswer = answer.Value;
                }
            }

            return question;
        }
    }
}
