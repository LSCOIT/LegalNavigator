namespace Access2Justice.Shared.A2JAuthor
{
    public static class Tokens
    {
        public const string SET = "SET";
        public const string TO = "TO";
        public const string AND = "AND";
        public const string OR = "OR";
        public const string IF = "IF";
        public const string ENDIF = "END IF";
        public const string GOTO = "GOTO";
        public const string EqualSign = "=";
        public const string EmptySpace = " ";
        public const string VarNameLeftSign = "[";
        public const string VarNameRightSign = "]";
        public const string MacroSign = "%%";
        public const string CustomHtmlTag = "<legal-nav-resource-id>";
        public const string CustomHtmlClosingTag = "</legal-nav-resource-id>";

        public class TrueTokens
        {
            public const string TrueText = "TRUE";
            public const string LogicalTrue = "true";
            public const string LogicalTrueText = "is-true";
        }

        public class FalseTokens
        {
            public const string FalseText = "FALSE";
            public const string LogicalFalse = "false";
            public const string LogicalFalseText = "is-false";
        }

        public class ParserConfig
        {
            public const string SetVariables = "SET";
            public const string GoToQuestions = "GOTO";
        }
    }
}
