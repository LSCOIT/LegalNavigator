namespace Access2Justice.Repository
{
    using System.Collections.Generic;

    public class LUISIntent
    {
        public string Query { get; set; }
        public Intent TopScoringIntent { get; set; }
        public List<Intent> Intents { get; set; }
        public List<LUISEntity> Entities { get; set; }
    }

    public class Intent
    {
        public string intent { get; set; }
        public decimal score { get; set; }
    }

    public class LUISEntity
    {
        public string Entity { get; set; }
        public string Type { get; set; }
        public string StartIndex { get; set; }
        public string EndIndex { get; set; }
        public string Score { get; set; }
    }
}
