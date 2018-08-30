using Access2Justice.Shared.Interfaces;
using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public class IfElseParser : IIfElseParser
    {
        private string a2jLogic;
        private Dictionary<string, string> SETvars = new Dictionary<string, string>();
        private Dictionary<string, string> IFvars = new Dictionary<string, string>();
        private Dictionary<string, string> ANDvars = new Dictionary<string, string>();
        private Dictionary<string, string> ORvars = new Dictionary<string, string>();

        public IfElseParser OnIfStatements(string a2jLogic)
        {
            this.a2jLogic = a2jLogic;
            return this;
        }

        public IfElseParser SET(Dictionary<string, string> SETvars)
        {
            //this.SETvars = "testing" + SETvars;
            return this;
        }

        public IfElseParser IF(Dictionary<string, string> Ifvars)
        {
            //Ifvars = "testing" + Ifvars;
            return this;
        }

        public IfElseParser AND(Dictionary<string, string> ANDvars)
        {
            //this.ANDvars = "testing" + ANDvars;
            return this;
        }

        public IfElseParser OR(Dictionary<string, string> ORvars)
        {
            this.ORvars = ORvars;
            return this;
        }

        public Dictionary<string, string> IsEqualTo(Dictionary<string, string> userAnswers)
        {
            var dic = new Dictionary<string, string>();
            //dic.Add(SETvars, ANDvars);

            return dic;
        }
    }
}
