using System.Collections.Generic;

namespace Access2Justice.Shared.Interfaces
{
    public interface IA2JAuthorParser
    {
        Dictionary<string, string> Parse(string logic, Dictionary<string, string> inputVars);
        bool IsConditionSatisfied(Dictionary<string, string> ANDvariables, Dictionary<string, string> ORvariables, Dictionary<string, string> inputVars);
    }
}
