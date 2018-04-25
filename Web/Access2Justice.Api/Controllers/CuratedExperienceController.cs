using Access2Justice.Api.Models.CuratedExperience;
using Access2Justice.Api.ViewModels;
using Access2Justice.CosmosDb;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Route("api/[controller]")]
    public class CuratedExperienceController : Controller
    {
        private readonly IBackendDatabaseService _backendDatabaseService;

        public CuratedExperienceController(IBackendDatabaseService backendDatabaseService)
        {
            _backendDatabaseService = backendDatabaseService;
        }

        [HttpGet]
        public async Task<CuratedExperienceChoiceSet> Get([FromQuery] string id)
        {
            var choiceSet = new CuratedExperienceChoiceSet();
            var choice = new List<Choice>();


            var children = new List<string>();
            children.Add("ffa0141d-ff2a-4f39-90eb-fbbb2f71e482");

            var survay = new SurvayTree();
            survay.ChoiceSetId = "b5269ec5-680f-4d8a-b0f9-779f7c9cfcb4";
            survay.Description = "are you a 'male' or 'female'?";
            survay.ParentId = "self";
            survay.Childern = children;

            var curatedExperience = new CuratedExperience();
            curatedExperience.CuratedExperienceId = "19a0c939-9635-40a5-a33c-c441fdb3f26f";
            curatedExperience.Name = "Child Custody";
            curatedExperience.SurvayTree.Add(survay);

            var answers = new Dictionary<Guid, string>();
            answers.Add(Guid.Parse("bfeee8b1-add7-4900-8726-7d758e8d0614"), "male");
            answers.Add(Guid.Parse("1e3d78ce-8d5b-4c57-bdc4-0c08e35d6e3c"), "yes i'm in military serivce");


            await _backendDatabaseService.CreateItemAsync(curatedExperience);
            await _backendDatabaseService.CreateItemAsync(new CuratedExperienceAnswersMapper
            {
                CuratedExperienceId = Guid.Parse("d7da741a-9844-40fc-9128-baff482ed505"),
                Answers = answers
            });


            var retrivedCuratedExperience = await _backendDatabaseService.GetItemAsync<CuratedExperience>("be6b65aa-1c27-4aed-bf22-418b33a74878");
            var retrivedAnswer = await _backendDatabaseService.GetItemAsync<CuratedExperienceAnswersMapper>("ffa0141d-ff2a-4f39-90eb-fbbb2f71e482");

            return choiceSet;
        }
    }
}