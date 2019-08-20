using Newtonsoft.Json;

namespace Access2Justice.Shared.Spot
{
    public class SpotRequest
    {
        [JsonProperty("text")]
        public string Query { get; set; }

        [JsonProperty("save-text")]
        public int SaveText { get; set; } = 1;

        [JsonProperty("cutoff-lower")]
        public decimal CutoffLower { get; set; } = 0.2M;

        [JsonProperty("cutoff-pred")]
        public decimal CutoffPred { get; set; } = 0.5M;

        [JsonProperty("cutoff-upper")]
        public decimal CutoffUpper { get; set; } = 0.4M;
    }
}
