using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class IfElseParser : IIfElseParser
    {
        private Dictionary<string, string> setVars = new Dictionary<string, string>();
        private Dictionary<string, string> ifVars = new Dictionary<string, string>();
        private Dictionary<string, string> andVars = new Dictionary<string, string>();
        private Dictionary<string, string> orVars = new Dictionary<string, string>();

        public IfElseParser SET(Dictionary<string, string> SETvars)
        {
            setVars.AddRange(SETvars);
            return this;
        }

        public IfElseParser IF(Dictionary<string, string> IFvars)
        {
            ifVars.AddRange(IFvars);
            return this;
        }

        public IfElseParser AND(Dictionary<string, string> ANDvars)
        {
            andVars.AddRange(ANDvars);
            return this;
        }

        public IfElseParser OR(Dictionary<string, string> ORvars)
        {
            orVars.AddRange(ORvars);
            return this;
        }

        public Dictionary<string, string> IsEqualTo(Dictionary<string, string> userAnswers)
        {
            var result = userAnswers.Intersect(andVars).ToDictionary(x => x.Key, x => x.Value);
            if (result.Any())
            {
                return setVars;
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }
    }
}
