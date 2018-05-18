namespace Access2Justice.Shared
{
    using System.Collections.Generic;

    public class LuisIntent
    {
        public string Query { get; set; }
        public Intents TopScoringIntent { get; set; }
        public IEnumerable<Intents> Intents { get; set; }
        public IEnumerable<LuisEntity> Entities { get; set; }
    }

    public class Intents
    {
        public string Intent { get; set; }
        public decimal Score { get; set; }
    }

    public class LuisEntity
    {
        public string Entity { get; set; }
        public string Type { get; set; }
        public string StartIndex { get; set; }
        public string EndIndex { get; set; }
        public string Score { get; set; }
    }
}
