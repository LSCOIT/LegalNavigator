using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Interfaces
{
    public interface IRtmSettings
    {
        Uri RtmApiUrl { get; }        
        string RtmApiKey { get; }

    }
}
