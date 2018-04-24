using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Api.Models;
using Access2Justice.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;

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

            var cosmosDb = new CosmosDbService.CosmosDbService<CuratedExperience>(
                _documentClient, _configurationManager);

            var itesm = await cosmosDb.GetItemsAsync(x => x.id == "46350752-a33d-3454-2e8d-e1045d554d41");

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
