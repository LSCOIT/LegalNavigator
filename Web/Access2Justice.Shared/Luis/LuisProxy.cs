using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Luis
{
    public class LuisProxy : ILuisProxy
    {
        private readonly ILuisSettings _luisSettings;
        private readonly IHttpClientService _httpClientService;

        public LuisProxy(IHttpClientService httpClientService, ILuisSettings luisSettings)
        {
            _luisSettings = luisSettings;
            _httpClientService = httpClientService;
        }

        public IEnumerable<string> FilterLuisIntents(IntentWithScore intentWithScore)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetIntents(string query)
        {
            var uri = string.Format(CultureInfo.InvariantCulture, _luisSettings.Endpoint.OriginalString, query);

            string result = string.Empty;
            using (var response = await _httpClientService.GetAsync(new Uri(uri)))
            {
                result = response.Content.ReadAsStringAsync().Result;
            }

            return result;
        }
    }
}

