﻿namespace Access2Justice.Api.Controllers
{
    using System;    
    using System.Threading.Tasks;
    using Access2Justice.Repository;    
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;


    [Produces("application/json")]
    [Route("api/IntentsMapper")]
    public class IntentsMapperController : Controller
    {

        private IOptions<App> _appSettings;
        private ILUISHelper _luisHelper;

        public IntentsMapperController(IOptions<App> appSettings, ILUISHelper luisHelper)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _luisHelper = luisHelper ?? throw new ArgumentNullException(nameof(luisHelper));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Json("");
        }

    }
}