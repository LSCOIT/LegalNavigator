﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IUserProfileBusinessLogic
    {
        Task<dynamic> GetUserProfileDataAsync(string oId);       
        Task<object> CreateUserProfileDataAsync(dynamic resource);
    }
}