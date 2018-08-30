using Access2Justice.Shared.A2JAuthor;
using System.Collections.Generic;

namespace Access2Justice.Shared.Interfaces
{
    public interface IIfElseParser
    {
        IfElseParser OnIfStatements(string a2jLogic);
        IfElseParser SET(Dictionary<string, string> SETvars);
        IfElseParser IF(Dictionary<string, string> Ifvars);
        IfElseParser AND(Dictionary<string, string> ANDvars);
        IfElseParser OR(Dictionary<string, string> ORvars);
        Dictionary<string, string> IsEqualTo(Dictionary<string, string> userAnswers);
    }
}
