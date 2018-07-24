﻿using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Access2Justice.Api.Controllers
{
    [Route("api/[controller]")]
    public class CuratedExperienceController : Controller
    {
        private readonly IA2JAuthorBusinessLogic a2jAuthorBuisnessLogic;
        private readonly ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic;
        private readonly IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogic;

        public CuratedExperienceController(IA2JAuthorBusinessLogic a2jAuthorBuisnessLogic, ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic,
            IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogic)
        {
            this.a2jAuthorBuisnessLogic = a2jAuthorBuisnessLogic;
            this.curatedExperienceBusinessLogic = curatedExperienceBusinessLogic;
            this.personalizedPlanBusinessLogic = personalizedPlanBusinessLogic;
        }

        #region DEMO
        /// <summary>
        /// This endpoint is just to demo the Guided Assistance. It is a sample schema imported from A2J Author.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Demo")]
        public IActionResult GetDemoSchema()
        {
            var json = "{\r\n  \"id\": \"f3daa1e4-5f20-47ce-ab6d-f59829f936fe\",\r\n  \"title\": \"Sample A2J Guided Interview (for Sample Exercise)\",\r\n  \"subjectAreas\": [\r\n    \"Jurisdiction of my interview\"\r\n  ],\r\n  \"description\": \"TESTING This A2J Guided Interview was created for the purposes of creating screenshots for a A2J Author and HotDocs sample exercise file.\",\r\n  \"version\": \"8-17-2015\",\r\n  \"authors\": [\r\n    {\r\n      \"name\": \"Jessica Frank\",\r\n      \"title\": \"A2J Author Program Coordinator\",\r\n      \"organization\": \"IIT Chicago-Kent College of Law\",\r\n      \"email\": \"jbolack@kentlaw.iit.edu\"\r\n    },\r\n    {\r\n      \"name\": \"Alaa Tadmori\",\r\n      \"title\": \"Testing A2J Author\",\r\n      \"organization\": \"Avanade\",\r\n      \"email\": \"alaa@xserver.com\"\r\n    }\r\n  ],\r\n  \"components\": [\r\n    {\r\n      \"id\": \"0173c6ca-2ba7-4d1d-94f5-7e3fec1f9dfc\",\r\n      \"name\": \"1-Introduction\",\r\n      \"text\": \"This is the introduction. This A2J Guided Interview was created as part of a sample exercise. It is not intended for use by the general public.\",\r\n      \"learn\": \"do you want to know more about this subject?\",\r\n      \"help\": \"this is just a sample question, feel free to fill any test data you want.\",\r\n      \"parentId\": \"00000000-0000-0000-0000-000000000000\",\r\n      \"tags\": [],\r\n      \"buttons\": [\r\n        {\r\n          \"id\": \"f3a1e369-edba-4152-bbc7-a52e8e77b56e\",\r\n          \"label\": \"Continue\",\r\n          \"destination\": \"1-Name\"\r\n        }\r\n      ],\r\n      \"fields\": []\r\n    },\r\n    {\r\n      \"id\": \"4adec03b-4f9b-4bc9-bc44-27a8e84e30ae\",\r\n      \"name\": \"1-Name\",\r\n      \"text\": \"Enter your name.\",\r\n      \"learn\": \"\",\r\n      \"help\": \"\",\r\n      \"parentId\": \"00000000-0000-0000-0000-000000000000\",\r\n      \"tags\": [],\r\n      \"buttons\": [\r\n        {\r\n          \"id\": \"2b92e07b-a555-48e8-ad7b-90b99ebc5c96\",\r\n          \"label\": \"Continue\",\r\n          \"destination\": \"2-Gender\"\r\n        }\r\n      ],\r\n      \"fields\": [\r\n        {\r\n          \"id\": \"22cbf2ac-a8d3-48c5-b230-297111e0e85c\",\r\n          \"type\": \"text\",\r\n          \"label\": \"First:\",\r\n          \"isRequired\": true,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"You must type a response in the highlighted space before you can continue.\"\r\n        },\r\n        {\r\n          \"id\": \"6c8312eb-131d-4cfb-a542-0e3f6d07a1d3\",\r\n          \"type\": \"text\",\r\n          \"label\": \"Middle:\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"You must type a response in the highlighted space before you can continue.\"\r\n        },\r\n        {\r\n          \"id\": \"d2a935b4-bb07-494f-9c59-f3115b19d002\",\r\n          \"type\": \"text\",\r\n          \"label\": \"Last:\",\r\n          \"isRequired\": true,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"You must type a response in the highlighted space before you can continue.\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"id\": \"29d06a1d-0898-439e-b208-2774d523ea0a\",\r\n      \"name\": \"2-Gender\",\r\n      \"text\": \"Choose your gender.\",\r\n      \"learn\": \"Why are you asking this question?\",\r\n      \"help\": \"This question is used to populate the avatar that will represent you through the rest of this A2J Guided Interview.\u00A0\",\r\n      \"parentId\": \"00000000-0000-0000-0000-000000000000\",\r\n      \"tags\": [],\r\n      \"buttons\": [\r\n        {\r\n          \"id\": \"1d34317b-4b2e-4de4-92c0-61407d0b70da\",\r\n          \"label\": \"Continue\",\r\n          \"destination\": \"3-Address\"\r\n        }\r\n      ],\r\n      \"fields\": [\r\n        {\r\n          \"id\": \"1a843edf-1c20-4f0f-96c5-fbcae98b381a\",\r\n          \"type\": \"gender\",\r\n          \"label\": \"Gender:\",\r\n          \"isRequired\": true,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"id\": \"1ba7a445-7a2b-4d7b-b043-296bffef402a\",\r\n      \"name\": \"3-Address\",\r\n      \"text\": \"%%[Client first name TE]%% what is your address?&nbsp;\",\r\n      \"learn\": \"\",\r\n      \"help\": \"\",\r\n      \"parentId\": \"00000000-0000-0000-0000-000000000000\",\r\n      \"tags\": [],\r\n      \"buttons\": [\r\n        {\r\n          \"id\": \"cafc4de0-91c5-4f67-a59f-bbffc2534236\",\r\n          \"label\": \"Continue\",\r\n          \"destination\": \"4-Phone number\"\r\n        }\r\n      ],\r\n      \"fields\": [\r\n        {\r\n          \"id\": \"78da510a-180c-4757-b75d-fd87f470ae76\",\r\n          \"type\": \"text\",\r\n          \"label\": \"Street\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        },\r\n        {\r\n          \"id\": \"6a81a78e-7692-4256-88d6-651dc4e9ce0f\",\r\n          \"type\": \"text\",\r\n          \"label\": \"City\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        },\r\n        {\r\n          \"id\": \"3a6180c3-d42c-400d-a2a7-d9b556a454af\",\r\n          \"type\": \"textpick\",\r\n          \"label\": \"State\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        },\r\n        {\r\n          \"id\": \"9471103f-7868-47fd-b1ab-581cc3934493\",\r\n          \"type\": \"numberzip\",\r\n          \"label\": \"Zip code\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"id\": \"3f2e2cdd-006e-4f4c-b0bf-32cea08f8e90\",\r\n      \"name\": \"4-Phone number\",\r\n      \"text\": \"What is your phone number?\",\r\n      \"learn\": \"\",\r\n      \"help\": \"\",\r\n      \"parentId\": \"00000000-0000-0000-0000-000000000000\",\r\n      \"tags\": [],\r\n      \"buttons\": [\r\n        {\r\n          \"id\": \"aa3777d0-e7dd-48dd-afb3-6d61fc172768\",\r\n          \"label\": \"Continue\",\r\n          \"destination\": \"1-Marital Status\"\r\n        }\r\n      ],\r\n      \"fields\": [\r\n        {\r\n          \"id\": \"d63e24a3-4a4d-4fec-a1aa-9c72b2fcab5e\",\r\n          \"type\": \"numberphone\",\r\n          \"label\": \"Phone number\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"id\": \"73464e7e-5c8a-4d96-9a0e-6f1f28592eb9\",\r\n      \"name\": \"1-Marital Status\",\r\n      \"text\": \"What is your marital status?\",\r\n      \"learn\": \"\",\r\n      \"help\": \"\",\r\n      \"parentId\": \"00000000-0000-0000-0000-000000000000\",\r\n      \"tags\": [],\r\n      \"buttons\": [\r\n        {\r\n          \"id\": \"5846cabb-1826-47bc-b2cd-6168f78fc4f2\",\r\n          \"label\": \"Continue\",\r\n          \"destination\": \"2-Have children?\"\r\n        }\r\n      ],\r\n      \"fields\": [\r\n        {\r\n          \"id\": \"0bc5ccb7-e1d2-40b6-b20c-4fbd6cb85de9\",\r\n          \"type\": \"radio\",\r\n          \"label\": \"Married\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        },\r\n        {\r\n          \"id\": \"93d7a9f7-a33e-4587-a04e-e57638b77e73\",\r\n          \"type\": \"radio\",\r\n          \"label\": \"Single\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        },\r\n        {\r\n          \"id\": \"1d292f71-312e-4308-b22b-cad39cc7e9be\",\r\n          \"type\": \"radio\",\r\n          \"label\": \"Divorced\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        },\r\n        {\r\n          \"id\": \"289d456e-698b-431d-b0ec-4e82d9d110a4\",\r\n          \"type\": \"radio\",\r\n          \"label\": \"Widowed\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"id\": \"d81340c4-b93a-46c1-8176-fcf8b49a9f21\",\r\n      \"name\": \"2-Have children?\",\r\n      \"text\": \"Do you have children?\",\r\n      \"learn\": \"\",\r\n      \"help\": \"\",\r\n      \"parentId\": \"00000000-0000-0000-0000-000000000000\",\r\n      \"tags\": [],\r\n      \"buttons\": [\r\n        {\r\n          \"id\": \"16402097-d668-486d-99ed-f1b26c9510a7\",\r\n          \"label\": \"Yes\",\r\n          \"destination\": \"3-How many children?\"\r\n        },\r\n        {\r\n          \"id\": \"f7534f78-5b33-4012-a4a6-16cf49c89ee6\",\r\n          \"label\": \"No\",\r\n          \"destination\": \"1-Purpose of form\"\r\n        }\r\n      ],\r\n      \"fields\": []\r\n    },\r\n    {\r\n      \"id\": \"1d2e7c41-f7d1-46da-bd3e-7bfc10a1678b\",\r\n      \"name\": \"3-How many children?\",\r\n      \"text\": \"How many children do you have?\u00A0\",\r\n      \"learn\": \"\",\r\n      \"help\": \"\",\r\n      \"parentId\": \"00000000-0000-0000-0000-000000000000\",\r\n      \"tags\": [],\r\n      \"buttons\": [\r\n        {\r\n          \"id\": \"f631fad5-bd39-4982-8881-e81d25cbe664\",\r\n          \"label\": \"Continue\",\r\n          \"destination\": \"4-Child's name and birthdate\"\r\n        }\r\n      ],\r\n      \"fields\": [\r\n        {\r\n          \"id\": \"c0e22b4a-4937-4f53-b880-b202700fb2e6\",\r\n          \"type\": \"numberpick\",\r\n          \"label\": \"Number of children\",\r\n          \"isRequired\": false,\r\n          \"min\": \"1\",\r\n          \"max\": \"4\",\r\n          \"invalidPrompt\": \"\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"id\": \"2247da03-da30-4cbb-b24f-2132f4f04485\",\r\n      \"name\": \"1-The End\",\r\n      \"text\": \"Congratulations! You have finished your A2J Guided Interview. Click Get My Document to print your completed form.\u00A0\",\r\n      \"learn\": \"\",\r\n      \"help\": \"\",\r\n      \"parentId\": \"00000000-0000-0000-0000-000000000000\",\r\n      \"tags\": [],\r\n      \"buttons\": [\r\n        {\r\n          \"id\": \"0956d97e-3fcd-4383-bb25-45c2a138ceb4\",\r\n          \"label\": \"Get My Document\",\r\n          \"destination\": \"SUCCESS\"\r\n        }\r\n      ],\r\n      \"fields\": []\r\n    },\r\n    {\r\n      \"id\": \"ff6fb136-1d62-4330-a82e-242e592d9d59\",\r\n      \"name\": \"1-Purpose of form\",\r\n      \"text\": \"Why are you filling out this form?\",\r\n      \"learn\": \"\",\r\n      \"help\": \"\",\r\n      \"parentId\": \"00000000-0000-0000-0000-000000000000\",\r\n      \"tags\": [],\r\n      \"buttons\": [\r\n        {\r\n          \"id\": \"98613482-53ad-458f-988c-5b473358c7bf\",\r\n          \"label\": \"Continue\",\r\n          \"destination\": \"1-The End\"\r\n        }\r\n      ],\r\n      \"fields\": [\r\n        {\r\n          \"id\": \"cfd6fd1d-2974-43ef-8e3c-6fb0e9a0d8cf\",\r\n          \"type\": \"textlong\",\r\n          \"label\": \"\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"id\": \"32e9dd47-5b06-4db2-a297-054436add52f\",\r\n      \"name\": \"4-Child's name and birthdate\",\r\n      \"text\": \"What is your %%ORDINAL(ChildCount)%% child's name and date of birth?&nbsp;\",\r\n      \"learn\": \"\",\r\n      \"help\": \"\",\r\n      \"parentId\": \"00000000-0000-0000-0000-000000000000\",\r\n      \"tags\": [],\r\n      \"buttons\": [\r\n        {\r\n          \"id\": \"7d6b2ee0-6b30-4011-b99e-0d64c16217c8\",\r\n          \"label\": \"Continue\",\r\n          \"destination\": \"\"\r\n        }\r\n      ],\r\n      \"fields\": [\r\n        {\r\n          \"id\": \"bf13a106-37b3-4e8f-bd9f-4c31a8a8d2c2\",\r\n          \"type\": \"text\",\r\n          \"label\": \"Child's first name:\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        },\r\n        {\r\n          \"id\": \"50dbe540-24c8-4f5e-9765-770d675f6240\",\r\n          \"type\": \"datemdy\",\r\n          \"label\": \"Date of birth:\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";
            return Content(JsonConvert.DeserializeObject(json).ToString());
        }

        /// <summary>
        /// This endpoint is just to demo the Curated Experience. It is a sample component payload for the UI.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Component/Demo")]
        public IActionResult GetDemoComponent()
        {
            var json = "    {\r\n      \"id\": \"1ba7a445-7a2b-4d7b-b043-296bffef402a\",\r\n      \"name\": \"3-Address\",\r\n      \"text\": \"%%[Client first name TE]%% what is your address?&nbsp;\",\r\n      \"learn\": \"The UI will prompt the user to learn more.\",\r\n      \"help\": \"The UI will show help on this item.\",\r\n   \"tags\": [],\r\n      \"buttons\": [\r\n        {\r\n          \"id\": \"cafc4de0-91c5-4f67-a59f-bbffc2534236\",\r\n          \"label\": \"Continue\",\r\n          \"destination\": \"4-Phone number\"\r\n        }\r\n      ],\r\n      \"fields\": [\r\n        {\r\n          \"id\": \"78da510a-180c-4757-b75d-fd87f470ae76\",\r\n          \"type\": \"text\",\r\n          \"label\": \"Street\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        },\r\n        {\r\n          \"id\": \"6a81a78e-7692-4256-88d6-651dc4e9ce0f\",\r\n          \"type\": \"text\",\r\n          \"label\": \"City\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        },\r\n        {\r\n          \"id\": \"3a6180c3-d42c-400d-a2a7-d9b556a454af\",\r\n          \"type\": \"textpick\",\r\n          \"label\": \"State\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        },\r\n        {\r\n          \"id\": \"9471103f-7868-47fd-b1ab-581cc3934493\",\r\n          \"type\": \"numberzip\",\r\n          \"label\": \"Zip code\",\r\n          \"isRequired\": false,\r\n          \"min\": \"\",\r\n          \"max\": \"\",\r\n          \"invalidPrompt\": \"\"\r\n        }\r\n      ]\r\n    }";
            return Content(JsonConvert.DeserializeObject(json).ToString());
        }

        /// <summary>
        /// This endpoint is just to demo the Curated Experience Personalized Plan schema. Added to help building the UI.
        /// </summary>
        /// <returns></returns>
        [HttpGet("PersonalizedPlan/Demo")]
        public IActionResult GetDemoPersonalizedPlan()
        {
            //var locations = new List<PlanLocation>();
            //locations.Add(new PlanLocation
            //{
            //    City = "test city",
            //    State = "test state",
            //    ZipCode = "11332"
            //});
            
            var quicklinks = new List<PlanQuickLink>();
            quicklinks.Add(new PlanQuickLink
            {
                Title = "Quick link to topic1",
                Url = new Uri("http://localhost/topic1")
            });

            //var resources = new List<PlanResource>();
            //resources.Add(new PlanResource
            //{
            //    Description = "Lorem ipsum dolor sit amet, usu soluta aliquid recusabo in, eloquentiam adversarium in vel.",
            //    ExternalUrl = new Uri("http://localhost"),
            //    Icon = "",
            //    IsRecommended = false,
            //    Location = locations,
            //    Name = "Resource Name",
            //    Overview = "Sale oratio tractatos duo et, pri harum senserit mediocritatem an. No eum aliquip menandri.",
            //    ResourceId = Guid.NewGuid(),
            //    Tags = new List<string>() { "Test", "Demo" },
            //    Type = "Organization",
            //});

            var steps = new List<PlanStep>();
            steps.Add(new PlanStep
            {
                StepId = Guid.NewGuid(),
                Title = "Plan title",
                Description = "test plan for building the UI",
                Order = 1,
                IsComplete = false,
                //Resources = resources,
                Type = "PersonalizedPlan"
            });

            var topics = new List<PlanTopic>();
            topics.Add(new PlanTopic
            {
                TopicId = Guid.NewGuid(),
                QuickLinks = quicklinks,
                Steps = steps
            });

            return Ok(new PersonalizedActionPlanViewModel()
            {
                PersonalizedPlanId = Guid.NewGuid(),
                Topics = topics,
            });
        }
        #endregion

        [HttpPost("Import")]
        public IActionResult ConvertA2JAuthorToCuratedExperience([FromBody] JObject a2jSchema)
        {
            try
            {
                JObject.Parse(a2jSchema.ToString());
                return Json(a2jAuthorBuisnessLogic.ConvertA2JAuthorToCuratedExperience(a2jSchema));
            }
            catch
            {
                return BadRequest("The schema you sent does not have a valid json.");
            }
        }

        [HttpGet("Start")]
        public IActionResult GetFirstComponent(Guid curatedExperienceId)
        {
            var component = curatedExperienceBusinessLogic.GetComponent(RetrieveCachedCuratedExperience(curatedExperienceId), Guid.Empty);
            if (component == null) return NotFound();

            return Ok(component);
        }

        [HttpGet("Component")]
        public IActionResult GetSpecificComponent([FromQuery] Guid curatedExperienceId, [FromQuery] Guid componentId)
        {
            var component = curatedExperienceBusinessLogic.GetComponent(RetrieveCachedCuratedExperience(curatedExperienceId), componentId);
            if (component == null) return NotFound();

            return Ok(component);
        }

        [HttpPost("Component/SaveAndGetNext")]
        public async Task<IActionResult> SaveAndGetNextComponent([FromBody] CuratedExperienceAnswersViewModel component)
        {
            var curatedExperience = RetrieveCachedCuratedExperience(component.CuratedExperienceId);
            var document = await curatedExperienceBusinessLogic.SaveAnswers(component);
            if (component == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(curatedExperienceBusinessLogic.GetNextComponent(curatedExperience, component));
        }

        [HttpPost("PersonalizedPlan")]
        public async Task<IActionResult> GeneratePersonalizedPlan([FromQuery] Guid curatedExperienceId, [FromQuery] Guid answersDocId)
        {
            var personalizedPlan = await personalizedPlanBusinessLogic.GeneratePersonalizedPlan(
                RetrieveCachedCuratedExperience(curatedExperienceId), answersDocId);
            if (personalizedPlan == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(personalizedPlan);
        }

        private CuratedExperience RetrieveCachedCuratedExperience(Guid id)
        {
            var cuExSession = HttpContext.Session.GetString(id.ToString());
            if (string.IsNullOrWhiteSpace(cuExSession))
            {
                var rawCuratedExperience = curatedExperienceBusinessLogic.GetCuratedExperience(id).Result;
                HttpContext.Session.SetObjectAsJson(id.ToString(), rawCuratedExperience);
            }

            return HttpContext.Session.GetObjectAsJson<CuratedExperience>(id.ToString());
        }
    }
}