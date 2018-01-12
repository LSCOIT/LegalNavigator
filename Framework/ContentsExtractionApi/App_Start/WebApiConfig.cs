using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using ContentsExtractionApi.Controllers;

namespace ContentsExtractionApi
{
    public static class WebApiConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // New code
            config.EnableCors();

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static HttpConfiguration mapRoutes(HttpConfiguration config)
        { 

         // Web API routes

         config.MapHttpAttributeRoutes();

         config.Routes.MapHttpRoute(

             name: "DefaultApi",

             routeTemplate: "api/{controller}/{id}",

             defaults: new { id = RouteParameter.Optional  }

         );

         //Binary JSON Formatter

       

            config.Formatters.Add(new BsonMediaTypeFormatter());
            config.Formatters.Add(new BrowserJsonFormatter());
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/png"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml" || t.MediaType == "text/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);


            //var json = config.Formatters.JsonFormatter;
            //json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            //config.Formatters.Remove(config.Formatters.XmlFormatter);
            //json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            return config;

     }

    }
}
