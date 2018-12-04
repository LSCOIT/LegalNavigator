using Access2Justice.Integration.Models;
using Access2Justice.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Access2Justice.Integration
{
    public class AdapterFactory
    {
        private readonly IHttpClientService httpClient;

        public AdapterFactory(IHttpClientService httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<dynamic> CallServiceProvider(AdapterSettings adapterSettings)
        {
            var instance = Activator.CreateInstance(Type.GetType(adapterSettings.Namespace));
            Type type = instance.GetType();
            var spResult = await (dynamic) type.GetMethod(adapterSettings.MethodName).Invoke(instance, new object[] { adapterSettings.AdapterDetails, httpClient });
            return spResult;
        }
    }
}
