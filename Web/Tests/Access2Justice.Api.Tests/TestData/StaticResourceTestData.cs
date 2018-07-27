using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Api.Tests.TestData
{
    class StaticResourceTestData
    {
        public static JArray userProfile =
                  JArray.Parse(@"[{'id': '4589592f-3312-eca7-64ed-f3561bbb7398',
                    'oId': '709709e7t0r7t96', 'firstName': 'family1.2.1', 'lastName': '5c035d27-2fdb-9776-6236-70983a918431','email': 'f102bfae-362d-4659-aaef-956c391f79de'}]");
    }
}