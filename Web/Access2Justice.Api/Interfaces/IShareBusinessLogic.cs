﻿using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IShareBusinessLogic
    {
        Task<ShareViewModel> ShareResourceDataAsync(ShareInput shareInput);
        Task<ShareViewModel> CheckPermaLinkDataAsync(ShareInput shareInput);
        Task<object> UnshareResourceDataAsync(ShareInput shareInput);
        Task<object> GetPermaLinkDataAsync(string permaLink);
    }
}
