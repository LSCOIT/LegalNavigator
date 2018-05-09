﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Access2Justice.Repositories.Interface;
using Access2Justice.Repositories.Models;
using Access2Justice.CosmosDb;
using Access2Justice.Shared.Interfaces;
using Microsoft.Azure.Documents;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
   
    public class TopicsController : Controller
    {
       
        ITopicRepository<TopicModel, string> _repo;
        public TopicsController(ITopicRepository<TopicModel, string> r)
        {
            _repo = r;
        }
        [HttpGet]
        [Route("api/Topics/Get")]
        public IActionResult Get()
        {
            var topics = _repo.GetTopicsFromCollectionAsync().Result;
            return Ok(topics);
        }

        [HttpGet]
        [Route("api/Topics/GetSubTopics/{id}")]
        public IActionResult Get(string id)
        {
            var topics = _repo.GetTopicsFromCollectionAsync(id).Result;
            return Ok(topics);
        }
    
    }
}
