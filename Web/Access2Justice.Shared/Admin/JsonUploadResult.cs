using Newtonsoft.Json;

namespace Access2Justice.Shared.Admin
{
    public class JsonUploadResult
    {
        /// <summary>
        /// Main cause of error
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Some extra information. Could be missed
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Details { get; set; }

        /// <summary>
        /// Integer error code
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JsonUploadError? ErrorCode { get; set; }
    }
}
