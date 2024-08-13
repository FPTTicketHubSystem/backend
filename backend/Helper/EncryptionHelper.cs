using System.Text;

namespace backend.Helper
{
    public class EncryptionHelper
    {
        public static string EncryptEmail(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string DecryptEmail(string encodedText)
        {
            var base64EncodedBytes = Convert.FromBase64String(encodedText);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
