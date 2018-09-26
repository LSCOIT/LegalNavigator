using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Interfaces;
using Access2Justice.CosmosDb;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Authorization
{
    public class AuthorizeUser : IAuthorizationRequirement
    {
        //public string OId { get; internal set; }
        //public string Role { get; private set; }

        //public AuthorizeUser(string role)
        //{
        //    OId = string.Empty;
        //    Role = role;
        //}
    }
}
