namespace Access2Justice.Shared
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class HttpClientService : IHttpClientService
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

        public async Task<HttpResponseMessage> GetAsync(string apiUrl)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.GetAsync(apiUrl);
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
                response = await httpClient.PostAsync(apiUrl, httpContent);
            }
            catch (Exception ex)
            {
                //TO DO : Need to implement exception logging..
            }
            return response;
        }
    }
}
