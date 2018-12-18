namespace Access2Justice.Shared
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IHttpClientService
    {
        /// <summary>
        /// The post async.
        /// </summary>
        /// <param name="apiUrl">
        /// The request uri.
        /// </param>
        /// <param name="httpContent">
        /// HttpContent to be POSTed.
        /// </param>
        /// <param name="subscriptionKey">
        /// 
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<HttpResponseMessage> PostAsync(Uri apiUrl, HttpContent httpContent, string subscriptionKey);

        /// <summary>
        /// The get async.
        /// </summary>
        /// <param name="apiUrl">
        /// The request uri.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<HttpResponseMessage> GetAsync(Uri apiUrl);

        Task<HttpResponseMessage> GetDataAsync(Uri apiUrl, string subscriptionKey);

    }
}
