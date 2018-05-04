using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Access2Justice.CosmosDbService;
using Access2Justice.CosmosDbService.Models;
using Microsoft.AspNetCore.Cors;
using Access2Justice.Repositories.Interface;
//using Access2Justice.Repositories.Implement;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    //[Route("api/Topics")]
    [EnableCors("AllowAll")]
    public class TopicsController : Controller
    {


        //ITopicRepository _repo;
        //public TopicsController(ITopicRepository r)
        //{
        //    _repo = r;
        //}
      


        //[HttpGet]
        //[Route("api/Topics/Get")]
        //public IActionResult Get()
        //{
        //    var persons = _repo.GetTopicsFromCollectionAsync().Result;
        //    return Ok(persons);
        //}

        //[HttpGet]
        //[Route("api/Topics/Get/{id}")]
        //public IActionResult Get(string id)
        //{
        //    var person = _repo.GetTopicsFromCollectionAsync(id).Result;
        //    return Ok(person);
        //}


        Class1 cls = new Class1();
        [Route("api/Topics/Get")]
        [HttpGet]
        public List<TopicModel> Get()
        {
            return cls.GetTopicsList();
        }

        [Route("api/topics/getcontent")]
        [HttpGet]
        public SubjectModel GetContent(string name) 
        {
            return cls.GetContentsList(name);
        }
        //public Overview GetContent(int id)
        //{
        //    return bal.GetContentsList(id);
        //}

    }
}