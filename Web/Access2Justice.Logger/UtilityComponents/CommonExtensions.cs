
using System;
using System.Collections.Generic;


namespace Access2Justice.Logger.Utility
{
    public static class CommonExtensions
    {
        private const string Error_EnumValueNotFound = "Cannot convert string value '{0}' into Enum of type {1}.  {0} is not defined in Enum {2}.";
        private const string Error_OnlyEnumSupported = "Only Enum types are supported.";
        private const int Const_DefualtEventId = 100;
        private const string Const_FullCertUserName = "{0}, {1}";
        private const string Key_EventId = "EventId";
        private const string Key_x509NameClaimType = "X509SubjectName";
        private const string Key_HttpRequestMessage = "MS_HttpRequestMessage";
        private const string Const_EventIdKey = "EventId";
        private const string Const_CorelationId = "CorelationId";

             
        /// <summary>
        /// Gets the event identifier from the exception.Data container
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static int GetEventId(this Exception ex)
        {
            int? eventId = ex.Data[Key_EventId] as int?;
            if (eventId.HasValue == false) return Const_DefualtEventId;
            return eventId.Value;
        }    

              
        /// <summary>
        /// Adds corelationId property - used in AI property bag
        /// </summary>
        public static void AddCorelationId(this IDictionary<string, string> properties, string corelationId)
        {
            properties.Add(Const_CorelationId, corelationId);
        }

        /// <summary>
        /// Gets the current HTTP request message.
        /// </summary>
        /// <returns></returns>
        // public static HttpRequestMessage GetCurrentHttpRequestMessage()
        //  {
        //     return System.Web.HttpContext.Current?.Items[Key_HttpRequestMessage] as HttpRequestMessage;
        //  }

        /// <summary>
        /// Adds eventId property - used in AI property bag
        /// </summary>
        public static void AddEventId(this IDictionary<string, string> properties, int eventId)
        {
            properties.Add(Const_EventIdKey, eventId.ToString());
        }    
       
    }
}

