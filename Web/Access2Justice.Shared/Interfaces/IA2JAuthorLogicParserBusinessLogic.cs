using System.Collections.Generic;

namespace Access2Justice.Shared.Interfaces
{
    public interface IA2JAuthorParserBusinessLogic
    {
        Dictionary<string, string> ComputeLogicText(Dictionary<string, bool> ANDvariables, Dictionary<string, bool> ORvariables, Dictionary<string, string> SETvariables);
        Dictionary<string, string> Parse(string logic);
    }
}
