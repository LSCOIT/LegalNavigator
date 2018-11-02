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
            public const string True = "TRUE";
            public const string A2JLogicalTrue = "is-true";
        }

        public class FalseTokens
        {
            public const string False = "FALSE";
            public const string A2JLogicalFalse = "is-false";
        }

        public class ParserConfig
        {
            public const string SetVariables = "SET";
            public const string GoToQuestions = "GOTO";
        }
    }
}
