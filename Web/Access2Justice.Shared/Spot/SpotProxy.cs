using Access2Justice.Shared.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Spot
{
    public class SpotProxy : ILuisProxy
    {
        private readonly ISpotSettings _settings;
        private readonly IHttpClientService _httpClientService;

        public SpotProxy(IHttpClientService httpClientService, ISpotSettings settings)
        {
            _httpClientService = httpClientService;
            _settings = settings;
        }

        public async Task<string> GetIntents(string query)
        {
            var requestContent = JsonConvert.SerializeObject(
                new SpotRequest
                    {
                        Query = query,
                        CutoffPred = _settings.IntentAccuracyThreshold
                    });

            var spotResponseContent = string.Empty;

            var request =
                new HttpRequestMessage(HttpMethod.Post, _settings.Endpoint.OriginalString)
                {
                    Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
                };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ApiToken);

            using (var response = await _httpClientService.SendAsync(request))
            {
                spotResponseContent = await response.Content.ReadAsStringAsync();
            }

            var result = new LuisIntent()
            {
                Query = query,
                Intents = Array.Empty<Intents>()
            };

            if (!string.IsNullOrEmpty(spotResponseContent))
            {
                var spotResponse =
                    JsonConvert.DeserializeObject<SpotResponse>(spotResponseContent);

                if (spotResponse.Labels != null &&
                    spotResponse.Labels.Length > 0)
                {
                    var intents = spotResponse.Labels.
                        OrderByDescending(l => l.PredictionScore).
                        Take(_settings.TopIntentsCount).
                        Select(l => new Intents { Intent = l.Name, Score = l.PredictionScore }).
                        ToArray();

                    var topIntent = intents.First();

                    result.TopScoringIntent = topIntent;
                    result.Intents = intents;
                }
            }

            return JsonConvert.SerializeObject(result);
        }
    }
}
