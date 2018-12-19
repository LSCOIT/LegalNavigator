using Access2Justice.Shared;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Access2Justice.Api.Tests.TestData
{
    class AdminBusinessLogicTestData
    {
        public static IEnumerable<object[]> CuratedTemplateContent()
        {
            yield return new object[] { CuratedTemplateDetails, CuratedTemplateDetails };
        }

        public static CuratedTemplate CuratedTemplateDetails =>
            new CuratedTemplate
            {
                Name = "TestTemplate",
                Description = "Test Template Description",
                TemplateFile = new List<IFormFile>() { }
            };

    }

}
