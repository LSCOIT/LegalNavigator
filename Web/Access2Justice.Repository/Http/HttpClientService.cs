namespace Access2Justice.Repository
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class HttpClientService : IHttpClientService
    {
        /// <summary>
        /// The _http client.
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// /// Initializes a new instance of the HttpClientService class.
        /// </summary>
        public HttpClientService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string apiUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.GetAsync(apiUrl);
            }
            catch (Exception ex)
            {
                //TO DO : Need to implement exception logging..
            }
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string apiUrl, HttpContent httpContent)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.PostAsync(apiUrl, httpContent);
            }
            catch (Exception ex)
            {
                //TO DO : Need to implement exception logging..
            }
            return response;
        }
    }
}
