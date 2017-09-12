using System.Security.Cryptography;

namespace cryptopals.Lib.Crypto
{
    public static class SecureRng
    {
        public static byte[] GenerateRandomBytes(int numBytes)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[numBytes];
                rng.GetBytes(bytes);
                return bytes;
            }
        }
    }
}
