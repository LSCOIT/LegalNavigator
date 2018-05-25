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
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<HttpResponseMessage> PostAsync(Uri apiUrl, HttpContent httpContent);

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
