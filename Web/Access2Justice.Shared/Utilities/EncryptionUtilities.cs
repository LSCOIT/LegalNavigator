using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Access2Justice.Shared.Utilities
{
    public class EncryptionUtilities
    {
        public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        public static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
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
