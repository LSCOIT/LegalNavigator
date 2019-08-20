using Newtonsoft.Json;

namespace Access2Justice.Shared.Spot
{
    public class SpotResponse
    {
        [JsonProperty("query-id")]
        public string QueryId { get; set; }

        [JsonProperty("text")]
        public string Query { get; set; }

        [JsonProperty("save-text")]
        public int? SaveText { get; set; }

        [JsonProperty("cutoff-lower")]
        public decimal? CutoffLower { get; set; }

        [JsonProperty("cutoff-pred")]
        public decimal? CutoffPrediction { get; set; }

        [JsonProperty("cutoff-upper")]
        public decimal? CutoffUpper { get; set; }

        [JsonProperty("labels")]
        public SpotLabel[] Labels { get; set; }
    }

    public class SpotLabel
    {
        [JsonProperty("id")]
        public string NsmiCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lower")]
        public decimal LowerScore { get; set; }

        [JsonProperty("pred")]
        public decimal PredictionScore { get; set; }

        [JsonProperty("upper")]
        public decimal UpperScore { get; set; }
    }
}
