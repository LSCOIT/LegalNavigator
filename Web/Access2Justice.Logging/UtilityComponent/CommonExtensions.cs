
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace Access2Justice.Logger.UtilityComponent
{
    /// <summary>
    /// A place to consolidate common routines which should not be peppered through code
    /// </summary>
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
        /// Converts string to Int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            return Int32.Parse(str);
        }

        /// <summary>
        /// Converts string (true/false) to bool
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ToBool(this string str)
        {
            return bool.Parse(str);
        }

        /// <summary>
        /// Converts minutes to milliseconds
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int MinutesToMs(this int value)
        {
            return value * 60 * 1000;
        }

        /// <summary>
        /// Invariants the format.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static string InvariantFormat(this string str, params object[] args)
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, str, args);
        }

        /// <summary>
        /// Invariant equals ignoring case
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool InvariantEquals(this string str, string value)
        {
            return string.Equals(str, value, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Converts to enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static T ConvertToEnum<T>(this string instance) where T : struct, IConvertible
        {
            if (typeof(T).IsEnum == false) throw new ArgumentException(Error_OnlyEnumSupported);

            try
            {
                return (T)Enum.Parse(typeof(T), instance, true);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, Error_EnumValueNotFound, instance, typeof(T).Name, typeof(T).FullName), ex);
            }
        }

        /// <summary>
        /// Converts to enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="defaultValue">The value the converted enum will take upon parse failure.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static T ConvertToEnum<T>(this string instance, T defaultValue) where T : struct, IConvertible
        {
            if (typeof(T).IsEnum == false) throw new ArgumentException(Error_OnlyEnumSupported);

            try
            {
                if (string.IsNullOrEmpty(instance) == true) return defaultValue;
                return (T)Enum.Parse(typeof(T), instance, true);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

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
        /// Sets the event identifier in the exception.Data container
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="eventId">The event identifier.</param>
        public static void SetEventId(this Exception ex, int eventId)
        {
            if ((ex.Data[Key_EventId] as int?).HasValue)
            {
                ex.Data[Key_EventId] = eventId;
            }
            else
            {
                ex.Data.Add(Key_EventId, eventId);
            }
        }

        /// <summary>
        /// Addx509s the name claim.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="value">The value.</param>
        public static void Addx509NameClaim(this ClaimsIdentity identity, string value)
        {
            identity.AddClaim(new Claim(Key_x509NameClaimType, value));
        }

        /// <summary>
        /// Gets the X509 name claim.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns></returns>
        public static string GetX509NameClaim(this ClaimsIdentity identity)
        {
            if (identity == null) return null;
            return identity.Claims.FirstOrDefault(c => c.Type == Key_x509NameClaimType)?.Value;
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

        /// <summary>
        /// Serialize object useing the json formatter.
        /// </summary>
        public static string ToJson(this object item)
        {
            if (item == null) return null;
            return Newtonsoft.Json.JsonConvert.SerializeObject(item);
        }

        /// <summary>
        /// Deserializes json string
        /// </summary>
        public static T FromJsonString<T>(this string json)
        {
            if (json == null) return default(T);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Serializes object as a byte array
        /// </summary>
        public static byte[] ToJsonBytes(this object item)
        {
            return Encoding.UTF8.GetBytes(item.ToJson());
        }

        /// <summary>
        /// Deserialize buffer to type T
        /// </summary>
        public static T FromJsonBytes<T>(this byte[] buffer)
        {
            return Encoding.UTF8.GetString(buffer).FromJsonString<T>();
        }
        public static string GetDescendingUtcTicks(this DateTime dateTime)
        {
            return (DateTime.MaxValue.Ticks - dateTime.Ticks).ToString();
        }

        /// <summary>
        /// Encodes guid to base64, 22 chars long
        /// </summary>
        public static string ToShortGuid(this Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray()).Replace('/', '_').Replace('+', '-').Substring(0, 22);
        }

        /// <summary>
        /// Returns resource for key, if not found, returns the key
        /// </summary>
        public static string SafeGetString(this ResourceManager mgr, string key)
        {
            string content = mgr.GetString(key);
            return content ?? key;
        }


        public static string GetUniqueKey()
        {
            int maxSize = 32;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length)]); }
            return result.ToString();

        }

    }
}