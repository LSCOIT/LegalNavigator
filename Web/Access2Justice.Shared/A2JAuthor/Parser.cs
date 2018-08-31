using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
            /*
    2. Divide logic to left and right Dictionary<string, string>
	3. Loop through the left/right dic
	4. Take the first item left side, extract vars and operand
	5. Check the vars/values against the answers vars/values (the entire list of answer vars/values)
    5.2. If condition is satisfied (evaluates to true) extract right side vars/values and add them to PlanDictionary<string,string> , if not skip
    5.3. return this dic
             */

            throw new NotImplementedException();
        }

        public A2JPersonalizedPlan Compile(Dictionary<string, string> evaluatedUserAnswers)
        {
    // 1. PlanDictionary now has all the vars needed to loop through the template, loop through the children and extract those that match, return children that are in scope.

            throw new NotImplementedException();
        }
    }
}
