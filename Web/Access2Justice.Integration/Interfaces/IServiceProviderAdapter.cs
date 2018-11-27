﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Access2Justice.Shared.Models.Integration;

namespace Access2Justice.Integration.Interfaces
{
    public interface IServiceProviderAdapter
    {
        Task<List<ServiceProvider>> GetServiceProviders(string TopicName);

        //Task<List<ServiceProvider>> GetServiceProviders(dynamic TopicName);

        ServiceProvider GetServiceProviderDetails(string id);
    }
}