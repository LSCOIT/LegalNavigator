using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.AdapterFunction
{
    public class AdapterSettings
    {
        //public AdapterSettings(IConfiguration configuration)
        //{
        //    try
        //    {
        //        AdapterApiUrl = new Uri(configuration.GetSection("AdapterApiUrl").Value);
        //    }
        //    catch
        //    {
        //        throw new Exception("Invalid Application configurations");
        //    }
        //}

        public Uri AdapterApiUrl { get; set; }
    }
}
