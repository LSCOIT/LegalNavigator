using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IDocumentClient _documentClient;

        public ValuesController(IConfigurationManager configurationManager, IDocumentClient documentClient)
        {
            _configurationManager = configurationManager;
            _documentClient = documentClient;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
