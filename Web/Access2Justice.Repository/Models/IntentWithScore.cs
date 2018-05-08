namespace Access2Justice.Repository
{
    public class IntentWithScore
    {
        /// <summary>
        /// Top Scoring Intent
        /// </summary>
        public string TopScoringIntent { get; set; }

        /// <summary>
        /// Score weight of the intent with respect to the user input
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        ///  Top three intents other than the top Scoring intent
        /// </summary>
        public string[] TopThreeIntents { get; set; }

        /// <summary>
        /// Error message in case error happens before completion of result parsing
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Indicator of whether the parsing is successful or not
        /// </summary>
        public bool IsSuccessful { get; set; }
    }
}
