using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Adaptees
{
    public class TopicAdaptee
    {
        private readonly IRtmSettings rtmSettings;
        private readonly IWebSearchBusinessLogic webSearchBusinessLogic;

        public async Task<IEnumerable<Topic>> GetTopics(string organizationalUnit)
        {
            string rtmApi = "Topics"; // RTM API name for topics is required
            string rtmSessionId = "1ZWQL0mwagl6hB9yCyGV"; // await GetSessionId().ConfigureAwait(false);
            string otherParams = "st=c,catid=364,sn='',zip=99504,county=''"; // hardcoded for demo 
            var uri = string.Format(CultureInfo.InvariantCulture, rtmSettings.RtmApiUrl.OriginalString, rtmApi, rtmSettings.RtmApiKey, otherParams, rtmSessionId);
            dynamic response = await webSearchBusinessLogic.SearchWebResourcesAsync(new Uri(uri)).ConfigureAwait(false);
            List<Topic> resources = JsonUtilities.DeserializeDynamicObject<List<Topic>>(response);
            var topicList = resources.Where(x => x.OrganizationalUnit == organizationalUnit);
            return topicList;
        }
    }
}
