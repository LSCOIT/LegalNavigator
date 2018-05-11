﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Access2Justice.CosmosDb;
using Access2Justice.Shared.Interfaces;
using Microsoft.Azure.Documents;
using System.Linq.Expressions;
using Access2Justice.Shared;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
   
    public class TopicsController : Controller
    {
        IBackendDatabaseService backendDataBaseService; 
        public TopicsController(IBackendDatabaseService backendDataBaseService)
        {
            this.backendDataBaseService = backendDataBaseService;          
        } 
        
        [HttpGet]
        [Route("api/topics/get")]
        public IActionResult Get()
        {
            var topics = backendDataBaseService.GetItemsAsync<TopicModel>(a => a.type == "topic" && a.parentId == "");         
            return Ok(topics);                     
        }
        [HttpGet]
        [Route("api/topics/getsubtopics/{id}")]
        public IActionResult Get(string id)
        {
            var topics = backendDataBaseService.GetTopicsFromCollectionAsync(id).Result;
            return Ok(topics);
        }
        [HttpGet]
        [Route("api/topics/getsubtopicdetails/{id}")]
        public IActionResult getsubtopicdetails()
        {
            return null;
        }
    }
}
