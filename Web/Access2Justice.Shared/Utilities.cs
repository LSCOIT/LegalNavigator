using Newtonsoft.Json;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Access2Justice.Shared
{
    public class Utilities
    {
        public static JsonSerializerSettings JSONSanitizer()
        {
            //Ignoring Null and Default values, Default values which are set at model.
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
        }

        public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in hash)
            {
                result.Append(item.ToString("X2", CultureInfo.InvariantCulture));
            }
            return result.ToString();
        }
    }
}
