namespace Access2Justice.Shared
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class HttpClientService : IHttpClientService, IDisposable
    {
        /// <summary>
        /// The http client.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// /// Initializes a new instance of the HttpClientService class.
        /// </summary>
        public HttpClientService()
        {
            httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetAsync(Uri apiUrl)
        {
            return await httpClient.GetAsync(apiUrl);
        }

        public async Task<HttpResponseMessage> PostAsync(Uri apiUrl, HttpContent httpContent)
        {
            return await httpClient.PostAsync(apiUrl, httpContent);
        }

        public async Task<HttpResponseMessage> GetDataAsync(Uri apiUrl, string subscriptionKey)
        {
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            return await httpClient.GetAsync(apiUrl);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await httpClient.SendAsync(request);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                httpClient.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
