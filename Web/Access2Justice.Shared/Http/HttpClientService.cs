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
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.GetAsync(apiUrl);
            }
            catch (Exception ex)
            {
                //TO DO : Need to implement exception logging..
                Console.WriteLine(ex);
            }
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(Uri apiUrl, HttpContent httpContent)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.PostAsync(apiUrl, httpContent);
            }
            catch (Exception ex)
            {
                //TO DO : Need to implement exception logging..
                Console.WriteLine(ex);
            }
            return response;
        }

        public async Task<HttpResponseMessage> GetDataAsync(Uri apiUrl,string subscriptionKey)
        {
            HttpResponseMessage response = null;
            try
            {
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                response = await httpClient.GetAsync(apiUrl);
            }
            catch (Exception ex)
            {
                //TO DO : Need to implement exception logging..
                Console.WriteLine(ex);
            }
            return response;
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
