using System.Security.Cryptography;
using System.Text;

namespace BE.Helpers
{
    public class EncryptionHelper
    {
        public EncryptionHelper()
        {

        }

        public async Task<string> Encrypt(string inputString)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(inputString));
            var encryptor = SHA256.Create();
            var hashBytes = await encryptor.ComputeHashAsync(stream);
            return Convert.ToHexString(hashBytes).ToLower();
        }
    }
}