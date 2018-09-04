using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class Parser : IParse, IEvaluate, ICompile
    {
        private readonly IEvaluate evaluator;
        private readonly ICompile compilePersonalizedPlan;

        public Parser(IEvaluate evaluator, ICompile compilePersonalizedPlan)
        {
            this.evaluator = evaluator;
            this.compilePersonalizedPlan = compilePersonalizedPlan;
        }

        public Dictionary<string, string> Evaluate(CuratedExperienceAnswers curatedExperienceAnswers)
        {
            // from business logic 
            // -------------
            Dictionary<string, string> userAnswers = new Dictionary<string, string>();

            foreach (ButtonComponent button in curatedExperienceAnswers.ButtonComponents)
            {
                userAnswers.Add(button.Name, button.Value);
            }

            foreach (FieldComponent fieldComponent in curatedExperienceAnswers.FieldComponents)
            {
                foreach (AnswerField field in fieldComponent.Fields)
                {
                    userAnswers.Add(field.Name, field.Value);
                }
            }
            // ----------

            Dictionary<string, string> actionPlan = new Dictionary<string, string>();

            foreach (ButtonComponent answer in curatedExperienceAnswers.ButtonComponents)
            {
                Dictionary<string, string> leftVarValues = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(answer.CodeAfter))
                {
                    string leftLogic = answer.CodeAfter.GetStringOnTheLeftOf("SET");
                     // Todo:@Alaa AND and OR must be treated differently
                    leftVarValues.AddRange(leftLogic.GetVariablesWithValues("AND")).AddRange(leftLogic.GetVariablesWithValues("OR"));
                }


                //Dictionary<string, string> rightVarValues = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> leftVarValue in leftVarValues)
                {
                    var plan = userAnswers.Where(x => x.Key == leftVarValue.Key && x.Value == leftVarValue.Value).FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(plan.Key))
                    {
                        // extract right to SET and set values
                        string rightLogic = answer.CodeAfter.GetStringOnTheRightOf("SET");

                        actionPlan.AddRange(rightLogic.SetValueTOVar());
                    }

                    string breakpoint = string.Empty; // Todo:@Alaa - remove this temp code
                }
            }
            /*
    2. Divide logic to left and right Dictionary<string, string>
	3. Loop through the left/right dic
	4. Take the first item left side, extract vars and operand
	5. Check the vars/values against the answers vars/values (the entire list of answer vars/values)
    5.2. If condition is satisfied (evaluates to true) extract right side vars/values and add them to PlanDictionary<string,string> , if not skip
    5.3. return this dic
             */

            return actionPlan;
        }

        public A2JPersonalizedPlan Compile(Dictionary<string, string> evaluatedUserAnswers)
        {
            // 1. PlanDictionary now has all the vars needed to loop through the template, loop through the children and extract those that match, return children that are in scope.
            var temp = evaluatedUserAnswers;

            return null;
        }
    }
}
