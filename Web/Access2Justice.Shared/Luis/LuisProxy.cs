using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Luis
{
    public class LuisProxy : ILuisProxy
    {
        private readonly ILuisSettings luisSettings;
        private readonly IHttpClientService httpClientService;

        public LuisProxy(IHttpClientService httpClientService, ILuisSettings luisSettings)
        {
            this.luisSettings = luisSettings;
            this.httpClientService = httpClientService;
        }

        public IEnumerable<string> FilterLuisIntents(IntentWithScore intentWithScore)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetIntents(string query)
        {
            var uri = string.Format(CultureInfo.InvariantCulture, luisSettings.Endpoint.OriginalString, query);

            string result = string.Empty;
            using (var response = await httpClientService.GetAsync(new Uri(uri)))
            {
                result = response.Content.ReadAsStringAsync().Result;
            }

            return result;
        }
    }
}

